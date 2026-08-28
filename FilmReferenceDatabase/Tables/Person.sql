CREATE TABLE Person (
	PersonId INT NOT NULL IDENTITY (1, 1),
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NULL,
	Description NVARCHAR(500) NULL,
	IsCastMember BIT NOT NULL,
	IsDirector BIT NOT NULL,
	NationalityId INT NULL,
	DateOfBirth DATETIME2 NULL,
	DateOfDeath DATETIME2 NULL,
	Picture VARBINARY(MAX) NULL,
	CONSTRAINT PK_Person PRIMARY KEY (PersonId),
	CONSTRAINT FK_Person_Nationality FOREIGN KEY (NationalityId)
		REFERENCES Nationality(NationalityId)
)