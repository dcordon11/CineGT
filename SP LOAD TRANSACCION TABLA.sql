--LOAD TRANSACCION TABLA

CREATE PROCEDURE ListarTransaccionesConDetalles
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        T.id_transaccion, 
        T.fecha_hora AS fecha_hora_compra,
        T.total_asientos,
        T.tipo_asignacion,
        S.id_sesion,
        S.fecha_hora_inicio,
        S.fecha_hora_fin,
        P.nombre AS nombre_pelicula,
        SA.nombre_sala AS nombre_sala,
        U.nombre_usuario AS usuario
    FROM 
        Transaccion T
    INNER JOIN 
        Sesion S ON T.id_sesion = S.id_sesion
    INNER JOIN 
        Pelicula P ON S.id_pelicula = P.id_pelicula
    INNER JOIN 
        Sala SA ON S.id_sala = SA.id_sala
    INNER JOIN 
        Usuario U ON T.id_usuario = U.id_usuario
    ORDER BY 
        T.fecha_hora DESC;
END;

