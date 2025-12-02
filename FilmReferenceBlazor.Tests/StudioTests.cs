namespace FilmReferenceBlazor.Tests;

public class StudioTests
{
    [Fact]
    public async Task CreateStudioAsync_ShouldAddStudio_WhenSaveChangesTrue()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new StudioHandler(context);

        var studio = new StudioModel { StudioId = 1, Name = "Warner Bros" };

        await handler.CreateStudioAsync(studio, saveChanges: true);

        var result = await context.Studios.FindAsync(1);
        result.Should().NotBeNull();
        result.Name.Should().Be("Warner Bros");
    }

    [Fact]
    public async Task DeleteStudioAsync_ShouldRemoveStudio_WhenExists()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new StudioHandler(context);

        var studio = new StudioModel { StudioId = 2, Name = "Paramount" };
        context.Studios.Add(studio);
        await context.SaveChangesAsync();

        await handler.DeleteStudioAsync(2, saveChanges: true);

        var result = await context.Studios.FindAsync(2);
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteStudioAsync_ShouldDoNothing_WhenNotFound()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new StudioHandler(context);

        await handler.DeleteStudioAsync(99, saveChanges: true);

        context.Studios.Should().BeEmpty();
    }

    [Fact]
    public async Task GetStudioAsync_ShouldReturnStudioWithFilmsOrdered()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new StudioHandler(context);

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
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new StudioHandler(context);

        var result = await handler.GetStudioAsync(123);

        result.Should().NotBeNull();
        result.StudioId.Should().Be(0); // default new StudioModel
    }

    [Fact]
    public async Task GetStudiosAsync_ShouldReturnStudiosOrderedByName()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new StudioHandler(context);

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
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new StudioHandler(context);

        var studio = new StudioModel { StudioId = 20, Name = "Old Name", Description = "Old Desc" };
        context.Studios.Add(studio);
        await context.SaveChangesAsync();

        var updated = new StudioModel { StudioId = 20, Name = "New Name", Description = "New Desc" };
        await handler.UpdateStudioAsync(updated, saveChanges: true);

        var result = await context.Studios.FindAsync(20);
        result!.Name.Should().Be("New Name");
        result.Description.Should().Be("New Desc");
    }

    [Fact]
    public async Task UpdateStudioAsync_ShouldDoNothing_WhenStudioNotFound()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new StudioHandler(context);

        var updated = new StudioModel { StudioId = 999, Name = "Does Not Exist" };
        await handler.UpdateStudioAsync(updated, saveChanges: true);

        context.Studios.Should().BeEmpty();
    }
}