namespace FilmReferenceTests;

public class FavouriteTests
{
    private FilmReferenceContext GetContext()
        => DbContextHelper.GetInMemoryContext();

    [Fact]
    public async Task CreateFavouriteAsync_ShouldAddFavourite_WhenNotDuplicate()
    {
        using var context = GetContext();
        var handler = new FavouriteHandler(context);

        var fav = new FavouriteModel
        {
            EntityTypeId = (int)FavouriteEntityEnum.Film,
            EntityId = 10
        };

        await handler.CreateFavouriteAsync(fav, saveChanges: true);

        var result = await context.Favourites.FirstOrDefaultAsync();
        result.Should().NotBeNull();
        result.EntityTypeId.Should().Be((int)FavouriteEntityEnum.Film);
        result.EntityId.Should().Be(10);
    }

    [Fact]
    public async Task CreateFavouriteAsync_ShouldNotAddDuplicate()
    {
        using var context = GetContext();

        context.Favourites.Add(new FavouriteModel
        {
            EntityTypeId = (int)FavouriteEntityEnum.Film,
            EntityId = 10
        });
        context.SaveChanges();

        var handler = new FavouriteHandler(context);

        var duplicate = new FavouriteModel
        {
            EntityTypeId = (int)FavouriteEntityEnum.Film,
            EntityId = 10
        };

        await handler.CreateFavouriteAsync(duplicate, saveChanges: true);

        context.Favourites.Count().Should().Be(1);
    }

