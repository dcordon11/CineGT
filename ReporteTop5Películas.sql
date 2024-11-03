--Reporte top 5 películas con más asientos
CREATE PROCEDURE Top5PeliculasOcupacion
AS
BEGIN
    -- Rango de fechas para el último trimestre (últimos 3 meses)
    DECLARE @fecha_inicio DATETIME = DATEADD(MONTH, -3, GETDATE());
    DECLARE @fecha_fin DATETIME = GETDATE();

    -- Consulta para obtener el top 5 de películas con mayor ocupación promedio
    SELECT TOP 5 
        p.nombre AS nombre_pelicula,
        AVG(ocupacion.porcentaje_ocupacion) AS promedio_ocupacion_porcentaje
    FROM 
        Pelicula p
    INNER JOIN 
        Sesion s ON p.id_pelicula = s.id_pelicula
    INNER JOIN 
        Sala sa ON s.id_sala = sa.id_sala
    LEFT JOIN (
        -- Subconsulta para obtener el porcentaje de ocupación por sesión
        SELECT 
            s.id_sesion,
            (COUNT(at.id_transaccion) * 1.0 / NULLIF(sa.cantidad_asientos, 0)) * 100 AS porcentaje_ocupacion
        FROM 
            Sesion s
        INNER JOIN 
            Sala sa ON s.id_sala = sa.id_sala
        LEFT JOIN 
            AsientoTransaccion at ON s.id_sesion = at.id_sesion
        WHERE 
            s.fecha_hora_inicio BETWEEN @fecha_inicio AND @fecha_fin
        GROUP BY 
            s.id_sesion, sa.cantidad_asientos
    ) AS ocupacion ON s.id_sesion = ocupacion.id_sesion
    WHERE 
        s.fecha_hora_inicio BETWEEN @fecha_inicio AND @fecha_fin
    GROUP BY 
        p.nombre
    ORDER BY 
        promedio_ocupacion_porcentaje DESC;
END;


EXEC Top5PeliculasOcupacion;
