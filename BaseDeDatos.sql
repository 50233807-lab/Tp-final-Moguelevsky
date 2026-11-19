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
    IdEquipo INT NOT NULL FOREIGN KEY REFERENCES Equipos(Id),
    IdUsuario INT NOT NULL FOREIGN KEY REFERENCES Usuarios(Id),
    FechaRetiro DATE NOT NULL,
    FechaDevolucion DATE NOT NULL,
    Total DECIMAL(10,2) NOT NULL,
    Cancelado BIT NOT NULL DEFAULT 0,
    FechaCancelacion DATETIME NULL
);
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

CREATE PROCEDURE sp_Login
    @NombreUsuario VARCHAR(50),
    @Contrasena VARCHAR(50)
AS
BEGIN
    SELECT * FROM Usuarios WHERE NombreUsuario = @NombreUsuario AND Contrasena = @Contrasena;
END
GO

CREATE PROCEDURE sp_BuscarUsuarioPorNombre
    @NombreUsuario VARCHAR(50)
AS
BEGIN
    SELECT * FROM Usuarios WHERE NombreUsuario = @NombreUsuario;
END
GO

CREATE PROCEDURE sp_RegistrarUsuario
    @NombreUsuario VARCHAR(50),
    @Contrasena VARCHAR(50),
    @Nombre VARCHAR(50),
    @Apellido VARCHAR(50)
AS
BEGIN
    INSERT INTO Usuarios (NombreUsuario, Contrasena, Nombre, Apellido)
    VALUES (@NombreUsuario, @Contrasena, @Nombre, @Apellido);
END
GO

CREATE PROCEDURE sp_ObtenerEquiposPaginados
    @Saltar INT,
    @Traer INT
AS
BEGIN
    SELECT E.Id, E.Nombre, E.Descripcion, E.Imagen, E.PrecioPorDia, E.IdCategoria, E.Activo, C.Nombre AS NombreCategoria
    FROM Equipos E
    INNER JOIN Categorias C ON E.IdCategoria = C.Id
    WHERE E.Activo = 1
    ORDER BY E.Id
    OFFSET @Saltar ROWS FETCH NEXT @Traer ROWS ONLY;
END
GO

CREATE PROCEDURE sp_ContarEquiposActivos
AS
BEGIN
    SELECT COUNT(*) FROM Equipos WHERE Activo = 1;
END
GO

CREATE PROCEDURE sp_ObtenerEquipoPorId
    @Id INT
AS
BEGIN
    SELECT E.Id, E.Nombre, E.Descripcion, E.Imagen, E.PrecioPorDia, E.IdCategoria, E.Activo, C.Nombre AS NombreCategoria
    FROM Equipos E
    INNER JOIN Categorias C ON E.IdCategoria = C.Id
    WHERE E.Id = @Id AND E.Activo = 1;
END
GO

CREATE PROCEDURE sp_BuscarAlquilerSuperpuesto
    @IdEquipo INT,
    @FechaRetiro DATE,
    @FechaDevolucion DATE
AS
BEGIN
    SELECT TOP 1 *
    FROM Alquileres
    WHERE IdEquipo = @IdEquipo
      AND Cancelado = 0
      AND @FechaRetiro <= FechaDevolucion
      AND @FechaDevolucion >= FechaRetiro;
END
GO

CREATE PROCEDURE sp_ContarCancelacionesMesActual
    @IdUsuario INT,
    @Mes INT,
    @Anio INT
AS
BEGIN
    SELECT COUNT(*)
    FROM Alquileres
    WHERE IdUsuario = @IdUsuario
      AND Cancelado = 1
      AND MONTH(FechaCancelacion) = @Mes
      AND YEAR(FechaCancelacion) = @Anio;
END
GO

CREATE PROCEDURE sp_InsertarAlquiler
    @IdEquipo INT,
    @IdUsuario INT,
    @FechaRetiro DATE,
    @FechaDevolucion DATE,
    @Total DECIMAL(10,2)
AS
BEGIN
    INSERT INTO Alquileres (IdEquipo, IdUsuario, FechaRetiro, FechaDevolucion, Total, Cancelado, FechaCancelacion)
    VALUES (@IdEquipo, @IdUsuario, @FechaRetiro, @FechaDevolucion, @Total, 0, NULL);
END
GO
