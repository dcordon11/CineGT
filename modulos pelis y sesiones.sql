--MODULO PARA REGISTRAR PELÍCULA
CREATE PROCEDURE RegistrarPelicula 
	@nombre VARCHAR(100), 
	@clasificacion VARCHAR (100),
	@duracion INT,
	@descripcion TEXT
AS
BEGIN
	--Validar si la película ya existe
	IF EXISTS (SELECT 1 FROM Pelicula WHERE nombre = @nombre)
	BEGIN
		PRINT 'ERROR: La película con este nombre ya está registrada.'
	RETURN;
	END

	--Validar clasificación
	IF @clasificacion NOT IN ('G', 'PG', 'PG-13', 'R', 'NC-17')
	BEGIN
		PRINT 'ERROR: Clasificación inválidda.'
	RETURN;
	END

	--	Validar duración
	IF @duracion <= 0
	BEGIN
		PRINT 'ERROR: La duración de la película debe ser mayor a 0 mins.'
	RETURN;
	END

	-- Validar la descripción no puede estar vacía
    IF LEN(@descripcion) = 0
    BEGIN
        PRINT 'ERROR: La descripción no puede estar vacía.';
        RETURN;
    END

	--Si pasa las validaciones, registrar película
	INSERT INTO Pelicula(nombre, clasificacion, duracion, descripcion)
	VALUES (@nombre, @clasificacion, @duracion, @descripcion);

	PRINT 'Película: ' + @nombre + ' registrada.'

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

    -- Obtener la duración de la película
    SELECT @duracion = duracion FROM Pelicula WHERE id_pelicula = @id_pelicula;

    IF @duracion IS NULL
    BEGIN
        PRINT 'Error: Película no encontrada.';
        RETURN;
    END

    -- Calcular la fecha y hora de fin de la sesión
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

    -- Verificar si hay 15 minutos entre la sesión anterior y la nueva
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

    -- Insertar la nueva sesión
    INSERT INTO Sesion (id_pelicula, id_sala, fecha_hora_inicio, fecha_hora_fin, estado)
    VALUES (@id_pelicula, @id_sala, @fecha_hora_inicio, @fecha_hora_fin, 'activa');

    PRINT 'Sesión registrada exitosamente.';
END;




----
-- Crear el procedimiento para registrar sesiones desde un archivo CSV
CREATE PROCEDURE RegistrarSesionesDesdeCSV
    @ruta_archivo NVARCHAR(255),
    @revertir_todas BIT -- 1: Revertir todas si hay error, 0: Insertar solo válidas
AS
BEGIN
    -- Tabla temporal para almacenar los datos importados
    CREATE TABLE #TempSesion (
        id_pelicula INT,
        id_sala INT,
        fecha_hora_inicio DATETIME
    );

    -- Cargar el archivo CSV en la tabla temporal
    BEGIN TRY
        -- Cargar el archivo CSV a una tabla temporal o permanente antes de ejecutar el procedimiento
		BULK INSERT #TempSesion
		FROM 'C:\ruta_del_archivo\sessions.csv'
		WITH (
			FIELDTERMINATOR = ',',
			ROWTERMINATOR = '\n',
			FIRSTROW = 2
			);


    END TRY
    BEGIN CATCH
        PRINT 'Error al cargar el archivo CSV.';
        RETURN;
    END CATCH

    DECLARE @id_pelicula INT;
    DECLARE @id_sala INT;
    DECLARE @fecha_hora_inicio DATETIME;
    DECLARE @duracion INT;
    DECLARE @fecha_hora_fin DATETIME;
    DECLARE @error_detectado BIT = 0;

    -- Cursor para iterar por cada sesión en la tabla temporal
    DECLARE sesion_cursor CURSOR FOR
    SELECT id_pelicula, id_sala, fecha_hora_inicio
    FROM #TempSesion;

    OPEN sesion_cursor;
    FETCH NEXT FROM sesion_cursor INTO @id_pelicula, @id_sala, @fecha_hora_inicio;

    -- Iniciar transacción
    BEGIN TRANSACTION;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Validar datos de la sesión
        SELECT @duracion = duracion FROM Pelicula WHERE id_pelicula = @id_pelicula;

        IF @duracion IS NULL
        BEGIN
            PRINT 'Error: Película no encontrada. Sesión ignorada.';
            SET @error_detectado = 1;
            FETCH NEXT FROM sesion_cursor INTO @id_pelicula, @id_sala, @fecha_hora_inicio;
            CONTINUE;
        END

        -- Calcular la fecha y hora de fin de la sesión
        SET @fecha_hora_fin = DATEADD(MINUTE, @duracion, @fecha_hora_inicio);

        -- Verificar traslape de sesiones
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
            PRINT 'Error: Traslape con otra sesión existente. Sesión ignorada.';
            SET @error_detectado = 1;
            FETCH NEXT FROM sesion_cursor INTO @id_pelicula, @id_sala, @fecha_hora_inicio;
            CONTINUE;
        END

        -- Verificar intervalo de 15 minutos entre sesiones
        IF EXISTS (
            SELECT 1
            FROM Sesion
            WHERE id_sala = @id_sala
            AND estado = 'activa'
            AND ABS(DATEDIFF(MINUTE, fecha_hora_fin, @fecha_hora_inicio)) < 15
        )
        BEGIN
            PRINT 'Error: No hay suficiente tiempo entre sesiones. Sesión ignorada.';
            SET @error_detectado = 1;
            FETCH NEXT FROM sesion_cursor INTO @id_pelicula, @id_sala, @fecha_hora_inicio;
            CONTINUE;
        END

        -- Insertar sesión válida
        INSERT INTO Sesion (id_pelicula, id_sala, fecha_hora_inicio, fecha_hora_fin, estado)
        VALUES (@id_pelicula, @id_sala, @fecha_hora_inicio, @fecha_hora_fin, 'activa');

        PRINT 'Sesión registrada exitosamente.';

        -- Avanzar al siguiente registro
        FETCH NEXT FROM sesion_cursor INTO @id_pelicula, @id_sala, @fecha_hora_inicio;
    END

    -- Cerrar y liberar el cursor
    CLOSE sesion_cursor;
    DEALLOCATE sesion_cursor;

    -- Revisar si hubo errores y revertir si es necesario
    IF @error_detectado = 1 AND @revertir_todas = 1
    BEGIN
        PRINT 'Error detectado en el archivo. Revirtiendo todas las inserciones de sesiones.';
        ROLLBACK TRANSACTION;
    END
    ELSE
    BEGIN
        COMMIT TRANSACTION;
        PRINT 'Sesiones procesadas exitosamente.';
    END

    -- Eliminar la tabla temporal
    DROP TABLE #TempSesion;
END;

