--Reporte baja ocupación porcentajes
CREATE PROCEDURE ObtenerSesionesConBajaOcupacion
    @porcentaje_limite DECIMAL(5,2)  -- porcentaje de ocupación límite
AS
BEGIN
    -- Variables de control para obtener el rango de fechas
    DECLARE @fecha_inicio DATETIME = DATEADD(MONTH, -3, GETDATE());
    DECLARE @fecha_fin DATETIME = GETDATE();

    -- Consulta para obtener las sesiones con ocupación menor al porcentaje dado
    SELECT 
        s.id_sesion,
        s.id_sala,
        s.fecha_hora_inicio,
        s.fecha_hora_fin,
        p.nombre AS nombre_pelicula,
        (COUNT(at.id_transaccion) * 1.0 / sa.cantidad_asientos) * 100 AS porcentaje_ocupacion,
        COUNT(at.id_transaccion) AS asientos_ocupados,
        sa.cantidad_asientos
    FROM 
        Sesion s
    INNER JOIN 
        Pelicula p ON s.id_pelicula = p.id_pelicula
    INNER JOIN 
        Sala sa ON s.id_sala = sa.id_sala
    LEFT JOIN 
        AsientoTransaccion at ON s.id_sesion = at.id_sesion
    WHERE 
        s.fecha_hora_inicio BETWEEN @fecha_inicio AND @fecha_fin
    GROUP BY 
        s.id_sesion, s.id_sala, s.fecha_hora_inicio, s.fecha_hora_fin, 
        p.nombre, sa.cantidad_asientos
    HAVING 
        (COUNT(at.id_transaccion) * 1.0 / sa.cantidad_asientos) * 100 < @porcentaje_limite
    ORDER BY 
        porcentaje_ocupacion ASC;
END;


EXEC ObtenerSesionesConBajaOcupacion @porcentaje_limite = 22;
