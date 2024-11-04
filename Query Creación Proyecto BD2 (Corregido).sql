--PROYECTO BD2 

--CREACIÓN DE BASE DE DATOS Y TABLAS

CREATE DATABASE CineGT
GO

USE CineGT
GO
/*
DROP TABLE Usuario
DROP TABLE Sala
DROP TABLE Pelicula
DROP TABLE Asiento
DROP TABLE Sesion
DROP TABLE Transaccion
DROP TABLE AsientoTransaccion
DROP TABLE LogTransaccion
DROP TABLE LogSesion

DELETE FROM  Usuario
DELETE FROM  Sala
DELETE FROM  Pelicula
DELETE FROM  Asiento
DELETE FROM  Sesion
DELETE FROM  Transaccion
DELETE FROM  AsientoTransaccion
DELETE FROM  LogTransaccion
DELETE FROM  LogSesion
*/
-- Tabla de Usuarios con roles admin y vendedor
CREATE TABLE Usuario (
    id_usuario INT PRIMARY KEY IDENTITY(1,1),
    nombre_usuario VARCHAR(50) NOT NULL UNIQUE,
    contraseña VARCHAR(255) NOT NULL,
    rol VARCHAR(20) CHECK (rol IN ('admin', 'vendedor')) NOT NULL --Podrían crearse más
);


-- Creación de la tabla Sala sin el contador de asientos
CREATE TABLE Sala (
    id_sala INT PRIMARY KEY IDENTITY(1,1),
    nombre_sala VARCHAR(50) NOT NULL,
    cantidad_asientos INT NOT NULL DEFAULT 60 -- Capacidad de 60 asientos por sala
);


-- Tabla de Pelicula
CREATE TABLE Pelicula (
    id_pelicula INT PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    clasificacion VARCHAR(10) NOT NULL,
    duracion INT NOT NULL,  
    descripcion TEXT
);


-- Creación de la tabla Asiento con id_asiento, id_sala y fila, numero como llaves primarias compuestas
CREATE TABLE Asiento (
    id_asiento INT IDENTITY(1,1),  -- Identificador único para cada asiento
    id_sala INT NOT NULL,          -- Relación con la sala
    fila CHAR(1) NOT NULL,         -- Fila (A, B, C...)
    numero INT NOT NULL,           -- Número de asiento en la fila (1-10)
    CONSTRAINT PK_Asiento PRIMARY KEY (id_sala, fila, numero), -- Llave primaria compuesta de id_sala, fila y numero
    UNIQUE (id_asiento),           -- id_asiento como identificador único
    FOREIGN KEY (id_sala) REFERENCES Sala(id_sala) -- Llave foránea hacia la tabla Sala
);



-- Tabla de Sesion
CREATE TABLE Sesion (
    id_sesion INT PRIMARY KEY IDENTITY(1,1),
    id_pelicula INT NOT NULL,
    id_sala INT NOT NULL,             --Sala a la que está asignada
    fecha_hora_inicio DATETIME NOT NULL,
    fecha_hora_fin DATETIME NOT NULL,
    estado VARCHAR(10) CHECK (estado IN ('activa', 'inactiva')) NOT NULL,
    FOREIGN KEY (id_pelicula) REFERENCES Pelicula(id_pelicula),
    FOREIGN KEY (id_sala) REFERENCES Sala(id_sala)
);

-- Creación de la tabla Transaccion sin la columna numero_transaccion
CREATE TABLE Transaccion (
    id_transaccion INT PRIMARY KEY IDENTITY(1,1), -- Clave primaria autoincremental
    id_usuario INT NOT NULL,                      -- Relación con la tabla Usuario
    id_sesion INT NOT NULL,                       -- Relación con la tabla Sesion
    fecha_hora DATETIME NOT NULL DEFAULT GETDATE(), -- Fecha y hora de la transacción
    total_asientos INT NOT NULL,                  -- Cantidad total de asientos comprados
    tipo_asignacion VARCHAR(10) CHECK (tipo_asignacion IN ('automatica', 'manual')) NOT NULL, -- Tipo de asignación
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario), -- Clave foránea con la tabla Usuario
    FOREIGN KEY (id_sesion) REFERENCES Sesion(id_sesion)    -- Clave foránea con la tabla Sesion
);


