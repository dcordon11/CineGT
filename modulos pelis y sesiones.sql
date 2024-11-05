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
    @fecha_hora_inicio DATETIME,
    @id_usuario INT -- Agregar el usuario para registrar en el Log
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

    -- Verificar si ya existe una sesi�n id�ntica
    IF EXISTS (
        SELECT 1 
        FROM Sesion
        WHERE id_pelicula = @id_pelicula
        AND id_sala = @id_sala
        AND fecha_hora_inicio = @fecha_hora_inicio
        AND fecha_hora_fin = @fecha_hora_fin
    )
    BEGIN
        PRINT 'Error: Ya existe una sesi�n id�ntica con los mismos detalles.';
        RETURN;
    END

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
        PRINT 'Error: Existe un traslape con otra pel�cula en la misma sala.';
        RETURN;
    END

    -- Verificar si hay al menos 15 minutos entre la sesi�n anterior y la nueva
    IF EXISTS (
        SELECT 1
        FROM Sesion
        WHERE id_sala = @id_sala
        AND estado = 'activa'
        AND (
            ABS(DATEDIFF(MINUTE, fecha_hora_fin, @fecha_hora_inicio)) < 15
            OR ABS(DATEDIFF(MINUTE, @fecha_hora_fin, fecha_hora_inicio)) < 15
        )
    )
    BEGIN
        PRINT 'Error: Debe haber al menos 15 minutos entre sesiones.';
        RETURN;
    END

    -- Insertar la nueva sesi�n
    INSERT INTO Sesion (id_pelicula, id_sala, fecha_hora_inicio, fecha_hora_fin, estado)
    VALUES (@id_pelicula, @id_sala, @fecha_hora_inicio, @fecha_hora_fin, 'activa');

    DECLARE @id_sesion INT = SCOPE_IDENTITY();

    -- Registrar en el log de sesiones
    INSERT INTO LogSesion (id_sesion, accion, fecha, id_usuario, datos_anteriores, datos_nuevos)
    VALUES (@id_sesion, 'Creaci�n de sesi�n', GETDATE(), @id_usuario, NULL, 
            'Pel�cula: ' + CAST(@id_pelicula AS VARCHAR) + 
            ', Sala: ' + CAST(@id_sala AS VARCHAR) + 
            ', Fecha y Hora Inicio: ' + CAST(@fecha_hora_inicio AS VARCHAR) + 
            ', Fecha y Hora Fin: ' + CAST(@fecha_hora_fin AS VARCHAR) + 
            ', Estado: activa');

    PRINT 'Sesi�n registrada exitosamente.';
END;




-- Crear el procedimiento para registrar sesiones desde un archivo CSV
ALTER PROCEDURE RegistrarSesionesDesdeCSV
    @ruta_archivo NVARCHAR(255),
    @revertir_todas BIT -- 1: Revertir todas si hay error, 0: Insertar solo v�lidas
AS
BEGIN
    -- Tabla temporal para almacenar los datos importados
    CREATE TABLE #TempSesion (
        id_pelicula INT,
        id_sala INT,
        fecha_hora_inicio DATETIME
    );

    -- Comando BULK INSERT din�mico
    DECLARE @sql NVARCHAR(MAX);
    SET @sql = N'
        BULK INSERT #TempSesion
        FROM ''' + @ruta_archivo + '''
        WITH (
            FIELDTERMINATOR = '','',  -- Asumiendo que el CSV est� separado por comas
            ROWTERMINATOR = ''\n'',
            FIRSTROW = 2  -- Saltar encabezados si los tiene
        );';

    BEGIN TRY
        EXEC sp_executesql @sql;
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
    DECLARE @insertados INT = 0; -- Contador de sesiones insertadas

    -- Cursor para iterar por cada sesi�n en la tabla temporal
    DECLARE sesion_cursor CURSOR FOR
    SELECT id_pelicula, id_sala, fecha_hora_inicio
    FROM #TempSesion;

    OPEN sesion_cursor;
    FETCH NEXT FROM sesion_cursor INTO @id_pelicula, @id_sala, @fecha_hora_inicio;

    -- Iniciar transacci�n
    BEGIN TRANSACTION;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT 'Procesando sesi�n para pel�cula ID ' + CAST(@id_pelicula AS NVARCHAR) 
            + ', sala ID ' + CAST(@id_sala AS NVARCHAR) 
            + ', inicio: ' + CAST(@fecha_hora_inicio AS NVARCHAR);

        -- Validar datos de la sesi�n
        SELECT @duracion = duracion FROM Pelicula WHERE id_pelicula = @id_pelicula;

        IF @duracion IS NULL
        BEGIN
            PRINT 'Error: Pel�cula no encontrada. Sesi�n ignorada.';
            SET @error_detectado = 1;
            FETCH NEXT FROM sesion_cursor INTO @id_pelicula, @id_sala, @fecha_hora_inicio;
            CONTINUE;
        END

        -- Calcular la fecha y hora de fin de la sesi�n
        SET @fecha_hora_fin = DATEADD(MINUTE, @duracion, @fecha_hora_inicio);

        -- Verificar si ya existe una sesi�n id�ntica
        IF EXISTS (
            SELECT 1 
            FROM Sesion
            WHERE id_pelicula = @id_pelicula
            AND id_sala = @id_sala
            AND fecha_hora_inicio = @fecha_hora_inicio
            AND fecha_hora_fin = @fecha_hora_fin
        )
        BEGIN
            PRINT 'Error: Sesi�n duplicada detectada. Sesi�n ignorada.';
            SET @error_detectado = 1;
            FETCH NEXT FROM sesion_cursor INTO @id_pelicula, @id_sala, @fecha_hora_inicio;
            CONTINUE;
        END

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
            PRINT 'Error: Traslape con otra sesi�n existente. Sesi�n ignorada.';
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
            AND (
                ABS(DATEDIFF(MINUTE, fecha_hora_fin, @fecha_hora_inicio)) < 15
                OR ABS(DATEDIFF(MINUTE, @fecha_hora_fin, fecha_hora_inicio)) < 15
            )
        )
        BEGIN
            PRINT 'Error: No hay suficiente tiempo entre sesiones. Sesi�n ignorada.';
            SET @error_detectado = 1;
            FETCH NEXT FROM sesion_cursor INTO @id_pelicula, @id_sala, @fecha_hora_inicio;
            CONTINUE;
        END

        -- Insertar sesi�n v�lida
        INSERT INTO Sesion (id_pelicula, id_sala, fecha_hora_inicio, fecha_hora_fin, estado)
        VALUES (@id_pelicula, @id_sala, @fecha_hora_inicio, @fecha_hora_fin, 'activa');

        SET @insertados = @insertados + 1; -- Incrementar contador de inserciones
        PRINT 'Sesi�n registrada exitosamente.';

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
        IF @insertados > 0
        BEGIN
            PRINT 'Sesiones procesadas exitosamente.';
        END
        ELSE
        BEGIN
            PRINT 'No se insertaron sesiones debido a errores en el archivo.';
        END
    END

    -- Eliminar la tabla temporal
    DROP TABLE #TempSesion;
END;


EXEC RegistrarSesionesDesdeCSV 
    @ruta_archivo = 'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\prueba2.csv', 
    @revertir_todas = 0; -- 1: Revertir todas si hay error, 0: Insertar solo v�lidas

	select * from Sesion
	select * from Pelicula
