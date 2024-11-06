USE [CineGT];

DECLARE @nombreUsuario NVARCHAR(100);
DECLARE @contrasena NVARCHAR(100);
DECLARE @rol NVARCHAR(50);

DECLARE usuarioCursor CURSOR FOR 
SELECT nombre_usuario, contraseña, rol FROM Usuario;

OPEN usuarioCursor;
FETCH NEXT FROM usuarioCursor INTO @nombreUsuario, @contrasena, @rol;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Crear inicio de sesión si no existe
    IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = @nombreUsuario)
    BEGIN
        DECLARE @sqlLogin NVARCHAR(MAX) = 'CREATE LOGIN [' + @nombreUsuario + '] WITH PASSWORD = ''' + @contrasena + '''';
        EXEC sp_executesql @sqlLogin;
    END

    -- Crear usuario de base de datos si no existe
    IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = @nombreUsuario)
    BEGIN
        DECLARE @sqlUser NVARCHAR(MAX) = 'CREATE USER [' + @nombreUsuario + '] FOR LOGIN [' + @nombreUsuario + ']';
        EXEC sp_executesql @sqlUser;
    END

    -- Asignar el rol adecuado en base al rol en la tabla Usuario
    IF @rol = 'admin'
    BEGIN
        EXEC sp_addrolemember 'db_admin', @nombreUsuario;
    END
    ELSE IF @rol = 'vendedor'
    BEGIN
        EXEC sp_addrolemember 'db_vendedor', @nombreUsuario;
    END

    FETCH NEXT FROM usuarioCursor INTO @nombreUsuario, @contrasena, @rol;
END;

CLOSE usuarioCursor;
DEALLOCATE usuarioCursor;
