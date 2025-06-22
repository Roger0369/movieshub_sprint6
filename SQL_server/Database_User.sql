CREATE DATABASE LoginApp;
GO

-- Usar base de datos
USE LoginApp;
GO

-- Eliminar la tabla si ya existe
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL
    DROP TABLE dbo.Users;
GO

-- Crear la tabla Users
CREATE TABLE dbo.Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(128) NOT NULL, -- HEX string (SHA2_512 = 128 chars)
);
GO

-- Insertar un usuario de prueba con contraseña hasheada
DECLARE @pass VARCHAR(50) = 'pass1';

INSERT INTO dbo.Users (Username, PasswordHash)
VALUES (
    'admin',
    CONVERT(NVARCHAR(128), HASHBYTES('SHA2_512', @pass), 2) -- estilo hexadecimal, mayúsculas
);
GO

-- Verificar la tabla
SELECT * FROM dbo.Users;
SELECT * FROM Movies;


