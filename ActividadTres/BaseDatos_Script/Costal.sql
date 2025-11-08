-- Crear la base de datos
CREATE DATABASE Cosatl12;
GO

USE Costal12;
GO

CREATE TABLE Clientes(
	ClienteID INT PRIMARY KEY IDENTITY(1,1),
    NombreCompleto VARCHAR(150) NOT NULL,
    CorreoElectronico VARCHAR(100) NULL,
    Telefono VARCHAR(15) NULL,
    Direccion VARCHAR(255) NULL
);
GO

CREATE TABLE Categorias(
	CategoriaID INT PRIMARY KEY IDENTITY(1,1),
    NombreCategoria VARCHAR(50) NOT NULL
);
GO

CREATE TABLE Proveedores (
    ProveedorID INT PRIMARY KEY IDENTITY(1,1),
    NombreProveedor VARCHAR(100) NOT NULL,
    Telefono VARCHAR(15) NULL,
    CorreoElectronico VARCHAR(100) NULL
);
GO

CREATE TABLE Productos (
    ProductoID INT PRIMARY KEY IDENTITY(1,1),
    NombreProducto VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255) NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Stock INT NOT NULL DEFAULT 0,
    CategoriaID INT NULL,
);
GO



INSERT INTO Clientes (ClienteID, nombreCompleto, CorreoElectronico, Telefono, Direccion) VALUES
('1', 'Oscar Thomas', 'oscar@Example.com', '8243568978', 'Av.45'),
('2', 'Ana María', 'ana.rodriguez@email.com', '8095551234', 'Calle Duarte'),
('3', 'Juan Carlos ', 'juan.mendez@email.com', '8095555678', 'Av. Independencia'),
('4', 'Pedro Antonio', 'pedro.castillo@email.com', '8095553456', 'Calle Las Palmas');
GO

INSERT INTO Categorias (CategoriaID, NombreCategoria) VALUES
(1, '1', 'Ropa Exterior'),
(2, '2', 'Lenceria y Pijamas'),
(3, '3', 'Ropa de CAma y Baño'),
(4, '4', 'Accesorios Textiles');
GO

INSERT INTO Proveedores ( ProveedorID,  NombreProveedor,  Telefono, CorreoElectronico) VALUES
(1, '501', 'Textil Superior S.R.L.', '809-555-2001', 'pedidos@textilsuperior.com'),
(2, '502', 'Moda Global Mayorista', '809-555-2002', 'contacto@modaglobal.com'),
(3, '503', 'Suministros Hilo Fino', '809-555-2003', 'ventas@hilofino.net'),
(4, '504', 'Fabrica de Lencería C.', '809-555-2004', 'info@lenceriafab.com'),
(4, '505', 'Home Comfort Textiles', '809-555-2005', 'soporte@homecomfort.com')
;
GO

INSERT INTO Productos ( ProductoID,  NombreProducto, Descripcion, Precio, Stock, CategoriaID) VALUES
(1, '1', 'Jeans Clásicos', 'Pantalón de mezclilla, corte recto', '45.00', '15', '1'),
(2, '2', 'Abrigo de Lana', 'Abrigo largo de invierno, gris', '120.00', '4', '1'),
(3, '3', 'Camiseta Algodón', 'Manga corta, 100% algodón', '15.00', '50', '1', '1'),
(4, '4', 'Conjunto Pijama Seda', 'Pijama de dos piezas en seda', '35.00', '12', '2'),
(5, '5', 'Sujetador Deportivo', 'Alta sujeción, material transpirable', '28.00', '20', '2'),
(6, '6', 'Juego Sábanas Queen', '4 piezas, 300 hilos, color blanco', '55.00', '7', '3'),
(7, '7', 'Toalla de Baño XL', 'Algodón egipcio, alta absorción', '18.00', '30', '3'),
(8, '8', 'Funda de Almohada', 'Microfibra anti-alérgica', '8.00', '40', '3'),
(9, '9', 'Bufanda de Cashmere', 'Suave y larga, ideal para invierno', '40.00', '10', '4'),
(10, '10', 'Set de Pañuelos', '3 pañuelos de lino para bolsillo', '12.00', '25', '4')
;
GO



GO

SELECT * FROM Clientes;
SELECT * FROM Categorias;
SELECT * FROM Proveedores;
SELECT * FROM Productos;

