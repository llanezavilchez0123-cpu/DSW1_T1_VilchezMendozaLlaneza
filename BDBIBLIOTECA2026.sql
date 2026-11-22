CREATE DATABASE BDBIBLIOTECA2026;
GO

USE BDBIBLIOTECA2026;
GO

CREATE TABLE dbo.Libros
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreLibro VARCHAR(120) NOT NULL,
    Autor VARCHAR(100) NOT NULL,
    NombreEditorial VARCHAR(100) NOT NULL,
    ISDN VARCHAR(20) NOT NULL
);
GO


USE BDBIBLIOTECA2026;
GO

SELECT * FROM dbo.Libros;



USE BDBIBLIOTECA2026;
GO

-- 1. LISTAR TODOS LOS LIBROS
CREATE OR ALTER PROCEDURE usp_ListarLibros
AS
BEGIN
    SELECT Id, NombreLibro, Autor,
           NombreEditorial, ISDN
    FROM dbo.Libros
    ORDER BY Id DESC;
END;
GO

-- 2. REGISTRAR UN LIBRO
CREATE OR ALTER PROCEDURE usp_RegistrarLibro
    @NombreLibro VARCHAR(120),
    @Autor VARCHAR(100),
    @NombreEditorial VARCHAR(100),
    @ISDN VARCHAR(20)
AS
BEGIN
    INSERT INTO dbo.Libros
        (NombreLibro, Autor, NombreEditorial, ISDN)
    VALUES
        (@NombreLibro, @Autor, @NombreEditorial, @ISDN);
END;
GO

-- 3. ACTUALIZAR UN LIBRO
CREATE OR ALTER PROCEDURE usp_ActualizarLibro
    @Id INT,
    @NombreLibro VARCHAR(120),
    @Autor VARCHAR(100),
    @NombreEditorial VARCHAR(100),
    @ISDN VARCHAR(20)
AS
BEGIN
    UPDATE dbo.Libros
    SET NombreLibro = @NombreLibro,
        Autor = @Autor,
        NombreEditorial = @NombreEditorial,
        ISDN = @ISDN
    WHERE Id = @Id;
END;
GO

-- 4. ELIMINAR UN LIBRO
CREATE OR ALTER PROCEDURE usp_EliminarLibro
    @Id INT
AS
BEGIN
    DELETE FROM dbo.Libros
    WHERE Id = @Id;
END;
GO

-- 5. BUSCAR LIBROS POR AUTOR
CREATE OR ALTER PROCEDURE usp_ListarLibrosPorAutor
    @Autor VARCHAR(100)
AS
BEGIN
    SELECT Id, NombreLibro, Autor,
           NombreEditorial, ISDN
    FROM dbo.Libros
    WHERE Autor LIKE '%' + @Autor + '%'
    ORDER BY NombreLibro;
END;
GO


USE BDBIBLIOTECA2026;
GO

EXEC usp_RegistrarLibro
    @NombreLibro = 'Cien años de soledad',
    @Autor = 'Gabriel Garcia Marquez',
    @NombreEditorial = 'Sudamericana',
    @ISDN = '9780307474728';

EXEC usp_ListarLibros;

EXEC usp_ListarLibrosPorAutor
    @Autor = 'Garcia';

    
SELECT @@SERVERNAME AS NombreServidor;