-- Creación de la tabla AsientoTransaccion referenciando correctamente Asiento (incluyendo id_sala)
CREATE TABLE AsientoTransaccion (
    id_transaccion INT NOT NULL,      -- Relación con la transacción
    id_sesion INT NOT NULL,           -- Relación con la sesión
    id_sala INT NOT NULL,             -- Relación con la sala
    fila CHAR(1) NOT NULL,            -- Fila del asiento
    numero INT NOT NULL,              -- Número del asiento en la fila
    PRIMARY KEY (id_transaccion, id_sesion, id_sala, fila, numero), -- Llave primaria compuesta
    FOREIGN KEY (id_sala, fila, numero) REFERENCES Asiento(id_sala, fila, numero), -- Llave foránea hacia Asiento
    FOREIGN KEY (id_transaccion) REFERENCES Transaccion(id_transaccion), -- Llave foránea hacia Transaccion
    FOREIGN KEY (id_sesion) REFERENCES Sesion(id_sesion),           -- Llave foránea hacia Sesion
    FOREIGN KEY (id_sala) REFERENCES Sala(id_sala)                  -- Llave foránea hacia Sala
);

-- Creación de la tabla LogTransaccion con los detalles de transacción, sesión y película
CREATE TABLE LogTransaccion (
    id_log INT PRIMARY KEY IDENTITY(1,1),
    id_transaccion INT,                       -- Identificador de la transacción
    accion VARCHAR(50) NOT NULL,              -- Tipo de acción realizada
    fecha DATETIME NOT NULL DEFAULT GETDATE(), -- Fecha y hora del log
    id_usuario INT NOT NULL,                  -- Usuario que realizó la acción
    datos_anteriores TEXT,                    -- Datos antes de la acción
    datos_nuevos TEXT,                        -- Datos después de la acción
    id_sesion INT,                            -- Identificador de la sesión en la transacción
    fecha_hora DATETIME,                      -- Fecha y hora de la transacción
    tipo_asignacion VARCHAR(10),              -- Tipo de asignación (automática o manual)
    id_pelicula INT,                          -- Identificador de la película
    nombre_pelicula VARCHAR(100),             -- Nombre de la película
    clasificacion_pelicula VARCHAR(10),       -- Clasificación de la película
    duracion_pelicula INT,                    -- Duración de la película en minutos
    FOREIGN KEY (id_transaccion) REFERENCES Transaccion(id_transaccion),
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario),
    FOREIGN KEY (id_sesion) REFERENCES Sesion(id_sesion)
);


-- Creación de la tabla LogSesion con los detalles de sesión y película
CREATE TABLE LogSesion (
    id_log INT PRIMARY KEY IDENTITY(1,1),
    id_sesion INT NOT NULL,                   -- Identificador de la sesión
    accion VARCHAR(50) NOT NULL,              -- Tipo de acción realizada en la sesión
    fecha DATETIME NOT NULL DEFAULT GETDATE(), -- Fecha y hora del log
    id_usuario INT NOT NULL,                  -- Usuario que realizó la acción
    datos_anteriores TEXT,                    -- Datos antes de la acción
    datos_nuevos TEXT,                        -- Datos después de la acción
    id_pelicula INT,                          -- Identificador de la película en la sesión
    nombre_pelicula VARCHAR(100),             -- Nombre de la película
    clasificacion_pelicula VARCHAR(10),       -- Clasificación de la película
    duracion_pelicula INT,                    -- Duración de la película en minutos
    fecha_hora_inicio DATETIME,               -- Fecha y hora de inicio de la sesión
    fecha_hora_fin DATETIME,                  -- Fecha y hora de fin de la sesión
    estado VARCHAR(10),                       -- Estado de la sesión (activa o inactiva)
    FOREIGN KEY (id_sesion) REFERENCES Sesion(id_sesion),
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);



