--Dada una sala, obtener el promedio de asientos ocupados
--y cantidad de sesiones por mes para los últimos 3 meses
DECLARE @idSala int
SET @idSala = 1

SELECT 
    MONTH(S.fecha_hora_inicio) AS mes,
    YEAR(S.fecha_hora_inicio) AS año,
    COUNT(S.id_sesion) AS cantidad_sesiones,
    AVG(AT.total_asientos) AS promedio_asientos_ocupados
FROM 
    Sala SA
JOIN 
    Sesion S ON S.id_sala = SA.id_sala
JOIN 
    AsientoTransaccion AT ON AT.id_sesion = S.id_sesion
JOIN 
    Transaccion T ON T.id_transaccion = AT.id_transaccion AND T.id_sala = SA.id_sala
WHERE 
    SA.id_sala = 1
    AND S.fecha_hora_inicio >= DATEADD(MONTH, -3, GETDATE())  -- Solo últimos 3 meses
GROUP BY 
    YEAR(S.fecha_hora_inicio), MONTH(S.fecha_hora_inicio)
ORDER BY 
    año DESC, mes DESC;



	WITH SesionesUltimosTresMeses AS (
    SELECT 
        s.id_sesion,
        s.id_sala,
        MONTH(s.fecha_hora_inicio) AS mes,
        YEAR(s.fecha_hora_inicio) AS anio,
        COUNT(t.id_transaccion) AS asientos_ocupados
    FROM 
        Sesion s
    LEFT JOIN 
        Transaccion t ON s.id_sesion = t.id_sesion AND s.id_sala = t.id_sala
    WHERE 
        s.id_sala = 1 AND 
        s.fecha_hora_inicio >= DATEADD(MONTH, -3, GETDATE())
    GROUP BY 
        s.id_sesion, s.id_sala, MONTH(s.fecha_hora_inicio), YEAR(s.fecha_hora_inicio)
)

SELECT 
    mes,
    anio,
    AVG(asientos_ocupados) AS promedio_asientos_ocupados,
    COUNT(id_sesion) AS cantidad_sesiones
FROM 
    SesionesUltimosTresMeses
GROUP BY  mes, anio
ORDER BY mes DESC





-- Sesiones con cantidad de asientos ocupados menor 
--a un porcentaje dado para los últimos 3 meses.

SELECT 
    s.id_sesion,
    s.id_sala,
    s.fecha_hora_inicio,
    s.fecha_hora_fin,
    COUNT(t.id_transaccion) AS asientos_ocupados,
    sa.cantidad_asientos,
    FORMAT((COUNT(t.id_transaccion) * 100.0 / sa.cantidad_asientos), 'N2') + '%' AS porcentaje_ocupado
FROM 
    Sesion s
JOIN 
    Sala sa ON s.id_sala = sa.id_sala
LEFT JOIN 
    Transaccion t ON s.id_sesion = t.id_sesion AND s.id_sala = t.id_sala
WHERE 
    s.fecha_hora_inicio >= DATEADD(MONTH, -3, GETDATE())
GROUP BY 
    s.id_sesion, s.id_sala, s.fecha_hora_inicio, s.fecha_hora_fin, sa.cantidad_asientos
HAVING 
    (COUNT(t.id_transaccion) * 100.0 / sa.cantidad_asientos) < 100
ORDER BY 
    s.fecha_hora_inicio DESC;

