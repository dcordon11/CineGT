--SP LLENADO AUTOMÁTICO

CREATE PROCEDURE ObtenerAsientosSecuencialesDisponibles
    @idSala INT,
    @idSesion INT,
    @cantidadAsientos INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@cantidadAsientos) id_asiento 
    FROM Asiento 
    WHERE id_sala = @idSala 
    AND id_asiento NOT IN (
        SELECT id_asiento 
        FROM AsientoTransaccion 
        WHERE id_sesion = @idSesion
    )
    ORDER BY fila ASC, numero ASC;
END;