--INSERTS

-- Inserrts para la tabla Usuario (solo admin y vendedor)
INSERT INTO Usuario (nombre_usuario, contraseña, rol) VALUES
('Admin', 'passwordAdmin1', 'admin'),
('Vendedor', 'passwordVendedor2', 'vendedor');


-- Inserciones para Sala (60 asientos en c/u y 60 disponibles) (La cantidad la elegimos nosotros :o)
INSERT INTO Sala (nombre_sala, cantidad_asientos) VALUES
('Sala 1', 60),
('Sala 2', 60),
('Sala 3', 60),
('Sala 4', 60),
('Sala 5', 60);

-- Inserts para Sala 1
INSERT INTO Asiento (id_sala, fila, numero) VALUES
(1, 'A', 1), (1, 'A', 2), (1, 'A', 3), (1, 'A', 4), (1, 'A', 5),
(1, 'A', 6), (1, 'A', 7), (1, 'A', 8), (1, 'A', 9), (1, 'A', 10),
(1, 'B', 1), (1, 'B', 2), (1, 'B', 3), (1, 'B', 4), (1, 'B', 5),
(1, 'B', 6), (1, 'B', 7), (1, 'B', 8), (1, 'B', 9), (1, 'B', 10),
(1, 'C', 1), (1, 'C', 2), (1, 'C', 3), (1, 'C', 4), (1, 'C', 5),
(1, 'C', 6), (1, 'C', 7), (1, 'C', 8), (1, 'C', 9), (1, 'C', 10),
(1, 'D', 1), (1, 'D', 2), (1, 'D', 3), (1, 'D', 4), (1, 'D', 5),
(1, 'D', 6), (1, 'D', 7), (1, 'D', 8), (1, 'D', 9), (1, 'D', 10),
(1, 'E', 1), (1, 'E', 2), (1, 'E', 3), (1, 'E', 4), (1, 'E', 5),
(1, 'E', 6), (1, 'E', 7), (1, 'E', 8), (1, 'E', 9), (1, 'E', 10),
(1, 'F', 1), (1, 'F', 2), (1, 'F', 3), (1, 'F', 4), (1, 'F', 5),
(1, 'F', 6), (1, 'F', 7), (1, 'F', 8), (1, 'F', 9), (1, 'F', 10);

-- Inserts para Sala 2
INSERT INTO Asiento (id_sala, fila, numero) VALUES
(2, 'A', 1), (2, 'A', 2), (2, 'A', 3), (2, 'A', 4), (2, 'A', 5),
(2, 'A', 6), (2, 'A', 7), (2, 'A', 8), (2, 'A', 9), (2, 'A', 10),
(2, 'B', 1), (2, 'B', 2), (2, 'B', 3), (2, 'B', 4), (2, 'B', 5),
(2, 'B', 6), (2, 'B', 7), (2, 'B', 8), (2, 'B', 9), (2, 'B', 10),
(2, 'C', 1), (2, 'C', 2), (2, 'C', 3), (2, 'C', 4), (2, 'C', 5),
(2, 'C', 6), (2, 'C', 7), (2, 'C', 8), (2, 'C', 9), (2, 'C', 10),
(2, 'D', 1), (2, 'D', 2), (2, 'D', 3), (2, 'D', 4), (2, 'D', 5),
(2, 'D', 6), (2, 'D', 7), (2, 'D', 8), (2, 'D', 9), (2, 'D', 10),
(2, 'E', 1), (2, 'E', 2), (2, 'E', 3), (2, 'E', 4), (2, 'E', 5),
(2, 'E', 6), (2, 'E', 7), (2, 'E', 8), (2, 'E', 9), (2, 'E', 10),
(2, 'F', 1), (2, 'F', 2), (2, 'F', 3), (2, 'F', 4), (2, 'F', 5),
(2, 'F', 6), (2, 'F', 7), (2, 'F', 8), (2, 'F', 9), (2, 'F', 10);

