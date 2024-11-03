CREATE PROCEDURE RegistrarLogTransaccion
    @id_transaccion INT,
    @accion VARCHAR(50),
    @id_usuario INT,
    @datos_anteriores TEXT = NULL,
    @datos_nuevos TEXT = NULL
AS
BEGIN
    DECLARE @id_sesion INT, @id_pelicula INT, @nombre_pelicula VARCHAR(100),
            @clasificacion_pelicula VARCHAR(10), @duracion_pelicula INT;

    -- Obtener detalles de la sesión y película relacionadas con la transacción
    SELECT 
        @id_sesion = t.id_sesion,
        @id_pelicula = s.id_pelicula,
        @nombre_pelicula = p.nombre,
        @clasificacion_pelicula = p.clasificacion,
        @duracion_pelicula = p.duracion
    FROM 
        Transaccion t
    INNER JOIN 
        Sesion s ON t.id_sesion = s.id_sesion
    INNER JOIN 
        Pelicula p ON s.id_pelicula = p.id_pelicula
    WHERE 
        t.id_transaccion = @id_transaccion;

    -- Insertar en LogTransaccion
    INSERT INTO LogTransaccion (
        id_transaccion, accion, fecha, id_usuario, datos_anteriores, datos_nuevos,
        id_sesion, id_pelicula, nombre_pelicula, clasificacion_pelicula, duracion_pelicula
    )
    VALUES (
        @id_transaccion, @accion, GETDATE(), @id_usuario, @datos_anteriores, @datos_nuevos,
        @id_sesion, @id_pelicula, @nombre_pelicula, @clasificacion_pelicula, @duracion_pelicula
    );
END;

--Prueba

INSERT INTO Pelicula (nombre, clasificacion, duracion, descripcion)
VALUES ('Película de Prueba', 'PG-13', 120, 'Descripción de la película de prueba');

-- Asegúrate de que el `id_pelicula` y `id_sala` existan en las tablas respectivas
INSERT INTO Sesion (id_pelicula, id_sala, fecha_hora_inicio, fecha_hora_fin, estado)
VALUES (1, 1, '2024-11-20 15:00:00', '2024-11-20 17:00:00', 'activa');

INSERT INTO Usuario (nombre_usuario, contraseña, rol)
VALUES ('UsuarioPrueba2', 'hashedpassword', 'admin');

-- Asegúrate de que el `id_usuario` y `id_sesion` existan en sus respectivas tablas
INSERT INTO Transaccion (id_usuario, id_sesion, fecha_hora, total_asientos, tipo_asignacion)
VALUES (1, 1, '2024-11-20 14:45:00', 2, 'manual');

EXEC RegistrarLogTransaccion
    @id_transaccion = 1,
    @accion = 'Creación de Transacción',
    @id_usuario = 1,
    @datos_anteriores = NULL,
    @datos_nuevos = 'Transacción registrada en el sistema';

SELECT * FROM LogTransaccion WHERE id_transaccion = 1;
