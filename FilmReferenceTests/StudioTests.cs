namespace FilmReferenceTests;

public class StudioTests
{
    [Fact]
    public async Task CreateStudioAsync_ShouldAddStudio()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new StudioHandler(factory);

        var studio = new StudioModel { StudioId = 1, Name = "Warner Bros" };

        await handler.CreateStudioAsync(studio);

        var result = await context.Studios.FindAsync(1);
        result.Should().NotBeNull();
        result.Name.Should().Be("Warner Bros");
    }

    [Fact]
    public async Task DeleteStudioAsync_ShouldRemoveStudio_WhenExists()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        var studio = new StudioModel { StudioId = 2, Name = "Paramount" };
        context.Studios.Add(studio);
        await context.SaveChangesAsync();

        // Clear so FindAsync queries the DB rather than returning the stale cached entity.
        context.ChangeTracker.Clear();

        var handler = new StudioHandler(factory);
        await handler.DeleteStudioAsync(2);

        var result = await context.Studios.FindAsync(2);
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteStudioAsync_ShouldDoNothing_WhenNotFound()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new StudioHandler(factory);

        await handler.DeleteStudioAsync(99);

        context.Studios.Should().BeEmpty();
    }

    [Fact]
    public async Task GetStudioAsync_ShouldReturnStudioWithFilmsOrdered()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new StudioHandler(factory);

        var studio = TestDataFactory.CreateStudio();
        var genre = TestDataFactory.CreateGenre();
        var director = TestDataFactory.CreateDirector();
        var filmA = TestDataFactory.CreateFilm("Z Movie", studio, genre, director);
        var filmB = TestDataFactory.CreateFilm("A Movie", studio, genre, director);

        context.AddRange(studio, genre, director, filmA, filmB);
        await context.SaveChangesAsync();

        var result = await handler.GetStudioAsync(studio.StudioId);

        result.Should().NotBeNull();
        result.Films.Should().HaveCount(2);
        result.Films.Select(f => f.Name).Should().ContainInOrder("A Movie", "Z Movie");
    }

    [Fact]
    public async Task GetStudioAsync_ShouldReturnEmptyStudio_WhenNotFound()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new StudioHandler(factory);

        var result = await handler.GetStudioAsync(123);

        result.Should().NotBeNull();
        result.StudioId.Should().Be(0);
    }

    [Fact]
    public async Task GetStudiosAsync_ShouldReturnStudiosOrderedByName()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new StudioHandler(factory);

        context.Studios.AddRange(
            new StudioModel { StudioId = 10, Name = "Zeta" },
            new StudioModel { StudioId = 11, Name = "Alpha" }
        );
        await context.SaveChangesAsync();

        var result = await handler.GetStudiosAsync();

        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Alpha");
    }

    [Fact]
    public async Task UpdateStudioAsync_ShouldModifyProperties_WhenStudioExists()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        var studio = new StudioModel { StudioId = 20, Name = "Old Name", Description = "Old Desc" };
        context.Studios.Add(studio);
        await context.SaveChangesAsync();

        // Clear so FindAsync queries the DB rather than returning the stale cached entity.
        context.ChangeTracker.Clear();

        var handler = new StudioHandler(factory);
        var updated = new StudioModel { StudioId = 20, Name = "New Name", Description = "New Desc" };
        await handler.UpdateStudioAsync(updated);

        var result = await context.Studios.FindAsync(20);
        result!.Name.Should().Be("New Name");
        result.Description.Should().Be("New Desc");
    }

    [Fact]
    public async Task UpdateStudioAsync_ShouldDoNothing_WhenStudioNotFound()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new StudioHandler(factory);

        var updated = new StudioModel { StudioId = 999, Name = "Does Not Exist" };
        await handler.UpdateStudioAsync(updated);

        context.Studios.Should().BeEmpty();
    }

    [Fact]
    public async Task GetStudiosLightweightAsync_ShouldReturnProjectedStudiosOrdered()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new StudioHandler(factory);

        context.Studios.AddRange(
            new StudioModel { StudioId = 1, Name = "Universal" },
            new StudioModel { StudioId = 2, Name = "Paramount" }
        );
        await context.SaveChangesAsync();

        var result = await handler.GetStudiosLightweightAsync();

        result.Should().HaveCount(2);

        result[0].Name.Should().Be("Paramount");
        result[1].Name.Should().Be("Universal");
    }

    [Fact]
    public async Task GetStudiosLightweightAsync_ShouldReturnEmptyList_WhenNoStudiosExist()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new StudioHandler(factory);

        var result = await handler.GetStudiosLightweightAsync();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