-- Inserts para Sala 3
INSERT INTO Asiento (id_sala, fila, numero) VALUES
(3, 'A', 1), (3, 'A', 2), (3, 'A', 3), (3, 'A', 4), (3, 'A', 5),
(3, 'A', 6), (3, 'A', 7), (3, 'A', 8), (3, 'A', 9), (3, 'A', 10),
(3, 'B', 1), (3, 'B', 2), (3, 'B', 3), (3, 'B', 4), (3, 'B', 5),
(3, 'B', 6), (3, 'B', 7), (3, 'B', 8), (3, 'B', 9), (3, 'B', 10),
(3, 'C', 1), (3, 'C', 2), (3, 'C', 3), (3, 'C', 4), (3, 'C', 5),
(3, 'C', 6), (3, 'C', 7), (3, 'C', 8), (3, 'C', 9), (3, 'C', 10),
(3, 'D', 1), (3, 'D', 2), (3, 'D', 3), (3, 'D', 4), (3, 'D', 5),
(3, 'D', 6), (3, 'D', 7), (3, 'D', 8), (3, 'D', 9), (3, 'D', 10),
(3, 'E', 1), (3, 'E', 2), (3, 'E', 3), (3, 'E', 4), (3, 'E', 5),
(3, 'E', 6), (3, 'E', 7), (3, 'E', 8), (3, 'E', 9), (3, 'E', 10),
(3, 'F', 1), (3, 'F', 2), (3, 'F', 3), (3, 'F', 4), (3, 'F', 5),
(3, 'F', 6), (3, 'F', 7), (3, 'F', 8), (3, 'F', 9), (3, 'F', 10);

-- Inserts para Sala 4
INSERT INTO Asiento (id_sala, fila, numero) VALUES
(4, 'A', 1), (4, 'A', 2), (4, 'A', 3), (4, 'A', 4), (4, 'A', 5),
(4, 'A', 6), (4, 'A', 7), (4, 'A', 8), (4, 'A', 9), (4, 'A', 10),
(4, 'B', 1), (4, 'B', 2), (4, 'B', 3), (4, 'B', 4), (4, 'B', 5),
(4, 'B', 6), (4, 'B', 7), (4, 'B', 8), (4, 'B', 9), (4, 'B', 10),
(4, 'C', 1), (4, 'C', 2), (4, 'C', 3), (4, 'C', 4), (4, 'C', 5),
(4, 'C', 6), (4, 'C', 7), (4, 'C', 8), (4, 'C', 9), (4, 'C', 10),
(4, 'D', 1), (4, 'D', 2), (4, 'D', 3), (4, 'D', 4), (4, 'D', 5),
(4, 'D', 6), (4, 'D', 7), (4, 'D', 8), (4, 'D', 9), (4, 'D', 10),
(4, 'E', 1), (4, 'E', 2), (4, 'E', 3), (4, 'E', 4), (4, 'E', 5),
(4, 'E', 6), (4, 'E', 7), (4, 'E', 8), (4, 'E', 9), (4, 'E', 10),
(4, 'F', 1), (4, 'F', 2), (4, 'F', 3), (4, 'F', 4), (4, 'F', 5),
(4, 'F', 6), (4, 'F', 7), (4, 'F', 8), (4, 'F', 9), (4, 'F', 10);

