namespace FilmReferenceTests;

public class SearchTests
{
    // Note: SearchHandler uses EF.Functions.Collate which is a SQL Server feature.
    // The InMemory provider falls back to a standard string Contains, which is
    // case-sensitive. Test data and search terms are matched in case to keep
    // tests meaningful. On SQL Server the CI_AI collation makes searches case
    // and accent insensitive.

    // ---------- Films ----------

    [Fact]
    public async Task SearchFilmsAsync_ShouldReturnEmpty_WhenSearchTextIsNull()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new SearchHandler(factory);

        var result = await handler.SearchFilmsAsync(null!);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchFilmsAsync_ShouldReturnEmpty_WhenSearchTextIsWhitespace()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Films.Add(new FilmModel { Name = "Star Wars" });
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchFilmsAsync("   ");

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchFilmsAsync_ShouldReturnMatchingFilms_OrderedByName()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Films.AddRange(
            new FilmModel { Name = "Star Wars" },
            new FilmModel { Name = "Star Trek" },
            new FilmModel { Name = "Alien" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchFilmsAsync("Star");

        result.Should().HaveCount(2);
        result.Select(f => f.Name).Should().ContainInOrder("Star Trek", "Star Wars");
    }

    [Fact]
    public async Task SearchFilmsAsync_ShouldReturnEmpty_WhenNoMatch()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Films.Add(new FilmModel { Name = "Alien" });
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchFilmsAsync("xyz");

        result.Should().BeEmpty();
    }

    // ---------- Genres ----------

    [Fact]
    public async Task SearchGenresAsync_ShouldReturnEmpty_WhenSearchTextIsEmpty()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Genres.Add(new GenreModel { Name = "Action" });
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchGenresAsync(string.Empty);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchGenresAsync_ShouldReturnMatchingGenres_OrderedByName()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Genres.AddRange(
            new GenreModel { Name = "Science Fiction" },
            new GenreModel { Name = "Fiction" },
            new GenreModel { Name = "Action" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchGenresAsync("Fiction");

        result.Should().HaveCount(2);
        result.Select(g => g.Name).Should().ContainInOrder("Fiction", "Science Fiction");
    }

    // ---------- Studios ----------

    [Fact]
    public async Task SearchStudiosAsync_ShouldReturnEmpty_WhenSearchTextIsEmpty()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Studios.Add(new StudioModel { Name = "Warner Bros" });
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchStudiosAsync(string.Empty);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchStudiosAsync_ShouldReturnMatchingStudios_OrderedByName()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Studios.AddRange(
            new StudioModel { Name = "Warner Bros" },
            new StudioModel { Name = "Warner Independent" },
            new StudioModel { Name = "Paramount" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchStudiosAsync("Warner");

        result.Should().HaveCount(2);
        result.Select(s => s.Name).Should().ContainInOrder("Warner Bros", "Warner Independent");
    }

    // ---------- People ----------

    [Fact]
    public async Task SearchPeopleAsync_ShouldReturnEmpty_WhenSearchTextIsNull()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new SearchHandler(factory);

        var result = await handler.SearchPeopleAsync(null!);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchPeopleAsync_ShouldReturnEmpty_WhenSearchTextIsWhitespace()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.People.Add(new PersonModel { FirstName = "John", LastName = "Smith" });
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchPeopleAsync("   ");

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchPeopleAsync_ShouldMatchOnFirstName()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.People.AddRange(
            new PersonModel { FirstName = "John", LastName = "Smith" },
            new PersonModel { FirstName = "Jane", LastName = "Doe" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchPeopleAsync("John");

        result.Should().ContainSingle(p => p.FirstName == "John");
    }

    [Fact]
    public async Task SearchPeopleAsync_ShouldMatchOnLastName()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.People.AddRange(
            new PersonModel { FirstName = "John", LastName = "Smith" },
            new PersonModel { FirstName = "Jane", LastName = "Doe" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchPeopleAsync("Smith");

        result.Should().ContainSingle(p => p.LastName == "Smith");
    }

    [Fact]
    public async Task SearchPeopleAsync_ShouldMatchMultipleWordsAcrossFirstAndLastName()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.People.AddRange(
            new PersonModel { FirstName = "John", LastName = "Smith" },
            new PersonModel { FirstName = "John", LastName = "Doe" },
            new PersonModel { FirstName = "Jane", LastName = "Smith" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);

        // Both parts must match -- "John Smith" should only return John Smith
        var result = await handler.SearchPeopleAsync("John Smith");

        result.Should().ContainSingle(p => p.FirstName == "John" && p.LastName == "Smith");
    }

    [Fact]
    public async Task SearchPeopleAsync_ShouldReturnResultsOrderedByFirstNameThenLastName()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.People.AddRange(
            new PersonModel { FirstName = "Alice", LastName = "Zulu" },
            new PersonModel { FirstName = "Alice", LastName = "Alpha" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchPeopleAsync("Alice");

        result.Select(p => p.LastName).Should().ContainInOrder("Alpha", "Zulu");
    }

    [Fact]
    public async Task SearchPeopleAsync_ShouldReturnEmpty_WhenNoMatch()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.People.Add(new PersonModel { FirstName = "John", LastName = "Smith" });
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchPeopleAsync("xyz");

        result.Should().BeEmpty();
    }
}
