--SP PARA COMPRA DE ASIENTOS

CREATE PROCEDURE RegistrarVentaAsientos
    @id_usuario INT,
    @id_sesion INT,
    @total_asientos INT,
    @tipo_asignacion VARCHAR(10),
    @asientos_ids VARCHAR(MAX) -- Lista de IDs de asiento separados por comas
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @fecha_hora_inicio DATETIME;
    DECLARE @id_sala INT;
    SELECT @id_sala = id_sala FROM Sesion WHERE id_sesion = @id_sesion;

    -- Validación: Verificar que todos los asientos pertenecen a la misma sala de la sesión
    IF EXISTS (
        SELECT 1
        FROM STRING_SPLIT(@asientos_ids, ',') AS ids
        JOIN Asiento AS a ON ids.value = a.id_asiento
        WHERE a.id_sala <> @id_sala
    )
    BEGIN
        RAISERROR ('Uno o más asientos no pertenecen a la sala asignada para esta sesión.', 16, 1);
        RETURN;
    END;

    -- Verificar la fecha y hora de inicio de la sesión
    SELECT @fecha_hora_inicio = fecha_hora_inicio
    FROM Sesion
    WHERE id_sesion = @id_sesion;

    -- Validar si la sesión ya ha iniciado
    IF @fecha_hora_inicio <= GETDATE()
    BEGIN
        RAISERROR('La sesión ya ha iniciado. No se puede completar la venta.', 16, 1);
        RETURN;
    END;

    -- Validación: Verificar que los asientos no estén ya vendidos en esta sesión
    IF EXISTS (
        SELECT 1
        FROM STRING_SPLIT(@asientos_ids, ',') AS ids
        JOIN AsientoTransaccion at ON at.id_asiento = CAST(ids.value AS INT)
        WHERE at.id_sesion = @id_sesion
    )
    BEGIN
        RAISERROR ('Uno o más asientos ya han sido vendidos en esta sesión.', 16, 1);
        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Insertar en la tabla Transaccion
        DECLARE @id_transaccion INT;
        INSERT INTO Transaccion (id_usuario, id_sesion, fecha_hora, total_asientos, tipo_asignacion)
        VALUES (@id_usuario, @id_sesion, GETDATE(), @total_asientos, @tipo_asignacion);
        
        SET @id_transaccion = SCOPE_IDENTITY();

        -- Insertar en la tabla AsientoTransaccion para cada asiento usando STRING_SPLIT
        INSERT INTO AsientoTransaccion (id_asiento, id_transaccion, id_sesion, id_sala, fila, numero)
        SELECT 
            CAST(value AS INT) AS id_asiento,
            @id_transaccion AS id_transaccion,
            @id_sesion AS id_sesion,
            s.id_sala,
            a.fila,
            a.numero
        FROM STRING_SPLIT(@asientos_ids, ',') AS ids
        JOIN Asiento a ON a.id_asiento = CAST(ids.value AS INT)
        JOIN Sesion s ON s.id_sesion = @id_sesion;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