-- Inserts para Sala 5
INSERT INTO Asiento (id_sala, fila, numero) VALUES
(5, 'A', 1), (5, 'A', 2), (5, 'A', 3), (5, 'A', 4), (5, 'A', 5),
(5, 'A', 6), (5, 'A', 7), (5, 'A', 8), (5, 'A', 9), (5, 'A', 10),
(5, 'B', 1), (5, 'B', 2), (5, 'B', 3), (5, 'B', 4), (5, 'B', 5),
(5, 'B', 6), (5, 'B', 7), (5, 'B', 8), (5, 'B', 9), (5, 'B', 10),
(5, 'C', 1), (5, 'C', 2), (5, 'C', 3), (5, 'C', 4), (5, 'C', 5),
(5, 'C', 6), (5, 'C', 7), (5, 'C', 8), (5, 'C', 9), (5, 'C', 10),
(5, 'D', 1), (5, 'D', 2), (5, 'D', 3), (5, 'D', 4), (5, 'D', 5),
(5, 'D', 6), (5, 'D', 7), (5, 'D', 8), (5, 'D', 9), (5, 'D', 10),
(5, 'E', 1), (5, 'E', 2), (5, 'E', 3), (5, 'E', 4), (5, 'E', 5),
(5, 'E', 6), (5, 'E', 7), (5, 'E', 8), (5, 'E', 9), (5, 'E', 10),
(5, 'F', 1), (5, 'F', 2), (5, 'F', 3), (5, 'F', 4), (5, 'F', 5),
(5, 'F', 6), (5, 'F', 7), (5, 'F', 8), (5, 'F', 9), (5, 'F', 10);



-- Inserts para tabla Pelicula
INSERT INTO Pelicula (nombre, clasificacion, duracion, descripcion) VALUES
('Avatar', 'PG-13', 162, 'Una ciencia ficción épica sobre el reino de Pandora y la guerra entre sus habitantes y los humanos.'),
('Inception', 'PG-13', 148, 'Un ladrón que roba secretos a través de los sueños busca su redención.'),
('The Dark Knight', 'PG-13', 152, 'Batman lucha contra el Joker en la secuela de la famosa saga.'),
('Interstellar', 'PG-13', 169, 'Un viaje a través del espacio y tiempo dirigido por Christopher Nolan.'),
('The Godfather', 'R', 175, 'La historia de la familia mafiosa Corleone.');

-- Inserts para Sesion (cantidad solicitada para proyecto en fase 1)
INSERT INTO Sesion (id_pelicula, id_sala, fecha_hora_inicio, fecha_hora_fin, estado) VALUES
(1, 1, '2024-10-01 15:00:00', '2024-10-01 17:42:00', 'activa'), -- Película 1 en Sala 1
(2, 2, '2024-10-02 18:00:00', '2024-10-02 20:28:00', 'activa'), -- Película 2 en Sala 2
(3, 3, '2024-10-03 20:00:00', '2024-10-03 22:02:00', 'activa'), -- Película 3 en Sala 3
(4, 4, '2024-10-04 16:30:00', '2024-10-04 18:39:00', 'activa'), -- Película 4 en Sala 4
(5, 5, '2024-10-05 14:00:00', '2024-10-05 16:55:00', 'activa'), -- Película 5 en Sala 5
(1, 2, '2024-10-06 17:00:00', '2024-10-06 19:42:00', 'activa'), -- Película 1 en Sala 2
(2, 3, '2024-10-07 19:30:00', '2024-10-07 22:28:00', 'activa'), -- Película 2 en Sala 3
(3, 4, '2024-10-08 21:00:00', '2024-10-08 23:02:00', 'activa'), -- Película 3 en Sala 4
(4, 5, '2024-10-09 13:30:00', '2024-10-09 15:39:00', 'activa'), -- Película 4 en Sala 5
(5, 1, '2024-10-10 15:00:00', '2024-10-10 17:55:00', 'activa'); -- Película 5 en Sala 1

select * from Sesion

-- Inserciones para la tabla Transaccion (sin numero_transaccion)
INSERT INTO Transaccion (id_usuario, id_sesion, fecha_hora, total_asientos, tipo_asignacion) VALUES
-- Transacciones para Sesión 1
(1, 1, '2024-10-01 14:30:00', 2, 'manual'),
(2, 1, '2024-10-01 14:45:00', 3, 'manual'),
(1, 1, '2024-10-01 14:55:00', 1, 'manual'),
(2, 1, '2024-10-01 15:10:00', 4, 'automatica'),
(1, 1, '2024-10-01 15:20:00', 2, 'automatica'),

