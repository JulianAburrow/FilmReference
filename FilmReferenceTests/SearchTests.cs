namespace FilmReferenceTests;

public class SearchTests
{
    // Note: SearchHandler uses EF.Functions.Collate which is a SQL Server feature.
    // The InMemory provider falls back to a standard string Contains, which is
    // case-sensitive. Test data and search terms are matched in case to keep
    // tests meaningful. On SQL Server the CI_AI collation makes searches case
    // and accent insensitive.
    //
    // Name (and FirstName/LastName for people) is always searched. Description and
    // Notes are only searched when includeDescriptionAndNotes is true.

    // ---------- Films ----------

    [Fact]
    public async Task SearchFilmsAsync_ShouldReturnEmpty_WhenSearchTextIsNull()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new SearchHandler(factory);

        var result = await handler.SearchFilmsAsync(null!, includeDescriptionAndNotes: false);

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
        var result = await handler.SearchFilmsAsync("   ", includeDescriptionAndNotes: false);

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
        var result = await handler.SearchFilmsAsync("Star", includeDescriptionAndNotes: false);

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
        var result = await handler.SearchFilmsAsync("xyz", includeDescriptionAndNotes: false);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchFilmsAsync_ShouldNotMatchDescriptionOrNotes_WhenFlagIsFalse()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Films.AddRange(
            new FilmModel { Name = "Alien", Description = "Space horror" },
            new FilmModel { Name = "Jaws", Notes = "Space horror of the sea" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchFilmsAsync("Space", includeDescriptionAndNotes: false);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchFilmsAsync_ShouldMatchOnDescription_WhenFlagIsTrue()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Films.AddRange(
            new FilmModel { Name = "Alien", Description = "Space horror" },
            new FilmModel { Name = "Jaws", Description = "Shark thriller" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchFilmsAsync("Space", includeDescriptionAndNotes: true);

        result.Should().ContainSingle(f => f.Name == "Alien");
    }

    [Fact]
    public async Task SearchFilmsAsync_ShouldMatchOnNotes_WhenFlagIsTrue()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Films.AddRange(
            new FilmModel { Name = "Alien", Notes = "Director's cut available" },
            new FilmModel { Name = "Jaws", Notes = "Original release" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchFilmsAsync("Director", includeDescriptionAndNotes: true);

        result.Should().ContainSingle(f => f.Name == "Alien");
    }

    [Fact]
    public async Task SearchFilmsAsync_ShouldStillMatchOnName_WhenFlagIsTrue()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Films.AddRange(
            new FilmModel { Name = "Alien", Description = null, Notes = null },
            new FilmModel { Name = "Jaws" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchFilmsAsync("Alien", includeDescriptionAndNotes: true);

        result.Should().ContainSingle(f => f.Name == "Alien");
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
        var result = await handler.SearchGenresAsync(string.Empty, includeDescriptionAndNotes: false);

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
        var result = await handler.SearchGenresAsync("Fiction", includeDescriptionAndNotes: false);

        result.Should().HaveCount(2);
        result.Select(g => g.Name).Should().ContainInOrder("Fiction", "Science Fiction");
    }

    [Fact]
    public async Task SearchGenresAsync_ShouldNotMatchDescriptionOrNotes_WhenFlagIsFalse()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Genres.AddRange(
            new GenreModel { Name = "Action", Description = "Explosive stunts" },
            new GenreModel { Name = "Drama", Notes = "Explosive performances" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchGenresAsync("Explosive", includeDescriptionAndNotes: false);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchGenresAsync_ShouldMatchOnDescriptionAndNotes_WhenFlagIsTrue()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Genres.AddRange(
            new GenreModel { Name = "Action", Description = "Explosive stunts" },
            new GenreModel { Name = "Drama", Notes = "Explosive performances" },
            new GenreModel { Name = "Comedy", Description = "Light-hearted" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchGenresAsync("Explosive", includeDescriptionAndNotes: true);

        result.Should().HaveCount(2);
        result.Select(g => g.Name).Should().ContainInOrder("Action", "Drama");
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
        var result = await handler.SearchStudiosAsync(string.Empty, includeDescriptionAndNotes: false);

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
        var result = await handler.SearchStudiosAsync("Warner", includeDescriptionAndNotes: false);

        result.Should().HaveCount(2);
        result.Select(s => s.Name).Should().ContainInOrder("Warner Bros", "Warner Independent");
    }

    [Fact]
    public async Task SearchStudiosAsync_ShouldNotMatchDescriptionOrNotes_WhenFlagIsFalse()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Studios.AddRange(
            new StudioModel { Name = "Paramount", Description = "Hollywood major" },
            new StudioModel { Name = "A24", Notes = "Hollywood indie" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchStudiosAsync("Hollywood", includeDescriptionAndNotes: false);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchStudiosAsync_ShouldMatchOnDescriptionAndNotes_WhenFlagIsTrue()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.Studios.AddRange(
            new StudioModel { Name = "Paramount", Description = "Hollywood major" },
            new StudioModel { Name = "A24", Notes = "Hollywood indie" },
            new StudioModel { Name = "Pixar", Description = "Animation" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchStudiosAsync("Hollywood", includeDescriptionAndNotes: true);

        result.Should().HaveCount(2);
        result.Select(s => s.Name).Should().ContainInOrder("A24", "Paramount");
    }

    // ---------- People ----------

    [Fact]
    public async Task SearchPeopleAsync_ShouldReturnEmpty_WhenSearchTextIsNull()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new SearchHandler(factory);

        var result = await handler.SearchPeopleAsync(null!, includeDescriptionAndNotes: false);

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
        var result = await handler.SearchPeopleAsync("   ", includeDescriptionAndNotes: false);

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
        var result = await handler.SearchPeopleAsync("John", includeDescriptionAndNotes: false);

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
        var result = await handler.SearchPeopleAsync("Smith", includeDescriptionAndNotes: false);

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
        var result = await handler.SearchPeopleAsync("John Smith", includeDescriptionAndNotes: false);

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
        var result = await handler.SearchPeopleAsync("Alice", includeDescriptionAndNotes: false);

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
        var result = await handler.SearchPeopleAsync("xyz", includeDescriptionAndNotes: false);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchPeopleAsync_ShouldNotMatchDescriptionOrNotes_WhenFlagIsFalse()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.People.AddRange(
            new PersonModel { FirstName = "John", LastName = "Smith", Description = "Stuntman" },
            new PersonModel { FirstName = "Jane", LastName = "Doe", Notes = "Former stuntman" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchPeopleAsync("stuntman", includeDescriptionAndNotes: false);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchPeopleAsync_ShouldMatchOnDescription_WhenFlagIsTrue()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.People.AddRange(
            new PersonModel { FirstName = "John", LastName = "Smith", Description = "Stuntman" },
            new PersonModel { FirstName = "Jane", LastName = "Doe", Description = "Actress" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchPeopleAsync("Stuntman", includeDescriptionAndNotes: true);

        result.Should().ContainSingle(p => p.FirstName == "John");
    }

    [Fact]
    public async Task SearchPeopleAsync_ShouldMatchOnNotes_WhenFlagIsTrue()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.People.AddRange(
            new PersonModel { FirstName = "John", LastName = "Smith", Notes = "Born in Leeds" },
            new PersonModel { FirstName = "Jane", LastName = "Doe", Notes = "Born in York" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchPeopleAsync("Leeds", includeDescriptionAndNotes: true);

        result.Should().ContainSingle(p => p.FirstName == "John");
    }

    [Fact]
    public async Task SearchPeopleAsync_ShouldStillMatchOnNames_WhenFlagIsTrueAndFieldsAreNull()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);

        context.People.AddRange(
            new PersonModel { FirstName = "John", LastName = null, Description = null, Notes = null },
            new PersonModel { FirstName = "Jane", LastName = "Doe" }
        );
        await context.SaveChangesAsync(CT.Token);

        var handler = new SearchHandler(factory);
        var result = await handler.SearchPeopleAsync("John", includeDescriptionAndNotes: true);

        result.Should().ContainSingle(p => p.FirstName == "John");
    }
}