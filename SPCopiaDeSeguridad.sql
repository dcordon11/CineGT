--Creacion de copia de seguridad para guardar la ruta
DECLARE @FileName NVARCHAR(100) = 'C:\Ruta\NombreDeTuBaseDeDatos_' 
    + FORMAT(GETDATE(), 'yyyyMMdd_HHmmss') + '.bak';

BACKUP DATABASE NombreDeTuBaseDeDatos 
TO DISK = @FileName
WITH FORMAT, INIT;

--copia de seguridad automatica
BACKUP DATABASE NombreDeTuBaseDeDatos 
TO DISK = 'C:\Ruta\NombreDeTuBaseDeDatos.bak'
WITH FORMAT, INIT;
