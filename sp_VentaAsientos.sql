ALTER PROCEDURE RegistrarVentaAsientos
    @id_sesion INT,
    @id_usuario INT,
    @cantidad_asientos INT,
    @tipo_asignacion VARCHAR(10), -- 'automatica' o 'manual'
    @asientos_manual NVARCHAR(MAX) = NULL -- Formato: 'A1,A2,B1' (solo si es asignaci�n manual)
AS
BEGIN
    DECLARE @id_transaccion INT;
    DECLARE @id_sala INT;
    DECLARE @asiento NVARCHAR(10);
    DECLARE @fila CHAR(1);
    DECLARE @numero INT;
    DECLARE @contador INT = 0;

    BEGIN TRY
        -- Iniciar transacci�n
        BEGIN TRANSACTION;

        -- Obtener la sala de la sesi�n
        SELECT @id_sala = id_sala
        FROM Sesion
        WHERE id_sesion = @id_sesion;

        IF @id_sala IS NULL
        BEGIN
            PRINT 'Error: Sesi�n no encontrada.';
            ROLLBACK TRANSACTION;
            -- Registrar en el log como fallida
            INSERT INTO LogTransaccion (accion, fecha, id_usuario, datos_anteriores, datos_nuevos)
            VALUES ('Venta de Asientos Fallida', GETDATE(), @id_usuario, NULL, 'Error: Sesi�n no encontrada');
            RETURN;
        END

        -- Registrar la transacci�n de la venta
        INSERT INTO AsientoTransaccion (id_usuario, id_sesion, total_asientos, tipo_asignacion)
        VALUES (@id_usuario, @id_sesion, @cantidad_asientos, @tipo_asignacion);

        SET @id_transaccion = SCOPE_IDENTITY();

        -- Verificar el tipo de asignaci�n
        IF @tipo_asignacion = 'manual'
        BEGIN
            -- Procesar asientos indicados manualmente
            WHILE CHARINDEX(',', @asientos_manual) > 0
            BEGIN
                SET @asiento = LEFT(@asientos_manual, CHARINDEX(',', @asientos_manual) - 1);
                SET @asientos_manual = SUBSTRING(@asientos_manual, CHARINDEX(',', @asientos_manual) + 1, LEN(@asientos_manual));

                -- Extraer fila y n�mero del asiento
                SET @fila = LEFT(@asiento, 1); -- Asume que la fila es la primera letra
                SET @numero = CAST(SUBSTRING(@asiento, 2, LEN(@asiento) - 1) AS INT); -- El resto es el n�mero

                -- Verificar si el asiento est� disponible
                IF NOT EXISTS (
                    SELECT 1 
                    FROM Asiento a
                    LEFT JOIN Transaccion t ON a.fila = t.fila AND a.numero = t.numero AND t.id_sesion = @id_sesion
                    WHERE a.id_sala = @id_sala AND a.fila = @fila AND a.numero = @numero AND t.fila IS NULL AND t.numero IS NULL
                )
                BEGIN
                    PRINT 'Error: El asiento ' + @fila + CAST(@numero AS NVARCHAR) + ' no est� disponible.';
                    ROLLBACK TRANSACTION;
                    -- Registrar en el log como fallida
                    INSERT INTO LogTransaccion (id_transaccion, accion, fecha, id_usuario, datos_anteriores, datos_nuevos)
                    VALUES (@id_transaccion, 'Venta de Asientos Fallida', GETDATE(), @id_usuario, NULL, 'Error: El asiento ' + @fila + CAST(@numero AS NVARCHAR) + ' no est� disponible.');
                    RETURN;
                END

                -- Insertar asiento en la transacci�n
                INSERT INTO Transaccion (fila, numero, id_transaccion, id_sesion, id_sala)
                VALUES (@fila, @numero, @id_transaccion, @id_sesion, @id_sala);
                SET @contador = @contador + 1;
            END

            -- Verificar que el n�mero de asientos seleccionados manualmente coincida con el solicitado
            IF @contador <> @cantidad_asientos
            BEGIN
                PRINT 'Error: La cantidad de asientos seleccionados manualmente no coincide con la solicitada.';
                ROLLBACK TRANSACTION;
                -- Registrar en el log como fallida
                INSERT INTO LogTransaccion (id_transaccion, accion, fecha, id_usuario, datos_anteriores, datos_nuevos)
                VALUES (@id_transaccion, 'Venta de Asientos Fallida', GETDATE(), @id_usuario, NULL, 'Error: La cantidad de asientos seleccionados manualmente no coincide con la solicitada.');
                RETURN;
            END
        END
        ELSE IF @tipo_asignacion = 'automatica'
        BEGIN
            -- Verificar que haya suficientes asientos disponibles en la sala
            IF (SELECT COUNT(*) 
                FROM Asiento a
                LEFT JOIN Transaccion t ON a.fila = t.fila AND a.numero = t.numero AND t.id_sesion = @id_sesion
                WHERE a.id_sala = @id_sala AND t.fila IS NULL AND t.numero IS NULL) < @cantidad_asientos
            BEGIN
                PRINT 'Error: No hay suficientes asientos disponibles.';
                ROLLBACK TRANSACTION;
                -- Registrar en el log como fallida
                INSERT INTO LogTransaccion (id_transaccion, accion, fecha, id_usuario, datos_anteriores, datos_nuevos)
                VALUES (@id_transaccion, 'Venta de Asientos Fallida', GETDATE(), @id_usuario, NULL, 'Error: No hay suficientes asientos disponibles.');
                RETURN;
            END

            -- Asignar autom�ticamente los primeros asientos disponibles en la transacci�n
            INSERT INTO Transaccion (fila, numero, id_transaccion, id_sesion, id_sala)
            SELECT TOP(@cantidad_asientos) a.fila, a.numero, @id_transaccion, @id_sesion, @id_sala
            FROM Asiento a
            LEFT JOIN Transaccion t ON a.fila = t.fila AND a.numero = t.numero AND t.id_sesion = @id_sesion
            WHERE a.id_sala = @id_sala AND t.fila IS NULL AND t.numero IS NULL
            ORDER BY a.fila, a.numero;
        END
        ELSE
        BEGIN
            PRINT 'Error: Tipo de asignaci�n inv�lido.';
            ROLLBACK TRANSACTION;
            -- Registrar en el log como fallida
            INSERT INTO LogTransaccion (accion, fecha, id_usuario, datos_anteriores, datos_nuevos)
            VALUES ('Venta de Asientos Fallida', GETDATE(), @id_usuario, NULL, 'Error: Tipo de asignaci�n inv�lido.');
            RETURN;
        END

        -- Registrar en el log de transacciones como exitosa
        INSERT INTO LogTransaccion (id_transaccion, accion, fecha, id_usuario, datos_anteriores, datos_nuevos)
        VALUES (@id_transaccion, 'Venta de Asientos Exitosa', GETDATE(), @id_usuario, NULL, 
                'ID de transacci�n: ' + CAST(@id_transaccion AS VARCHAR) + 
                ', Asientos comprados: ' + CAST(@cantidad_asientos AS VARCHAR));

        -- Confirmar la transacci�n
        COMMIT TRANSACTION;

        PRINT 'Venta de asientos realizada exitosamente. ID de transacci�n: ' + CAST(@id_transaccion AS VARCHAR);
    END TRY
    BEGIN CATCH
        -- Si ocurre un error, hacer rollback de la transacci�n
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- Registrar en el log como fallida
        INSERT INTO LogTransaccion (accion, fecha, id_usuario, datos_anteriores, datos_nuevos)
        VALUES ('Venta de Asientos Fallida', GETDATE(), @id_usuario, NULL, 'Error en la transacci�n: ' + ERROR_MESSAGE());

        -- Mostrar el mensaje de error
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        PRINT 'Error en la transacci�n: ' + @ErrorMessage;
    END CATCH
END;
