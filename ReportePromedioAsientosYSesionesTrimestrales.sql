--Obtener el Promedio de Asientos y Sesiones Por Mes en el último trimestre
CREATE PROCEDURE PromedioAsientosYSesionesTrimestrales
    @id_sala INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Calcular el rango de fechas para los últimos 3 meses
    DECLARE @fecha_inicio DATE = DATEADD(MONTH, -3, GETDATE());
    DECLARE @fecha_fin DATE = GETDATE();

    -- Consulta para obtener el promedio de asientos ocupados y la cantidad de sesiones
    SELECT 
        YEAR(s.fecha_hora_inicio) AS Año,
        MONTH(s.fecha_hora_inicio) AS Mes,
        COUNT(s.id_sesion) AS Total_Sesiones,
        AVG(oc.Asientos_Ocupados) AS Promedio_Asientos_Ocupados
    FROM 
        Sesion s
    INNER JOIN 
        Sala sala ON s.id_sala = sala.id_sala
    LEFT JOIN (
        -- Subconsulta para contar los asientos ocupados en cada sesión
        SELECT 
            id_sesion,
            COUNT(at.id_transaccion) AS Asientos_Ocupados
        FROM 
            AsientoTransaccion at
        GROUP BY 
            id_sesion
    ) oc ON s.id_sesion = oc.id_sesion
    WHERE 
        s.id_sala = @id_sala
        AND s.fecha_hora_inicio BETWEEN @fecha_inicio AND @fecha_fin
    GROUP BY 
        YEAR(s.fecha_hora_inicio),
        MONTH(s.fecha_hora_inicio)
    ORDER BY 
        Año, Mes;
END;

EXEC PromedioAsientosYSesionesTrimestrales @id_sala = 1;
