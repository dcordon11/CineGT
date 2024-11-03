CREATE PROCEDURE ListarSesionesRangoFecha
    @fecha_inicio DATETIME,
    @fecha_fin DATETIME
AS
BEGIN
    -- Selección de sesiones en el rango con detalles de la sesión, película y el conteo de asientos ocupados
    SELECT 
        s.id_sesion,
        s.fecha_hora_inicio,
        s.fecha_hora_fin,
        s.estado,
        p.id_pelicula,
        p.nombre AS nombre_pelicula,
        p.clasificacion AS clasificacion_pelicula,
        p.duracion AS duracion_pelicula,
        sala.id_sala,
        sala.nombre_sala,
        COUNT(at.id_transaccion) AS asientos_ocupados -- Contar asientos ocupados
    FROM 
        Sesion s
    INNER JOIN 
        Pelicula p ON s.id_pelicula = p.id_pelicula
    INNER JOIN 
        Sala sala ON s.id_sala = sala.id_sala
    LEFT JOIN 
        AsientoTransaccion at ON s.id_sesion = at.id_sesion
    WHERE 
        s.fecha_hora_inicio BETWEEN @fecha_inicio AND @fecha_fin
    GROUP BY 
        s.id_sesion, s.fecha_hora_inicio, s.fecha_hora_fin, s.estado,
        p.id_pelicula, p.nombre, p.clasificacion, p.duracion,
        sala.id_sala, sala.nombre_sala
    ORDER BY 
        s.fecha_hora_inicio;
END;

EXEC ListarSesionesRangoFecha '2024-09-01 00:00:00', '2024-12-01 23:59:59';
