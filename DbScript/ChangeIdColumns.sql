-- Use JA version of application

USE FilmReference
GO

sp_rename 'Film.Id', 'FilmId', 'COLUMN'
GO
sp_rename 'FilmPerson.Id', 'FilmPersonId', 'COLUMN'
GO
sp_rename 'Genre.Id', 'GenreId', 'COLUMN'
GO
sp_rename 'Person.Id', 'PersonId', 'COLUMN'
GO
sp_rename 'Studio.Id', 'StudioId', 'COLUMN'
GO

-- Use TS version of application

USE FilmReference
GO

sp_rename 'Film.FilmId', 'Id', 'COLUMN'
GO
sp_rename 'FilmPerson.FilmPersonId', 'Id', 'COLUMN'
GO
sp_rename 'Genre.GenreId', 'Id', 'COLUMN'
GO
sp_rename 'Person.PersonId', 'Id', 'COLUMN'
GO
sp_rename 'Studio.StudioId', 'Id', 'COLUMN'
GO