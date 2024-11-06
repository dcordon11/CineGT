-- TRIGGER LOG SESION

CREATE TRIGGER trg_LogSesion
ON Sesion
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @id_usuario INT;

    -- Obtener el id_usuario basado en el nombre de usuario actual del sistema
    SELECT @id_usuario = id_usuario FROM Usuario WHERE nombre_usuario = SYSTEM_USER;

    -- Validar que se haya encontrado un id_usuario
    IF @id_usuario IS NULL
    BEGIN
        RAISERROR ('Usuario no encontrado en la tabla Usuario.', 16, 1);
        RETURN;
    END

    -- Insertar log en caso de INSERT
    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO LogSesion (
            id_sesion, accion, fecha, id_usuario, datos_nuevos, 
            id_pelicula, nombre_pelicula, clasificacion_pelicula, duracion_pelicula,
            fecha_hora_inicio, fecha_hora_fin, estado
        )
        SELECT 
            i.id_sesion, 
            'INSERT', 
            GETDATE(), 
            @id_usuario, 
            CONCAT('id_pelicula: ', i.id_pelicula, ', fecha_hora_inicio: ', i.fecha_hora_inicio, ', fecha_hora_fin: ', i.fecha_hora_fin, ', estado: ', i.estado), 
            i.id_pelicula, 
            p.nombre, 
            p.clasificacion, 
            p.duracion, 
            i.fecha_hora_inicio, 
            i.fecha_hora_fin, 
            i.estado
        FROM 
            inserted i
        JOIN 
            Pelicula p ON i.id_pelicula = p.id_pelicula;
    END

    -- Insertar log en caso de UPDATE
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO LogSesion (
            id_sesion, accion, fecha, id_usuario, datos_anteriores, datos_nuevos, 
            id_pelicula, nombre_pelicula, clasificacion_pelicula, duracion_pelicula,
            fecha_hora_inicio, fecha_hora_fin, estado
        )
        SELECT 
            d.id_sesion, 
            'UPDATE', 
            GETDATE(), 
            @id_usuario, 
            CONCAT('id_pelicula: ', d.id_pelicula, ', fecha_hora_inicio: ', d.fecha_hora_inicio, ', fecha_hora_fin: ', d.fecha_hora_fin, ', estado: ', d.estado), 
            CONCAT('id_pelicula: ', i.id_pelicula, ', fecha_hora_inicio: ', i.fecha_hora_inicio, ', fecha_hora_fin: ', i.fecha_hora_fin, ', estado: ', i.estado),
            i.id_pelicula, 
            p.nombre, 
            p.clasificacion, 
            p.duracion, 
            i.fecha_hora_inicio, 
            i.fecha_hora_fin, 
            i.estado
        FROM 
            deleted d
        JOIN 
            inserted i ON d.id_sesion = i.id_sesion
        JOIN 
            Pelicula p ON i.id_pelicula = p.id_pelicula;
    END

    -- Insertar log en caso de DELETE
    IF NOT EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO LogSesion (
            id_sesion, accion, fecha, id_usuario, datos_anteriores, 
            id_pelicula, nombre_pelicula, clasificacion_pelicula, duracion_pelicula,
            fecha_hora_inicio, fecha_hora_fin, estado
        )
        SELECT 
            d.id_sesion, 
            'DELETE', 
            GETDATE(), 
            @id_usuario, 
            CONCAT('id_pelicula: ', d.id_pelicula, ', fecha_hora_inicio: ', d.fecha_hora_inicio, ', fecha_hora_fin: ', d.fecha_hora_fin, ', estado: ', d.estado),
            d.id_pelicula, 
            p.nombre, 
            p.clasificacion, 
            p.duracion, 
            d.fecha_hora_inicio, 
            d.fecha_hora_fin, 
            d.estado
        FROM 
            deleted d
        JOIN 
            Pelicula p ON d.id_pelicula = p.id_pelicula;
    END
END;