-- Transacciones para Sesión 2
(2, 2, '2024-10-02 17:30:00', 2, 'manual'),
(1, 2, '2024-10-02 17:45:00', 3, 'manual'),
(2, 2, '2024-10-02 18:15:00', 1, 'automatica'),
(1, 2, '2024-10-02 18:25:00', 4, 'automatica'),
(2, 2, '2024-10-02 18:30:00', 2, 'manual'),

-- Transacciones para Sesión 3
(1, 3, '2024-10-03 19:30:00', 2, 'manual'),
(2, 3, '2024-10-03 19:45:00', 3, 'manual'),
(1, 3, '2024-10-03 20:10:00', 1, 'manual'),
(2, 3, '2024-10-03 20:25:00', 4, 'automatica'),
(1, 3, '2024-10-03 20:35:00', 2, 'manual'),

-- Transacciones para Sesión 4
(2, 4, '2024-10-04 16:00:00', 2, 'manual'),
(1, 4, '2024-10-04 16:15:00', 3, 'manual'),
(2, 4, '2024-10-04 16:30:00', 1, 'automatica'),
(1, 4, '2024-10-04 16:45:00', 4, 'manual'),
(2, 4, '2024-10-04 16:55:00', 2, 'manual'),

-- Transacciones para Sesión 5
(1, 5, '2024-10-05 13:30:00', 2, 'manual'),
(2, 5, '2024-10-05 13:45:00', 3, 'manual'),
(1, 5, '2024-10-05 14:00:00', 1, 'automatica'),
(2, 5, '2024-10-05 14:10:00', 4, 'manual'),
(1, 5, '2024-10-05 14:20:00', 2, 'automatica'),

-- Transacciones para Sesión 6
(2, 6, '2024-10-06 16:30:00', 2, 'manual'),
(1, 6, '2024-10-06 16:45:00', 3, 'manual'),
(2, 6, '2024-10-06 17:00:00', 1, 'manual'),
(1, 6, '2024-10-06 17:15:00', 4, 'manual'),
(2, 6, '2024-10-06 17:30:00', 2, 'manual'),

-- Transacciones para Sesión 7
(1, 7, '2024-10-07 19:00:00', 2, 'manual'),
(2, 7, '2024-10-07 19:15:00', 3, 'manual'),
(1, 7, '2024-10-07 19:30:00', 1, 'manual'),
(2, 7, '2024-10-07 19:45:00', 4, 'automatica'),
(1, 7, '2024-10-07 20:00:00', 2, 'automatica'),

-- Transacciones para Sesión 8
(2, 8, '2024-10-08 20:30:00', 2, 'manual'),
(1, 8, '2024-10-08 20:45:00', 3, 'manual'),
(2, 8, '2024-10-08 21:00:00', 1, 'manual'),
(1, 8, '2024-10-08 21:15:00', 4, 'automatica'),
(2, 8, '2024-10-08 21:30:00', 2, 'manual'),

-- Transacciones para Sesión 9
(1, 9, '2024-10-09 13:00:00', 2, 'manual'),
(2, 9, '2024-10-09 13:15:00', 3, 'manual'),
(1, 9, '2024-10-09 13:30:00', 1, 'manual'),
(2, 9, '2024-10-09 13:45:00', 4, 'automatica'),
(1, 9, '2024-10-09 14:00:00', 2, 'manual'),

-- Transacciones para Sesión 10
(2, 10, '2024-10-10 14:30:00', 2, 'manual'),
(1, 10, '2024-10-10 14:45:00', 3, 'manual'),
(2, 10, '2024-10-10 15:00:00', 1, 'automatica'),
(1, 10, '2024-10-10 15:15:00', 4, 'manual'),
(2, 10, '2024-10-10 15:30:00', 2, 'automatica');


