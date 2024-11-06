-- SP ASIENTOS DISPONIBLES EN EL GRID

CREATE PROCEDURE ObtenerAsientosDisponiblesPorSesion
    @id_sesion INT,
    @id_sala INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        a.id_asiento, 
        a.id_sala, 
        a.fila, 
        a.numero 
    FROM 
        Asiento a
    LEFT JOIN AsientoTransaccion at ON a.id_asiento = at.id_asiento AND at.id_sesion = @id_sesion
    WHERE 
        a.id_sala = @id_sala 
        AND at.id_asiento IS NULL;  -- Solo asientos no vendidos en esta sesión
END;
