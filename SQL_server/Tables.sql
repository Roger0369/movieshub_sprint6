USE LoginApp;
GO

-- Crea la tabla Movies (películas)
CREATE TABLE Movies (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Director NVARCHAR(100),
    Cast NVARCHAR(300),
    ReleaseDate DATE,
    Image NVARCHAR(255)
);
GO

-- Crea la tabla MovieSuggestions
-- Sugerencias de películas relacionadas
CREATE TABLE MovieSuggestions (
    Id INT PRIMARY KEY IDENTITY(1,1),
    MovieId INT NOT NULL,
    SuggestedSlug NVARCHAR(100) NOT NULL,
    FOREIGN KEY (MovieId) REFERENCES Movies(Id)
);
GO

-- Crea la tabla UserFavorites
-- Registro de películas favoritas por usuario
CREATE TABLE UserFavorites (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    MovieId INT NOT NULL,
    CreatedAt DATETIME2 DEFAULT SYSDATETIME(),
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (MovieId) REFERENCES Movies(Id),
    CONSTRAINT UQ_User_Movie_Favorite UNIQUE (UserId, MovieId)
);
GO

-- Actualiza valores nulos en IsFavorite y IsHidden a 0 (false)
-- Antes de convertirlos a NOT NULL
UPDATE Movies SET IsFavorite = 0 WHERE IsFavorite IS NULL;
UPDATE Movies SET IsHidden = 0 WHERE IsHidden IS NULL;

-- Modifica las columnas para que no acepten valores nulos
ALTER TABLE Movies ALTER COLUMN IsFavorite BIT NOT NULL;
ALTER TABLE Movies ALTER COLUMN IsHidden BIT NOT NULL;

-- Consulta final para verificar las películas
SELECT * FROM Movies