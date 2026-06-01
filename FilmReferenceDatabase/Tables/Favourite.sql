CREATE TABLE Favourite (
	FavouriteId INT NOT NULL IDENTITY(1, 1),
	EntityTypeId INT NOT NULL,
	EntityId INT NOT NULL,
	CONSTRAINT PK_Favourite PRIMARY KEY (FavouriteId)
);