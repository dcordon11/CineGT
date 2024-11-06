---TRIGGER LOG TRANSACCION

CREATE TRIGGER trg_LogTransaccion
ON Transaccion
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
        INSERT INTO LogTransaccion (
            id_transaccion, accion, fecha, id_usuario, datos_nuevos, id_sesion, 
            fecha_hora, tipo_asignacion, id_pelicula, nombre_pelicula, clasificacion_pelicula, duracion_pelicula
        )
        SELECT 
            i.id_transaccion, 
            'INSERT', 
            GETDATE(), 
            @id_usuario, 
            CONCAT('id_sesion: ', i.id_sesion, ', total_asientos: ', i.total_asientos, ', tipo_asignacion: ', i.tipo_asignacion), 
            i.id_sesion, 
            i.fecha_hora, 
            i.tipo_asignacion, 
            p.id_pelicula, 
            p.nombre, 
            p.clasificacion, 
            p.duracion
        FROM 
            inserted i
        JOIN 
            Sesion s ON i.id_sesion = s.id_sesion
        JOIN 
            Pelicula p ON s.id_pelicula = p.id_pelicula;
    END

    -- Insertar log en caso de UPDATE
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO LogTransaccion (
            id_transaccion, accion, fecha, id_usuario, datos_anteriores, datos_nuevos, id_sesion, 
            fecha_hora, tipo_asignacion, id_pelicula, nombre_pelicula, clasificacion_pelicula, duracion_pelicula
        )
        SELECT 
            d.id_transaccion, 
            'UPDATE', 
            GETDATE(), 
            @id_usuario, 
            CONCAT('id_sesion: ', d.id_sesion, ', total_asientos: ', d.total_asientos, ', tipo_asignacion: ', d.tipo_asignacion), 
            CONCAT('id_sesion: ', i.id_sesion, ', total_asientos: ', i.total_asientos, ', tipo_asignacion: ', i.tipo_asignacion),
            i.id_sesion, 
            i.fecha_hora, 
            i.tipo_asignacion, 
            p.id_pelicula, 
            p.nombre, 
            p.clasificacion, 
            p.duracion
        FROM 
            deleted d
        JOIN 
            inserted i ON d.id_transaccion = i.id_transaccion
        JOIN 
            Sesion s ON i.id_sesion = s.id_sesion
        JOIN 
            Pelicula p ON s.id_pelicula = p.id_pelicula;
    END

    -- Insertar log en caso de DELETE
    IF NOT EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        INSERT INTO LogTransaccion (
            id_transaccion, accion, fecha, id_usuario, datos_anteriores, id_sesion, 
            fecha_hora, tipo_asignacion, id_pelicula, nombre_pelicula, clasificacion_pelicula, duracion_pelicula
        )
        SELECT 
            d.id_transaccion, 
            'DELETE', 
            GETDATE(), 
            @id_usuario, 
            CONCAT('id_sesion: ', d.id_sesion, ', total_asientos: ', d.total_asientos, ', tipo_asignacion: ', d.tipo_asignacion),
            d.id_sesion, 
            d.fecha_hora, 
            d.tipo_asignacion, 
            p.id_pelicula, 
            p.nombre, 
            p.clasificacion, 
            p.duracion
        FROM 
            deleted d
        JOIN 
            Sesion s ON d.id_sesion = s.id_sesion
        JOIN 
            Pelicula p ON s.id_pelicula = p.id_pelicula;
    END
END;
