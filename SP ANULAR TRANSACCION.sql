--SP PARA ANULAR TRANSACCION
CREATE PROCEDURE AnularTransaccion
    @id_transaccion INT,
    @id_usuario INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Primero elimina los registros asociados en LogTransaccion
        DELETE FROM LogTransaccion
        WHERE id_transaccion = @id_transaccion;

        -- Luego elimina los registros de AsientoTransaccion
        DELETE FROM AsientoTransaccion
        WHERE id_transaccion = @id_transaccion;

        -- Finalmente, elimina el registro de la tabla Transaccion
        DELETE FROM Transaccion
        WHERE id_transaccion = @id_transaccion;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;  -- Lanza el error para que sea manejado en el código de C#
    END CATCH
END;
