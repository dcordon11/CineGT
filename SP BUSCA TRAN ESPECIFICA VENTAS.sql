--SP BUSCAR ASIENTOS EN ASIENTO TRANSACCIÓN SEGÚN ID
CREATE PROCEDURE BuscarAsientosPorTransaccionID
    @id_transaccion INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        AT.id_transaccion,
        AT.id_asiento,
        AT.id_sesion,
        AT.id_sala,
        A.fila,
        A.numero
    FROM 
        AsientoTransaccion AT
    INNER JOIN 
        Asiento A ON AT.id_asiento = A.id_asiento
    WHERE 
        AT.id_transaccion = @id_transaccion;
END;
