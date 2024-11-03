--LOG SESIONES
CREATE PROCEDURE RegistrarLogSesion
    @id_sesion INT,
    @accion VARCHAR(50),
    @id_usuario INT,
    @datos_anteriores TEXT = NULL,
    @datos_nuevos TEXT = NULL
AS
BEGIN
    DECLARE @id_pelicula INT, @nombre_pelicula VARCHAR(100), 
            @clasificacion_pelicula VARCHAR(10), @duracion_pelicula INT,
            @fecha_hora_inicio DATETIME, @fecha_hora_fin DATETIME;

    -- Obtener detalles de la sesi�n y pel�cula relacionadas
    SELECT 
        @id_pelicula = s.id_pelicula,
        @nombre_pelicula = p.nombre,
        @clasificacion_pelicula = p.clasificacion,
        @duracion_pelicula = p.duracion,
        @fecha_hora_inicio = s.fecha_hora_inicio,
        @fecha_hora_fin = s.fecha_hora_fin
    FROM 
        Sesion s
    INNER JOIN 
        Pelicula p ON s.id_pelicula = p.id_pelicula
    WHERE 
        s.id_sesion = @id_sesion;

    -- Insertar en LogSesion
    INSERT INTO LogSesion (
        id_sesion, accion, fecha, id_usuario, datos_anteriores, datos_nuevos,
        id_pelicula, nombre_pelicula, clasificacion_pelicula, duracion_pelicula, 
        fecha_hora_inicio, fecha_hora_fin
    )
    VALUES (
        @id_sesion, @accion, GETDATE(), @id_usuario, @datos_anteriores, @datos_nuevos,
        @id_pelicula, @nombre_pelicula, @clasificacion_pelicula, @duracion_pelicula, 
        @fecha_hora_inicio, @fecha_hora_fin
    );
END;

--Prueba

INSERT INTO Pelicula (nombre, clasificacion, duracion, descripcion)
VALUES ('Pel�cula de Prueba', 'PG', 120, 'Descripci�n de prueba');

-- Aseg�rate de que id_pelicula y id_sala existan previamente en sus respectivas tablas
INSERT INTO Sesion (id_pelicula, id_sala, fecha_hora_inicio, fecha_hora_fin, estado)
VALUES (1, 1, '2024-11-15 18:00:00', '2024-11-15 20:00:00', 'activa');

INSERT INTO Usuario (nombre_usuario, contrase�a, rol)
VALUES ('UsuarioPrueba', 'hashedpassword', 'admin');

EXEC RegistrarLogSesion
    @id_sesion = 1,
    @accion = 'Creaci�n',
    @id_usuario = 1,
    @datos_anteriores = NULL,
    @datos_nuevos = 'Nueva sesi�n creada en el sistema';

SELECT * FROM LogSesion WHERE id_sesion = 1;