    [Fact]
    public async Task DeleteFavouriteAsync_ShouldRemoveFavourite_WhenExists()
    {
        using var context = GetContext();

        var fav = new FavouriteModel
        {
            EntityTypeId = (int)FavouriteEntityEnum.Film,
            EntityId = 10
        };

        context.Favourites.Add(fav);
        context.SaveChanges();

        var handler = new FavouriteHandler(context);

        await handler.DeleteFavouriteAsync((int)FavouriteEntityEnum.Film, 10, saveChanges: true);

        context.Favourites.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteFavouriteAsync_ShouldDoNothing_WhenNotFound()
    {
        using var context = GetContext();
        var handler = new FavouriteHandler(context);

        await handler.DeleteFavouriteAsync((int)FavouriteEntityEnum.Film, 999, saveChanges: true);

        context.Favourites.Should().BeEmpty();
    }

    [Fact]
    public async Task IsFavouriteAsync_ShouldReturnTrue_WhenExists()
    {
        using var context = GetContext();

        context.Favourites.Add(new FavouriteModel
        {
            EntityTypeId = (int)FavouriteEntityEnum.Genre,
            EntityId = 5
        });
        context.SaveChanges();

        var handler = new FavouriteHandler(context);

        var result = await handler.IsFavouriteAsync((int)FavouriteEntityEnum.Genre, 5);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsFavouriteAsync_ShouldReturnFalse_WhenNotExists()
    {
        using var context = GetContext();
        var handler = new FavouriteHandler(context);

        var result = await handler.IsFavouriteAsync((int)FavouriteEntityEnum.Genre, 5);

        result.Should().BeFalse();
    }

    // ------------------------------
    // GetAllFavouritesAsync Tests
    // ------------------------------

    [Fact]
    public async Task GetAllFavouritesAsync_ShouldReturnFilmFavourites_WithCorrectDetails()
    {
        using var context = GetContext();

        var film = new FilmModel
        {
            FilmId = 10,
            Name = "Test Film",
            BoxCover = new byte[] { 1, 2, 3 }
        };

        context.Films.Add(film);
        context.Favourites.Add(new FavouriteModel
        {
            EntityTypeId = (int)FavouriteEntityEnum.Film,
            EntityId = 10
        });

        context.SaveChanges();

        var handler = new FavouriteHandler(context);

        var result = await handler.GetAllFavouritesAsync();

        result.Should().HaveCount(1);
        var fav = result.Single();

        fav.EntityTypeId.Should().Be((int)FavouriteEntityEnum.Film);
        fav.EntityName.Should().Be("Test Film");
        fav.EntityImage.Should().BeEquivalentTo(new byte[] { 1, 2, 3 });
    }

    [Fact]
    public async Task GetAllFavouritesAsync_ShouldReturnGenreFavourites_WithCorrectDetails()
    {
        using var context = GetContext();

        var genre = new GenreModel
        {
            GenreId = 3,
            Name = "Horror"
        };

        context.Genres.Add(genre);
        context.Favourites.Add(new FavouriteModel
        {
            EntityTypeId = (int)FavouriteEntityEnum.Genre,
            EntityId = 3
        });

        context.SaveChanges();

        var handler = new FavouriteHandler(context);

        var result = await handler.GetAllFavouritesAsync();

        result.Should().HaveCount(1);
        var fav = result.Single();

        fav.EntityTypeId.Should().Be((int)FavouriteEntityEnum.Genre);
        fav.EntityName.Should().Be("Horror");
        fav.EntityImage.Should().BeNull();
    }

    [Fact]
    public async Task GetAllFavouritesAsync_ShouldReturnPersonFavourites_WithCorrectDetails()
    {
        using var context = GetContext();

        var person = new PersonModel
        {
            PersonId = 7,
            FirstName = "John",
            LastName = "Smith",
            Picture = new byte[] { 9, 9 }
        };

        context.People.Add(person);
        context.Favourites.Add(new FavouriteModel
        {
            EntityTypeId = (int)FavouriteEntityEnum.Person,
            EntityId = 7
        });

        context.SaveChanges();

        var handler = new FavouriteHandler(context);

        var result = await handler.GetAllFavouritesAsync();

        result.Should().HaveCount(1);
        var fav = result.Single();

        fav.EntityTypeId.Should().Be((int)FavouriteEntityEnum.Person);
        fav.EntityName.Should().Be("John Smith");
        fav.EntityImage.Should().BeEquivalentTo(new byte[] { 9, 9 });
    }

    [Fact]
    public async Task GetAllFavouritesAsync_ShouldReturnStudioFavourites_WithCorrectDetails()
    {
        using var context = GetContext();

        var studio = new StudioModel
        {
            StudioId = 4,
            Name = "A24",
            Logo = new byte[] { 5, 5 }
        };

        context.Studios.Add(studio);
        context.Favourites.Add(new FavouriteModel
        {
            EntityTypeId = (int)FavouriteEntityEnum.Studio,
            EntityId = 4
        });

        context.SaveChanges();

        var handler = new FavouriteHandler(context);

        var result = await handler.GetAllFavouritesAsync();

        result.Should().HaveCount(1);
        var fav = result.Single();

        fav.EntityTypeId.Should().Be((int)FavouriteEntityEnum.Studio);
        fav.EntityName.Should().Be("A24");
        fav.EntityImage.Should().BeEquivalentTo(new byte[] { 5, 5 });
    }

    [Fact]
    public async Task GetAllFavouritesAsync_ShouldGroupAndOrderByEntityType()
    {
        using var context = GetContext();

        context.Genres.Add(new GenreModel { GenreId = 1, Name = "Drama" });
        context.Films.Add(new FilmModel { FilmId = 2, Name = "Zeta Film" });
        context.Films.Add(new FilmModel { FilmId = 3, Name = "Alpha Film" });

        context.Favourites.AddRange(
            new FavouriteModel { EntityTypeId = (int)FavouriteEntityEnum.Film, EntityId = 2 },
            new FavouriteModel { EntityTypeId = (int)FavouriteEntityEnum.Film, EntityId = 3 },
            new FavouriteModel { EntityTypeId = (int)FavouriteEntityEnum.Genre, EntityId = 1 }
        );

        context.SaveChanges();

        var handler = new FavouriteHandler(context);

        var result = await handler.GetAllFavouritesAsync();

        result.Should().HaveCount(3);

        // Ordered by EntityTypeId first (Genre before Film)
        result[0].EntityTypeId.Should().Be((int)FavouriteEntityEnum.Film);

        // Film favourites ordered by Film.Name
        var filmNames = result.Where(r => r.EntityTypeId == (int)FavouriteEntityEnum.Film)
                              .Select(r => r.EntityName)
                              .ToList();

        filmNames.Should().ContainInOrder("Alpha Film", "Zeta Film");
    }
}
