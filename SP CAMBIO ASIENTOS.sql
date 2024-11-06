-- SP CAMBIO DE ASIENTOS

CREATE PROCEDURE CambiarAsientosTransaccion
    @id_transaccion INT,
    @id_sesion_destino INT,
    @id_usuario INT,
    @nuevos_asientos NVARCHAR(MAX)  -- IDs de asientos separados por comas
AS
BEGIN
    SET NOCOUNT ON;

    -- Declarar variables para controlar la transacción
    DECLARE @fecha_hora_inicio DATETIME;
    DECLARE @error INT;
	DECLARE @id_sala INT;

    SELECT @id_sala = id_sala FROM Sesion WHERE id_sesion = @id_sesion_destino;

    -- Validación: Verificar que todos los nuevos asientos pertenecen a la sala de la sesión destino
    IF EXISTS (
        SELECT 1
        FROM STRING_SPLIT(@nuevos_asientos, ',') AS ids
        JOIN Asiento AS a ON ids.value = a.id_asiento
        WHERE a.id_sala <> @id_sala
    )
    BEGIN
        RAISERROR ('Uno o más asientos no pertenecen a la sala de la sesión destino.', 16, 1);
        RETURN;
    END;

    -- Iniciar transacción
    BEGIN TRANSACTION;

    -- Obtener fecha de inicio de la sesión de destino para verificar que no haya comenzado
    SELECT @fecha_hora_inicio = fecha_hora_inicio
    FROM Sesion
    WHERE id_sesion = @id_sesion_destino;

    -- Verificar si la sesión de destino ya comenzó
    IF @fecha_hora_inicio <= GETDATE()
    BEGIN
        RAISERROR('La sesión de destino ya ha comenzado. No es posible cambiar los asientos.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Eliminar los asientos actuales de la transacción en AsientoTransaccion
    DELETE FROM AsientoTransaccion
    WHERE id_transaccion = @id_transaccion;

    -- Convertir el parámetro @nuevos_asientos en una tabla para insertar los nuevos asientos
    DECLARE @Asientos TABLE (id_asiento INT);

    INSERT INTO @Asientos (id_asiento)
    SELECT value
    FROM STRING_SPLIT(@nuevos_asientos, ',');

    -- Insertar los nuevos asientos en AsientoTransaccion
    INSERT INTO AsientoTransaccion (id_asiento, id_transaccion, id_sesion, id_sala, fila, numero)
    SELECT 
        A.id_asiento, 
        @id_transaccion, 
        @id_sesion_destino, 
        A.id_sala, 
        A.fila, 
        A.numero
    FROM 
        Asiento A
    INNER JOIN 
        @Asientos AS AT ON A.id_asiento = AT.id_asiento;

    -- Actualizar la transacción en la tabla Transaccion con la nueva sesión y total de asientos
    UPDATE Transaccion
    SET 
        id_sesion = @id_sesion_destino,
        total_asientos = (SELECT COUNT(*) FROM @Asientos),
        fecha_hora = GETDATE()
    WHERE 
        id_transaccion = @id_transaccion;

    -- Verificar errores
    SET @error = @@ERROR;
    IF @error <> 0
    BEGIN
        ROLLBACK TRANSACTION;
        RAISERROR('Ocurrió un error al actualizar la transacción.', 16, 1);
        RETURN;
    END

    -- Confirmar transacción
    COMMIT TRANSACTION;
END;


