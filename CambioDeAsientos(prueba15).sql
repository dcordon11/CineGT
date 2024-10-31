CREATE PROCEDURE CambiarAsientos 
    @id_transaccion INT,
    @id_sesion_destino INT,
    @asientos_nuevos NVARCHAR(MAX) -- Ejemplo: '1-5,1-6,2-3' (fila-numero separados por comas)
AS
BEGIN
    DECLARE @id_sesion_actual INT, @id_usuario INT, @fecha_hora_inicio DATETIME;
    
    -- Tabla temporal para almacenar los asientos nuevos
    DECLARE @tempAsientos TABLE (fila INT, numero INT);
    
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Paso 1: Obtener detalles de la transacción
        SELECT @id_sesion_actual = id_sesion, @id_usuario = id_usuario
        FROM Transaccion
        WHERE id_transaccion = @id_transaccion;

        IF @id_sesion_actual IS NULL
        BEGIN
            RAISERROR ('Transacción no encontrada', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Paso 2: Validar que la sesión destino aún no ha comenzado
        SELECT @fecha_hora_inicio = fecha_hora_inicio
        FROM Sesion
        WHERE id_sesion = @id_sesion_destino;

        IF @fecha_hora_inicio <= GETDATE()
        BEGIN
            RAISERROR ('La sesión destino ya ha comenzado', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Paso 3: Parsear el parámetro de asientos y llenar la tabla temporal
        DECLARE @fila INT, @numero INT, @pos INT, @asiento NVARCHAR(50);

        WHILE LEN(@asientos_nuevos) > 0
        BEGIN
            SET @pos = CHARINDEX(',', @asientos_nuevos);
            
            IF @pos = 0
                SET @pos = LEN(@asientos_nuevos) + 1;

            SET @asiento = LEFT(@asientos_nuevos, @pos - 1);
            SET @asientos_nuevos = SUBSTRING(@asientos_nuevos, @pos + 1, LEN(@asientos_nuevos) - @pos);

            SET @fila = CONVERT(INT, LEFT(@asiento, CHARINDEX('-', @asiento) - 1));
            SET @numero = CONVERT(INT, SUBSTRING(@asiento, CHARINDEX('-', @asiento) + 1, LEN(@asiento)));

            INSERT INTO @tempAsientos (fila, numero) VALUES (@fila, @numero);
        END

        -- Paso 4: Verificar la disponibilidad de los nuevos asientos
        IF EXISTS (
            SELECT 1 
            FROM AsientoTransaccion
            WHERE id_sesion = @id_sesion_destino
            AND (fila IN (SELECT fila FROM @tempAsientos) AND numero IN (SELECT numero FROM @tempAsientos))
        )
        BEGIN
            RAISERROR ('Algunos de los asientos solicitados ya están ocupados en la sesión destino', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Paso 5: Realizar el cambio de asientos
        DELETE FROM AsientoTransaccion
        WHERE id_transaccion = @id_transaccion
        AND id_sesion = @id_sesion_actual;

        -- Insertar los nuevos asientos en la sesión destino
        INSERT INTO AsientoTransaccion (id_transaccion, id_sesion, id_sala, fila, numero)
        SELECT @id_transaccion, @id_sesion_destino, Sesion.id_sala, fila, numero
        FROM @tempAsientos
        JOIN Sesion ON Sesion.id_sesion = @id_sesion_destino;

        -- Registrar en LogTransaccion el cambio
        INSERT INTO LogTransaccion (id_transaccion, accion, fecha, id_usuario, datos_anteriores, datos_nuevos)
        VALUES (
            @id_transaccion,
            'Cambio de asientos',
            GETDATE(),
            @id_usuario,
            CONCAT('Sesión:', @id_sesion_actual, ' Asientos:', (
                SELECT STRING_AGG(CONCAT(fila, '-', numero), ', ') 
                FROM AsientoTransaccion 
                WHERE id_transaccion = @id_transaccion 
                AND id_sesion = @id_sesion_actual
            )),
            CONCAT('Sesión:', @id_sesion_destino, ' Asientos:', (
                SELECT STRING_AGG(CONCAT(fila, '-', numero), ', ') 
                FROM @tempAsientos
            ))
        );

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1);
    END CATCH
END;
