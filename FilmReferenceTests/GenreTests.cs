namespace FilmReferenceTests;

public class GenreTests
{
    [Fact]
    public async Task CreateGenreAsync_ShouldAddGenre_WhenSaveChangesTrue()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new GenreHandler(context);

        var genre = new GenreModel { GenreId = 1, Name = "Action", Logo = [1,2,3] };

        await handler.CreateGenreAsync(genre, saveChanges: true);

        var result = await context.Genres.FindAsync(1);
        result.Should().NotBeNull();
        result.Name.Should().Be("Action");
        result.Logo.Should().Equal([1,2,3]);
    }

    [Fact]
    public async Task DeleteGenreAsync_Should_RemoveGenre_WhenExists()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new GenreHandler(context);

        var genre = new GenreModel { GenreId = 2, Name = "Comedy" };
        context.Genres.Add(genre);
        await context.SaveChangesAsync();

        await handler.DeleteGenreAsync(2, saveChanges: true);
        
        var result = await context.Genres.FindAsync(2);
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteGenreAsync_ShouldDoNothing_WhenNotFound()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new GenreHandler(context);

        await handler.DeleteGenreAsync(99, saveChanges: true);

        context.Studios.Should().BeEmpty();
    }

    [Fact]
    public async Task GetGenreAsync_ShouldReturnGenreWithFilmsOrdered()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new GenreHandler(context);

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
        await context.SaveChangesAsync();

        var result = await handler.GetGenreAsync(3);

        result.Should().NotBeNull();
        result.Films.Should().HaveCount(2);
        result.Films.First().Name.Should().Be("A Movie"); // ordered
    }

    [Fact]
    public async Task GetGenreAsync_ShouldReturnEmptyGenre_WhenNotFound()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new GenreHandler(context);

        var result = await handler.GetGenreAsync(123);

        result.Should().NotBeNull();
        result.GenreId.Should().Be(0); // default new StudioModel
    }

    [Fact]
    public async Task GetGenressAsync_ShouldReturnGenresOrderedByName()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new GenreHandler(context);

        context.Genres.AddRange(
            new GenreModel { GenreId = 10, Name = "Zeta" },
            new GenreModel { GenreId = 11, Name = "Alpha" }
        );
        await context.SaveChangesAsync();

        var result = await handler.GetGenresAsync();

        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Alpha");
    }

    [Fact]
    public async Task UpdateGenreAsync_ShouldModifyProperties_WhenGenreExists()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new GenreHandler(context);

        var genre = new GenreModel { GenreId = 20, Name = "Old Name", Description = "Old Desc", Logo = [1,2,3] };
        context.Genres.Add(genre);
        await context.SaveChangesAsync();

        var updated = new GenreModel { GenreId = 20, Name = "New Name", Description = "New Desc", Logo = [3,2,1] };
        await handler.UpdateGenreAsync(updated, saveChanges: true);

        var result = await context.Genres.FindAsync(20);
        result!.Name.Should().Be("New Name");
        result.Description.Should().Be("New Desc");
        result.Logo.Should().Equal([3, 2, 1]);
    }

    [Fact]
    public async Task UpdateGenreAsync_ShouldDoNothing_WhenStudioGenreNotFound()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new GenreHandler(context);

        var updated = new GenreModel { GenreId = 999, Name = "Does Not Exist" };
        await handler.UpdateGenreAsync(updated, saveChanges: true);

        context.Genres.Should().BeEmpty();
    }
}
