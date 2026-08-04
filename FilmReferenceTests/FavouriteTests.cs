using FilmReferenceDataAccess.Interfaces;

namespace FilmReferenceTests;

public class FavouriteTests
{
    // ----------------------------------------------------------------------
    // CREATE
    // ----------------------------------------------------------------------

    [Fact]
    public async Task CreateFavouriteAsync_ShouldAddFavourite_WhenNotDuplicate()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new FavouriteHandler(factory);

        var fav = new FavouriteModel
        {
            EntityTypeId = (int)FavouriteEntityEnum.Film,
            EntityId = 10
        };

        await handler.CreateFavouriteAsync(fav);

        context.ChangeTracker.Clear();

        var result = await context.Favourites.FirstOrDefaultAsync(CT.Token);
        result.Should().NotBeNull();
        result.EntityTypeId.Should().Be((int)FavouriteEntityEnum.Film);
        result.EntityId.Should().Be(10);
    }

    [Fact]
    public async Task CreateFavouriteAsync_ShouldNotAddDuplicate()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Favourites.Add(new FavouriteModel
        {
            EntityTypeId = (int)FavouriteEntityEnum.Film,
            EntityId = 10
        });
        await context.SaveChangesAsync(CT.Token);

        var handler = new FavouriteHandler(factory);

        var duplicate = new FavouriteModel
        {
            EntityTypeId = (int)FavouriteEntityEnum.Film,
            EntityId = 10
        };

        await handler.CreateFavouriteAsync(duplicate);

        context.ChangeTracker.Clear();

        context.Favourites.Count().Should().Be(1);
    }

    // ----------------------------------------------------------------------
    // DELETE
    // ----------------------------------------------------------------------

    [Fact]
    public async Task DeleteFavouriteAsync_ShouldRemoveFavourite_WhenExists()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        var fav = new FavouriteModel
        {
            EntityTypeId = (int)FavouriteEntityEnum.Film,
            EntityId = 10
        };

        context.Favourites.Add(fav);
        await context.SaveChangesAsync(CT.Token);

        var handler = new FavouriteHandler(factory);

        await handler.DeleteFavouriteAsync((int)FavouriteEntityEnum.Film, 10);

        context.ChangeTracker.Clear();

        context.Favourites.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteFavouriteAsync_ShouldDoNothing_WhenNotFound()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new FavouriteHandler(factory);

        await handler.DeleteFavouriteAsync((int)FavouriteEntityEnum.Film, 999);

        context.Favourites.Should().BeEmpty();
    }

    // ----------------------------------------------------------------------
    // IS FAVOURITE
    // ----------------------------------------------------------------------

    [Fact]
    public async Task IsFavouriteAsync_ShouldReturnTrue_WhenExists()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Favourites.Add(new FavouriteModel
        {
            EntityTypeId = (int)FavouriteEntityEnum.Genre,
            EntityId = 5
        });
        await context.SaveChangesAsync(CT.Token);

        var handler = new FavouriteHandler(factory);

        var result = await handler.IsFavouriteAsync((int)FavouriteEntityEnum.Genre, 5);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsFavouriteAsync_ShouldReturnFalse_WhenNotExists()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        var handler = new FavouriteHandler(factory);

        var result = await handler.IsFavouriteAsync((int)FavouriteEntityEnum.Genre, 5);

        result.Should().BeFalse();
    }

    // ----------------------------------------------------------------------
    // GET ALL FAVOURITES
    // ----------------------------------------------------------------------

    [Fact]
    public async Task GetAllFavouritesAsync_ShouldReturnFilmFavourites_WithCorrectDetails()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

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

        await context.SaveChangesAsync(CT.Token);

        var handler = new FavouriteHandler(factory);

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
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

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

        await context.SaveChangesAsync(CT.Token);

        var handler = new FavouriteHandler(factory);

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
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

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

        await context.SaveChangesAsync(CT.Token);

        var handler = new FavouriteHandler(factory);

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
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

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

        await context.SaveChangesAsync(CT.Token);

        var handler = new FavouriteHandler(factory);

        var result = await handler.GetAllFavouritesAsync();

        result.Should().HaveCount(1);
        var fav = result.Single();

        fav.EntityTypeId.Should().Be((int)FavouriteEntityEnum.Studio);
        fav.EntityName.Should().Be("A24");
        fav.EntityImage.Should().BeEquivalentTo(new byte[] { 5, 5 });
    }

    [Fact]
    public async Task GetAllFavouritesAsync_ShouldGroupAndOrderFilmFavouritesByName()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Genres.Add(new GenreModel { GenreId = 1, Name = "Drama" });
        context.Films.Add(new FilmModel { FilmId = 2, Name = "Zeta Film" });
        context.Films.Add(new FilmModel { FilmId = 3, Name = "Alpha Film" });

        context.Favourites.AddRange(
            new FavouriteModel { EntityTypeId = (int)FavouriteEntityEnum.Film, EntityId = 2 },
            new FavouriteModel { EntityTypeId = (int)FavouriteEntityEnum.Film, EntityId = 3 },
            new FavouriteModel { EntityTypeId = (int)FavouriteEntityEnum.Genre, EntityId = 1 }
        );

        await context.SaveChangesAsync(CT.Token);

        var handler = new FavouriteHandler(factory);

        var result = await handler.GetAllFavouritesAsync();

        result.Should().HaveCount(3);

        var filmNames = result
            .Where(r => r.EntityTypeId == (int)FavouriteEntityEnum.Film)
            .Select(r => r.EntityName)
            .ToList();

        filmNames.Should().ContainInOrder("Alpha Film", "Zeta Film");
    }
}