-- iNSERTS para la tabla AsientoTransaccion

--Los inserts en este caso son para ejemplo del funcionamiento de la tabla, están asignados según las 5 salas
-- Transacciones para Sesión 1 (Sala 1)
INSERT INTO AsientoTransaccion (fila, numero, id_transaccion, id_sesion, id_sala) VALUES
('A', 1, 1, 1, 1), ('A', 2, 1, 1, 1),
('A', 3, 2, 1, 1), ('A', 4, 2, 1, 1), ('A', 5, 2, 1, 1),
('A', 6, 3, 1, 1),
('A', 7, 4, 1, 1), ('A', 8, 4, 1, 1), ('A', 9, 4, 1, 1), ('A', 10, 4, 1, 1),
('B', 1, 5, 1, 1), ('B', 2, 5, 1, 1);

-- Transacciones para Sesión 2 (Sala 2)
INSERT INTO AsientoTransaccion (fila, numero, id_transaccion, id_sesion, id_sala) VALUES
('A', 1, 6, 2, 2), ('A', 2, 6, 2, 2),
('A', 3, 7, 2, 2), ('A', 4, 7, 2, 2), ('A', 5, 7, 2, 2),
('A', 6, 8, 2, 2),
('A', 7, 9, 2, 2), ('A', 8, 9, 2, 2), ('A', 9, 9, 2, 2), ('A', 10, 9, 2, 2),
('B', 1, 10, 2, 2), ('B', 2, 10, 2, 2);

-- Transacciones para Sesión 3 (Sala 3)
INSERT INTO AsientoTransaccion (fila, numero, id_transaccion, id_sesion, id_sala) VALUES
('A', 1, 11, 3, 3), ('A', 2, 11, 3, 3),
('A', 3, 12, 3, 3), ('A', 4, 12, 3, 3), ('A', 5, 12, 3, 3),
('A', 6, 13, 3, 3),
('A', 7, 14, 3, 3), ('A', 8, 14, 3, 3), ('A', 9, 14, 3, 3), ('A', 10, 14, 3, 3),
('B', 1, 15, 3, 3), ('B', 2, 15, 3, 3);

-- Transacciones para Sesión 4 (Sala 4)
INSERT INTO AsientoTransaccion (fila, numero, id_transaccion, id_sesion, id_sala) VALUES
('A', 1, 16, 4, 4), ('A', 2, 16, 4, 4),
('A', 3, 17, 4, 4), ('A', 4, 17, 4, 4), ('A', 5, 17, 4, 4),
('A', 6, 18, 4, 4),
('A', 7, 19, 4, 4), ('A', 8, 19, 4, 4), ('A', 9, 19, 4, 4), ('A', 10, 19, 4, 4),
('B', 1, 20, 4, 4), ('B', 2, 20, 4, 4);

-- Transacciones para Sesión 5 (Sala 5)
INSERT INTO AsientoTransaccion (fila, numero, id_transaccion, id_sesion, id_sala) VALUES
('A', 1, 21, 5, 5), ('A', 2, 21, 5, 5),
('A', 3, 22, 5, 5), ('A', 4, 22, 5, 5), ('A', 5, 22, 5, 5),
('A', 6, 23, 5, 5),
('A', 7, 24, 5, 5), ('A', 8, 24, 5, 5), ('A', 9, 24, 5, 5), ('A', 10, 24, 5, 5),
('B', 1, 25, 5, 5), ('B', 2, 25, 5, 5);

-- Transacciones para Sesión 6 (Sala 2)
INSERT INTO AsientoTransaccion (fila, numero, id_transaccion, id_sesion, id_sala) VALUES
('A', 1, 26, 6, 2), ('A', 2, 26, 6, 2),
('A', 3, 27, 6, 2), ('A', 4, 27, 6, 2), ('A', 5, 27, 6, 2),
('A', 6, 28, 6, 2),
('A', 7, 29, 6, 2), ('A', 8, 29, 6, 2), ('A', 9, 29, 6, 2), ('A', 10, 29, 6, 2),
('B', 1, 30, 6, 2), ('B', 2, 30, 6, 2);

