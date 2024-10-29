--MODULO PARA REGISTRAR PEL�CULA
CREATE PROCEDURE RegistrarPelicula 
	@nombre VARCHAR(100), 
	@clasificacion VARCHAR (100),
	@duracion INT,
	@descripcion TEXT
AS
BEGIN
	--Validar si la pel�cula ya existe
	IF EXISTS (SELECT 1 FROM Pelicula WHERE nombre = @nombre)
	BEGIN
		PRINT 'ERROR: La pel�cula con este nombre ya est� registrada.'
	RETURN;
	END

	--Validar clasificaci�n
	IF @clasificacion NOT IN ('G', 'PG', 'PG-13', 'R', 'NC-17')
	BEGIN
		PRINT 'ERROR: Clasificaci�n inv�lidda.'
	RETURN;
	END

	--	Validar duraci�n
	IF @duracion <= 0
	BEGIN
		PRINT 'ERROR: La duraci�n de la pel�cula debe ser mayor a 0 mins.'
	RETURN;
	END

	-- Validar la descripci�n no puede estar vac�a
    IF LEN(@descripcion) = 0
    BEGIN
        PRINT 'ERROR: La descripci�n no puede estar vac�a.';
        RETURN;
    END

	--Si pasa las validaciones, registrar pel�cula
	INSERT INTO Pelicula(nombre, clasificacion, duracion, descripcion)
	VALUES (@nombre, @clasificacion, @duracion, @descripcion);

	PRINT 'Pel�cula: ' + @nombre + ' registrada.'

END;

--MODULO PARA REGISTRAR SESIONES
CREATE PROCEDURE RegistrarSesion
    @id_pelicula INT,
    @id_sala INT,
    @fecha_hora_inicio DATETIME
AS
BEGIN
    DECLARE @duracion INT;
    DECLARE @fecha_hora_fin DATETIME;

    -- Obtener la duraci�n de la pel�cula
    SELECT @duracion = duracion FROM Pelicula WHERE id_pelicula = @id_pelicula;

    IF @duracion IS NULL
    BEGIN
        PRINT 'Error: Pel�cula no encontrada.';
        RETURN;
    END

    -- Calcular la fecha y hora de fin de la sesi�n
    SET @fecha_hora_fin = DATEADD(MINUTE, @duracion, @fecha_hora_inicio);

    -- Verificar si la sala tiene sesiones traslapadas
    IF EXISTS (
        SELECT 1 
        FROM Sesion
        WHERE id_sala = @id_sala
        AND estado = 'activa'
        AND (
            (@fecha_hora_inicio BETWEEN fecha_hora_inicio AND fecha_hora_fin)
            OR (@fecha_hora_fin BETWEEN fecha_hora_inicio AND fecha_hora_fin)
            OR (fecha_hora_inicio BETWEEN @fecha_hora_inicio AND @fecha_hora_fin)
        )
    )
    BEGIN
        PRINT 'Error: Existe un traslape con otra pelicula en la misma sala.';
        RETURN;
    END

    -- Verificar si hay 15 minutos entre la sesi�n anterior y la nueva
    IF EXISTS (
        SELECT 1
        FROM Sesion
        WHERE id_sala = @id_sala
        AND estado = 'activa'
        AND ABS(DATEDIFF(MINUTE, fecha_hora_fin, @fecha_hora_inicio)) < 15
    )
    BEGIN
        PRINT 'Error: Debe haber al menos 15 minutos entre sesiones.';
        RETURN;
    END

    -- Insertar la nueva sesi�n
    INSERT INTO Sesion (id_pelicula, id_sala, fecha_hora_inicio, fecha_hora_fin, estado)
    VALUES (@id_pelicula, @id_sala, @fecha_hora_inicio, @fecha_hora_fin, 'activa');

    PRINT 'Sesi�n registrada exitosamente.';
END;


BULK INSERT TempSesion
FROM 'C:\path\to\sesiones.csv'
WITH (
    FIELDTERMINATOR = ',',  -- Asumiendo que el CSV est� separado por comas
    ROWTERMINATOR = '\n',
    FIRSTROW = 2  -- Saltar encabezados si los tiene
);
