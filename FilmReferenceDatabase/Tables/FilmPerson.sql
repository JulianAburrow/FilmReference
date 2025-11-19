CREATE TABLE FilmPerson (
	FilmPersonId INT NOT NULL IDENTITY (1, 1),
	FilmId INT NOT NULL,
	PersonId INT NOT NULL,
	CONSTRAINT PK_FilmPerson PRIMARY KEY (FilmPersonId),
	CONSTRAINT FK_FilmPerson_Film FOREIGN KEY (FilmId)
		REFERENCES Film (FilmId),
	CONSTRAINT FK_FilmPerson_Person FOREIGN KEY (PersonId)
		REFERENCES Person (PersonId)
)