CREATE DATABASE RentaYa;
GO

USE RentaYa;
GO

CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    NombreUsuario VARCHAR(50) NOT NULL UNIQUE,
    Contrasena VARCHAR(50) NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL
);
GO

CREATE TABLE Categorias (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(50) NOT NULL
);
GO

CREATE TABLE Equipos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(500) NOT NULL,
    Imagen VARCHAR(200) NOT NULL,
    PrecioPorDia DECIMAL(10,2) NOT NULL,
    IdCategoria INT NOT NULL FOREIGN KEY REFERENCES Categorias(Id),
    Activo BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE Alquileres (
    Id INT PRIMARY KEY IDENTITY(1,1),
    IdUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuarios(Id),
    IdEquipo INT NOT NULL FOREIGN KEY REFERENCES Equipos(Id),
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    Total DECIMAL(10,2) NOT NULL,
    Cancelado BIT NOT NULL DEFAULT 0
);
GO

INSERT INTO Usuarios (NombreUsuario, Contrasena, Nombre, Apellido) VALUES
('matias', '1234', 'Matias', 'Moguelevsky');
GO

INSERT INTO Categorias (Nombre) VALUES ('Camaras'), ('Consolas'), ('Herramientas');
GO

INSERT INTO Equipos (Nombre, Descripcion, Imagen, PrecioPorDia, IdCategoria, Activo) VALUES
('Camara Canon R50', 'Camara digital 4K', 'camara1.jpg', 15000.00, 1, 1),
('Camara Sony A6400', 'Camara compacta', 'camara2.jpg', 18000.00, 1, 1),
('Camara Nikon Z30', 'Camara para video', 'camara3.jpg', 14000.00, 1, 1),
('GoPro Hero 12', 'Camara sumergible', 'camara4.jpg', 9500.00, 1, 1),
('DJI Osmo Pocket', 'Camara de bolsillo', 'camara5.jpg', 12000.00, 1, 1),
('Fujifilm X-S20', 'Camara profesional', 'camara6.jpg', 19500.00, 1, 1),
('Lumix G100', 'Camara liviana', 'camara7.jpg', 11000.00, 1, 1),
('PlayStation 5', 'Consola Sony con joystick', 'consola1.jpg', 13000.00, 2, 1),
('Xbox Series X', 'Consola Microsoft 1TB', 'consola2.jpg', 12500.00, 2, 1),
('Nintendo Switch', 'Consola portatil', 'consola3.jpg', 9000.00, 2, 1),
('PlayStation 4', 'Consola Sony 1TB', 'consola4.jpg', 8000.00, 2, 1),
('Xbox Series S', 'Consola digital', 'consola5.jpg', 8500.00, 2, 1),
('Steam Deck', 'Consola portatil PC', 'consola6.jpg', 14500.00, 2, 1),
('Meta Quest 3', 'Visor realidad virtual', 'consola7.jpg', 16000.00, 2, 1),
('Taladro Bosch', 'Taladro percutor 750W', 'herramienta1.jpg', 5500.00, 3, 1),
('Amoladora Makita', 'Amoladora 115mm', 'herramienta2.jpg', 5000.00, 3, 1),
('Sierra DeWalt', 'Sierra circular', 'herramienta3.jpg', 7500.00, 3, 1),
('Atornillador B+D', 'Atornillador a bateria', 'herramienta4.jpg', 4500.00, 3, 1),
('Hidrolavadora K3', 'Hidrolavadora alta presion', 'herramienta5.jpg', 8500.00, 3, 1),
('Lijadora Stanley', 'Lijadora orbital', 'herramienta6.jpg', 4000.00, 3, 1);
GO

INSERT INTO Alquileres (IdUsuario, IdEquipo, FechaInicio, FechaFin, Total, Cancelado) VALUES
(1, 1, '2026-10-01', '2026-10-03', 30000.00, 0),
(1, 2, '2026-10-05', '2026-10-07', 36000.00, 1),
(1, 3, '2026-10-10', '2026-10-12', 28000.00, 0),
(1, 4, '2026-10-15', '2026-10-16', 9500.00, 0),
(1, 5, '2026-11-01', '2026-11-03', 24000.00, 0),
(1, 6, '2026-11-05', '2026-11-07', 39000.00, 0),
(1, 7, '2026-11-10', '2026-11-12', 22000.00, 0),
(1, 8, '2026-12-01', '2026-12-03', 26000.00, 0),
(1, 9, '2026-12-05', '2026-12-07', 25000.00, 0),
(1, 10, '2026-12-10', '2026-12-12', 18000.00, 0);
GO

CREATE PROCEDURE sp_ObtenerAlquileresPorUsuario
    @pIdUsuario INT
AS
BEGIN
    SELECT A.Id, A.IdUsuario, A.IdEquipo, A.FechaInicio, A.FechaFin, A.Total, A.Cancelado,
           E.Nombre AS NombreEquipo, E.Imagen AS ImagenEquipo
    FROM Alquileres A
    INNER JOIN Equipos E ON A.IdEquipo = E.Id
    WHERE A.IdUsuario = @pIdUsuario
    ORDER BY A.FechaInicio DESC;
END;
GO

CREATE PROCEDURE sp_CancelarAlquiler
    @pIdAlquiler INT,
    @pIdUsuario INT
AS
BEGIN
    UPDATE Alquileres
    SET Cancelado = 1
    WHERE Id = @pIdAlquiler AND IdUsuario = @pIdUsuario;
END;
GO
