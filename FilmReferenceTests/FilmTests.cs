namespace FilmReferenceTests;

public class FilmTests
{
    [Fact]
    public async Task CreateFilmAsync_ShouldAddFilmWithoutActors()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new FilmHandler(factory);

        var film = new FilmModel { Name = "Test Film", Description = "Desc", GenreId = 1, StudioId = 1 };

        await handler.CreateFilmAsync(film, []);

        var result = await context.Films.SingleOrDefaultAsync();
        result.Should().NotBeNull();
        result.Name.Should().Be("Test Film");
        result.Description.Should().Be("Desc");
    }

    [Fact]
    public async Task CreateFilmAsync_ShouldAddFilmWithActors_WhenActorIdsProvided()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new FilmHandler(factory);

        var person = new PersonModel { FirstName = "David" };
        context.People.Add(person);
        await context.SaveChangesAsync();

        var film = new FilmModel { Name = "Film With Actor", GenreId = 1, StudioId = 1 };

        await handler.CreateFilmAsync(film, new[] { person.PersonId });

        var filmWithActor = await context.Films.Include(f => f.FilmPerson).SingleAsync();
        filmWithActor.FilmPerson.Should().ContainSingle(fp => fp.PersonId == person.PersonId);
    }

    [Fact]
    public async Task DeleteFilmAsync_ShouldRemoveFilmAndJunctions()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        var film = new FilmModel { Name = "Delete Me" };
        var person = new PersonModel { FirstName = "Actor" };
        var fp = new FilmPersonModel { Film = film, Person = person };

        context.AddRange(film, person, fp);
        await context.SaveChangesAsync();

        var handler = new FilmHandler(factory);
        await handler.DeleteFilmAsync(film.FilmId);

        (await context.Films.AnyAsync()).Should().BeFalse();
        (await context.FilmPeople.AnyAsync()).Should().BeFalse();
    }

    [Fact]
    public async Task GetFilmAsync_ShouldReturnFilmWithOrderedActors()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        var genre = TestDataFactory.CreateGenre();
        var studio = TestDataFactory.CreateStudio();
        var director = TestDataFactory.CreateDirector();
        var film = TestDataFactory.CreateFilm("Film", studio, genre, director);

        var personA = new PersonModel { FirstName = "Zoe" };
        var personB = new PersonModel { FirstName = "Alice" };
        var fpA = new FilmPersonModel { Film = film, Person = personA };
        var fpB = new FilmPersonModel { Film = film, Person = personB };

        context.AddRange(genre, studio, director, film, personA, personB, fpA, fpB);
        await context.SaveChangesAsync();

        var handler = new FilmHandler(factory);
        var result = await handler.GetFilmAsync(film.FilmId);

        result.FilmPerson.Select(fp => fp.PersonId).Should().Contain([personA.PersonId, personB.PersonId]);
        result.Should().NotBeNull();
        result.Name.Should().Be("Film");

        var actorNames = result.FilmPerson.Select(fp => fp.Person.FirstName).ToList();
        actorNames.Should().Contain(new[] { "Alice", "Zoe" });
        actorNames.Should().ContainInOrder("Alice", "Zoe");

    }

    [Fact]
    public async Task GetFilmAsync_ShouldReturnEmptyFilm_WhenNotFound()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new FilmHandler(factory);

        var result = await handler.GetFilmAsync(999);

        result.Should().NotBeNull();
        result.Name.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task GetAllFilmsAsync_ShouldReturnFilmsOrderedByName()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        var genre = new GenreModel { GenreId = 1, Name = "Action" };
        context.Genres.Add(genre);

        var filmA = new FilmModel { Name = "Zeta", GenreId = 1, StudioId = 1 };
        var filmB = new FilmModel { Name = "Alpha", GenreId = 1, StudioId = 1 };

        context.AddRange(filmA, filmB);
        await context.SaveChangesAsync();

        var handler = new FilmHandler(factory);
        var result = await handler.GetAllFilmsAsync();

        result.Select(f => f.Name).Should().ContainInOrder("Alpha", "Zeta");
    }

    [Fact]
    public async Task UpdateFilmAsync_ShouldUpdateFilmAndActors()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        var film = new FilmModel { Name = "Old Name", GenreId = 1, StudioId = 1 };
        var person = new PersonModel { FirstName = "Actor" };
        context.AddRange(film, person);
        await context.SaveChangesAsync();

        context.ChangeTracker.Clear(); // Clear the change tracker to simulate a fresh context

        var handler = new FilmHandler(factory);
        var updatedFilm = new FilmModel { FilmId = film.FilmId, Name = "New Name", GenreId = 1, StudioId = 1 };

        await handler.UpdateFilmAsync(updatedFilm, new[] { person.PersonId });

        var result = await context.Films.Include(f => f.FilmPerson).SingleAsync();
        result.Name.Should().Be("New Name");
        result.FilmPerson.Should().ContainSingle(fp => fp.PersonId == person.PersonId);
    }
}