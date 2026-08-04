namespace FilmReferenceTests;

public class GenreTests
{
    [Fact]
    public async Task CreateGenreAsync_ShouldAddGenre()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new GenreHandler(factory);

        var genre = new GenreModel { GenreId = 1, Name = "Action", Logo = [1,2,3] };

        await handler.CreateGenreAsync(genre);

        var result = await context.Genres.FindAsync(new object[] { 1 }, CT.Token);
        result.Should().NotBeNull();
        result.Name.Should().Be("Action");
        result.Logo.Should().Equal([1,2,3]);
    }

    [Fact]
    public async Task DeleteGenreAsync_ShouldRemoveGenre_WhenExists()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        var genre = new GenreModel { GenreId = 2, Name = "Comedy" };
        context.Genres.Add(genre);
        await context.SaveChangesAsync(CT.Token);

        context.ChangeTracker.Clear(); // Clear the change tracker to simulate a fresh context

        var handler = new GenreHandler(factory);
        await handler.DeleteGenreAsync(2);
        
        var result = await context.Genres.FindAsync(new object[] { 2 }, CT.Token);
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteGenreAsync_ShouldDoNothing_WhenNotFound()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new GenreHandler(factory);

        await handler.DeleteGenreAsync(99);

        context.Genres.Should().BeEmpty();
    }

    [Fact]
    public async Task GetGenreAsync_ShouldReturnGenreWithFilmsOrdered()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new GenreHandler(factory);

        var genre = new GenreModel
        {
            GenreId = 3,
            Name = "Fantasy",
            Films =
            [
                new() { FilmId = 1, Name = "Z Movie" },
                new() { FilmId = 2, Name = "A Movie" }
            ]
        };
        context.Genres.Add(genre);
        await context.SaveChangesAsync(CT.Token);

        var result = await handler.GetGenreAsync(3);

        result.Should().NotBeNull();
        result.Films.Should().HaveCount(2);
        result.Films.First().Name.Should().Be("A Movie"); // ordered
    }

    [Fact]
    public async Task GetGenreAsync_ShouldReturnEmptyGenre_WhenNotFound()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new GenreHandler(factory);

        var result = await handler.GetGenreAsync(123);

        result.Should().NotBeNull();
        result.GenreId.Should().Be(0); // default new StudioModel
    }

    [Fact]
    public async Task GetGenresAsync_ShouldReturnGenresOrderedByName()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new GenreHandler(factory);

        context.Genres.AddRange(
            new GenreModel { GenreId = 10, Name = "Zeta" },
            new GenreModel { GenreId = 11, Name = "Alpha" }
        );
        await context.SaveChangesAsync(CT.Token);

        var result = await handler.GetGenresAsync();

        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Alpha");
    }

    [Fact]
    public async Task UpdateGenreAsync_ShouldModifyProperties_WhenGenreExists()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        var genre = new GenreModel { GenreId = 20, Name = "Old Name", Description = "Old Desc", Logo = [1,2,3] };
        context.Genres.Add(genre);
        await context.SaveChangesAsync(CT.Token);

        context.ChangeTracker.Clear();

        var handler = new GenreHandler(factory);
        var updated = new GenreModel { GenreId = 20, Name = "New Name", Description = "New Desc", Logo = [3,2,1] };
        await handler.UpdateGenreAsync(updated);

        var result = await context.Genres.FindAsync(new object[] { 20 }, CT.Token);
        result!.Name.Should().Be("New Name");
        result.Description.Should().Be("New Desc");
        result.Logo.Should().Equal([3, 2, 1]);
    }

    [Fact]
    public async Task UpdateGenreAsync_ShouldDoNothing_WhenStudioGenreNotFound()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new GenreHandler(factory);

        var updated = new GenreModel { GenreId = 999, Name = "Does Not Exist" };
        await handler.UpdateGenreAsync(updated);

        context.Genres.Should().BeEmpty();
    }

    [Fact]
    public async Task GetGenresLightweightAsync_ShouldReturnProjectedGenresOrdered()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new GenreHandler(factory);

        context.Genres.AddRange(
            new GenreModel { GenreId = 10, Name = "Zeta" },
            new GenreModel { GenreId = 11, Name = "Alpha" }
        );
        await context.SaveChangesAsync(CT.Token);

        var result = await handler.GetGenresLightweightAsync();

        result.Should().HaveCount(2);

        // Ordered by Name ascending
        result[0].GenreId.Should().Be(11);
        result[0].Name.Should().Be("Alpha");

        result[1].GenreId.Should().Be(10);
        result[1].Name.Should().Be("Zeta");
    }

    [Fact]
    public async Task GetGenresLightweightAsync_ShouldReturnEmptyList_WhenNoGenresExist()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new GenreHandler(factory);

        var result = await handler.GetGenresLightweightAsync();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task DeleteGenre_AlsoDeletesFavourite_WhenGenreIsFavourite()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Genres.Add(new GenreModel { GenreId = 300, Name = "Genre", Description = "Description" });
        context.Favourites.Add(new FavouriteModel { EntityId = 300, EntityTypeId = (int)FavouriteEntityEnum.Genre });
        context.Favourites.Add(new FavouriteModel { EntityId = 999, EntityTypeId = (int)FavouriteEntityEnum.Genre });

        await context.SaveChangesAsync(CT.Token);
        context.ChangeTracker.Clear();

        var handler = new GenreHandler(factory);

        await handler.DeleteGenreAsync(300);

        await using var verify = await factory.CreateDbContextAsync(CT.Token);
        verify.Genres.Should().BeEmpty();
        verify.Favourites.Where(f => f.EntityId == 300 && f.EntityTypeId == (int)FavouriteEntityEnum.Genre).Should().BeEmpty();
        verify.Favourites.Where(f => f.EntityId == 999 && f.EntityTypeId == (int)FavouriteEntityEnum.Genre).Should().HaveCount(1);
    }
}