-- Transacciones para Sesión 7 (Sala 3)
INSERT INTO AsientoTransaccion (fila, numero, id_transaccion, id_sesion, id_sala) VALUES
('A', 1, 31, 7, 3), ('A', 2, 31, 7, 3),
('A', 3, 32, 7, 3), ('A', 4, 32, 7, 3), ('A', 5, 32, 7, 3),
('A', 6, 33, 7, 3),
('A', 7, 34, 7, 3), ('A', 8, 34, 7, 3), ('A', 9, 34, 7, 3), ('A', 10, 34, 7, 3),
('B', 1, 35, 7, 3), ('B', 2, 35, 7, 3);

-- Transacciones para Sesión 8 (Sala 4)
INSERT INTO AsientoTransaccion (fila, numero, id_transaccion, id_sesion, id_sala) VALUES
('A', 1, 36, 8, 4), ('A', 2, 36, 8, 4),
('A', 3, 37, 8, 4), ('A', 4, 37, 8, 4), ('A', 5, 37, 8, 4),
('A', 6, 38, 8, 4),
('A', 7, 39, 8, 4), ('A', 8, 39, 8, 4), ('A', 9, 39, 8, 4), ('A', 10, 39, 8, 4),
('B', 1, 40, 8, 4), ('B', 2, 40, 8, 4);

-- Transacciones para Sesión 9 (Sala 5)
INSERT INTO AsientoTransaccion (fila, numero, id_transaccion, id_sesion, id_sala) VALUES
('A', 1, 41, 9, 5), ('A', 2, 41, 9, 5),
('A', 3, 42, 9, 5), ('A', 4, 42, 9, 5), ('A', 5, 42, 9, 5),
('A', 6, 43, 9, 5),
('A', 7, 44, 9, 5), ('A', 8, 44, 9, 5), ('A', 9, 44, 9, 5), ('A', 10, 44, 9, 5),
('B', 1, 45, 9, 5), ('B', 2, 45, 9, 5);

-- Transacciones para Sesión 10 (Sala 1)
INSERT INTO AsientoTransaccion (fila, numero, id_transaccion, id_sesion, id_sala) VALUES
('A', 1, 46, 10, 1), ('A', 2, 46, 10, 1),
('A', 3, 47, 10, 1), ('A', 4, 47, 10, 1), ('A', 5, 47, 10, 1),
('A', 6, 48, 10, 1),
('A', 7, 49, 10, 1), ('A', 8, 49, 10, 1), ('A', 9, 49, 10, 1), ('A', 10, 49, 10, 1),
('B', 1, 50, 10, 1), ('B', 2, 50, 10, 1);


--Backup de la Base de Datos
BACKUP DATABASE [CineGT] TO DISK = 'C:\Windows\Temp\proyecto_basesII.bak';

--Verifica si la tabla fue creada correctamente
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Sala';
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Asiento';
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Pelicula';
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Sesion';
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Transaccion';
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AsientoTransaccion';
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LogSesion';
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'LogTransaccion';
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Usuario';

--VER TABLAS
SELECT * FROM Asiento
SELECT * FROM AsientoTransaccion
SELECT * FROM LogSesion
SELECT * FROM LogTransaccion
SELECT * FROM Pelicula
SELECT * FROM Sala
SELECT * FROM Sesion
SELECT * FROM Transaccion
SELECT * FROM Usuario


--Cambio de nombres que estaban al revés
EXEC sp_rename 'Transaccion', 'TransaccionTemp';
EXEC sp_rename 'AsientoTransaccion', 'Transaccion';
EXEC sp_rename 'TransaccionTemp', 'AsientoTransaccion';

