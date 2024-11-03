CREATE PROCEDURE ListarTransaccionesRangoFecha
    @fecha_inicio DATETIME,
    @fecha_fin DATETIME
AS
BEGIN
    -- Selección de transacciones en el rango con detalles de la transacción, sesión y conteo de asientos
    SELECT 
        t.id_transaccion,
        t.fecha_hora AS fecha_hora_compra,
        t.total_asientos,
        t.tipo_asignacion,
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
        COUNT(at.fila) AS cantidad_asientos -- Contar asientos por transacción
    FROM 
        Transaccion t
    INNER JOIN 
        Sesion s ON t.id_sesion = s.id_sesion
    INNER JOIN 
        Pelicula p ON s.id_pelicula = p.id_pelicula
    INNER JOIN 
        Sala sala ON s.id_sala = sala.id_sala
    LEFT JOIN 
        AsientoTransaccion at ON t.id_transaccion = at.id_transaccion
    WHERE 
        t.fecha_hora BETWEEN @fecha_inicio AND @fecha_fin
    GROUP BY 
        t.id_transaccion, t.fecha_hora, t.total_asientos, t.tipo_asignacion,
        s.id_sesion, s.fecha_hora_inicio, s.fecha_hora_fin, s.estado,
        p.id_pelicula, p.nombre, p.clasificacion, p.duracion,
        sala.id_sala, sala.nombre_sala
    ORDER BY 
        t.fecha_hora;
END;



EXEC ListarTransaccionesRangoFecha '2024-09-01 00:00:00', '2024-12-01 23:59:59';
