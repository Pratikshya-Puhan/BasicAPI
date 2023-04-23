CREATE DATABASE BasicDB;
GO
USE BasicDB;
GO
CREATE TABLE tblCountry
(
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Name VARCHAR(255) NOT NULL UNIQUE,
	ShortCode VARCHAR(10) NOT NULL UNIQUE,
	ISDCode VARCHAR(20) NOT NULL,
	FlagUrl VARCHAR(1000) NOT NULL
);
GO
CREATE TABLE tblState
(
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Name VARCHAR(255) NOT NULL,
	CountryId INT NOT NULL REFERENCES tblCountry(Id)
);
GO
CREATE OR ALTER PROCEDURE SpGetCountry
(
	@Id INT
)
AS
BEGIN
	-- GET COUNRTY
	SELECT * FROM tblCountry WHERE Id = @Id
	-- GET STATES
	SELECT * FROM tblState WHERE CountryId = @Id
END
GO
CREATE OR ALTER PROCEDURE SpSaveCountry
(
	@Name VARCHAR(255),
	@ShortCode VARCHAR(10),
	@ISDCode VARCHAR(20),
	@FlagUrl VARCHAR(1000) 
)
AS
BEGIN
	INSERT INTO tblCountry
	SELECT @Name Name, @ShortCode ShortCode, @ISDCode ISDCode, @FlagUrl FlagUrl

	SELECT * FROM tblCountry WHERE Id = SCOPE_IDENTITY()
END