namespace FilmReferenceBlazor.Tests;

public class FilmHandlerTests
{
    [Fact]
    public async Task CreateFilmAsync_ShouldAddFilmWithoutActors_WhenSaveChangesTrue()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new FilmHandler(context);

        var film = new FilmModel { Name = "Test Film", Description = "Desc", GenreId = 1, StudioId = 1 };

        await handler.CreateFilmAsync(film, null, saveChanges: true);

        var result = await context.Films.SingleOrDefaultAsync();
        result.Should().NotBeNull();
        result.Name.Should().Be("Test Film");
        result.Description.Should().Be("Desc");
    }

    [Fact]
    public async Task CreateFilmAsync_ShouldAddFilmWithActors_WhenActorIdsProvided()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new FilmHandler(context);

        var person = new PersonModel { FirstName = "David" };
        context.People.Add(person);
        context.SaveChanges();

        var film = new FilmModel { Name = "Film With Actor", GenreId = 1, StudioId = 1 };

        await handler.CreateFilmAsync(film, new[] { person.PersonId }, saveChanges: true);

        var filmWithActor = await context.Films.Include(f => f.FilmPerson).SingleAsync();
        filmWithActor.FilmPerson.Should().ContainSingle(fp => fp.PersonId == person.PersonId);
    }

    [Fact]
    public async Task DeleteFilmAsync_ShouldRemoveFilmAndJunctions()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var film = new FilmModel { Name = "Delete Me" };
        var person = new PersonModel { FirstName = "Actor" };
        var fp = new FilmPersonModel { Film = film, Person = person };

        context.AddRange(film, person, fp);
        context.SaveChanges();

        var handler = new FilmHandler(context);
        await handler.DeleteFilmAsync(film.FilmId, saveChanges: true);

        (await context.Films.AnyAsync()).Should().BeFalse();
        (await context.FilmPeople.AnyAsync()).Should().BeFalse();
    }

    [Fact]
    public async Task GetFilmAsync_ShouldReturnFilmWithOrderedActors()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var genre = new GenreModel { Name = "Drama" };
        var studio = new StudioModel { Name = "Test Studio" };
        var director = new PersonModel { FirstName = "Director" };

        var film = new FilmModel
        {
            Name = "Film",
            Genre = genre,
            Studio = studio,
            Director = director
        };

        var personA = new PersonModel { FirstName = "Zoe" };
        var personB = new PersonModel { FirstName = "Alice" };
        var fpA = new FilmPersonModel { Film = film, Person = personA };
        var fpB = new FilmPersonModel { Film = film, Person = personB };

        context.AddRange(genre, studio, director, film, personA, personB, fpA, fpB);
        context.SaveChanges();

        var handler = new FilmHandler(context);
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
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new FilmHandler(context);

        var result = await handler.GetFilmAsync(999);

        result.Should().NotBeNull();
        result.Name.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task GetAllFilmsAsync_ShouldReturnFilmsOrderedByName()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var filmA = new FilmModel { Name = "Zeta", GenreId = 1, StudioId = 1 };
        var filmB = new FilmModel { Name = "Alpha", GenreId = 1, StudioId = 1 };
        context.AddRange(filmA, filmB);
        context.SaveChanges();

        var handler = new FilmHandler(context);
        var result = await handler.GetAllFilmsAsync();

        result.Select(f => f.Name).Should().ContainInOrder("Alpha", "Zeta");
    }

    [Fact]
    public async Task UpdateFilmAsync_ShouldUpdateFilmAndActors()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var film = new FilmModel { Name = "Old Name", GenreId = 1, StudioId = 1 };
        var person = new PersonModel { FirstName = "Actor" };
        context.AddRange(film, person);
        context.SaveChanges();

        var handler = new FilmHandler(context);
        var updatedFilm = new FilmModel { FilmId = film.FilmId, Name = "New Name", GenreId = 1, StudioId = 1 };

        await handler.UpdateFilmAsync(updatedFilm, new[] { person.PersonId }, saveChanges: true);

        var result = await context.Films.Include(f => f.FilmPerson).SingleAsync();
        result.Name.Should().Be("New Name");
        result.FilmPerson.Should().ContainSingle(fp => fp.PersonId == person.PersonId);
    }
}