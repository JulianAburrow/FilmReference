namespace FilmReferenceBlazor.Tests;

public class PersonTests
{
    [Fact]
    public async Task CreatePersonAsync_ShouldAddPerson_WhenSaveChangesTrue()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new PersonHandler(context);

        var person = new PersonModel { PersonId = 1, FirstName = "John", LastName = "Doe" };

        await handler.CreatePersonAsync(person, saveChanges: true);

        var result = await context.People.FindAsync(1);
        result.Should().NotBeNull();
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
    }

    [Fact]
    public async Task CreatePersonAsync_ShouldNotPersist_WhenSaveChangesFalse()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new PersonHandler(context);

        var person = new PersonModel { PersonId = 1, FirstName = "Jane", LastName = "Smith" };

        await handler.CreatePersonAsync(person, saveChanges: false);

        // Entity is tracked but not committed
        var entry = context.Entry(person);
        entry.State.Should().Be(EntityState.Added);

    }

    [Fact]
    public async Task DeletePersonAsync_ShouldRemovePerson_WhenExistsAndSaveChangesTrue()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var person = new PersonModel { PersonId = 1, FirstName = "Alice", LastName = "Brown" };
        context.People.Add(person);
        context.SaveChanges();

        var handler = new PersonHandler(context);

        await handler.DeletePersonAsync(1, saveChanges: true);

        var result = await context.People.FindAsync(1);
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeletePersonAsync_ShouldDoNothing_WhenPersonDoesNotExist()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new PersonHandler(context);

        await handler.DeletePersonAsync(99, saveChanges: true);

        context.People.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdatePersonAsync_ShouldModifyPerson_WhenExistsAndSaveChangesTrue()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var person = new PersonModel { PersonId = 1, FirstName = "Bob", LastName = "Marley" };
        context.People.Add(person);
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var updated = new PersonModel { PersonId = 1, FirstName = "Robert", LastName = "Marley" };
        await handler.UpdatePersonAsync(updated, saveChanges: true);

        var result = await context.People.FindAsync(1);
        result?.FirstName.Should().Be("Robert");
        result?.LastName.Should().Be("Marley");
    }

    [Fact]
    public async Task UpdatePersonAsync_ShouldDoNothing_WhenPersonDoesNotExist()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new PersonHandler(context);

        var updated = new PersonModel { PersonId = 99, FirstName = "Ghost", LastName = "User" };
        await handler.UpdatePersonAsync(updated, saveChanges: true);

        context.People.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCastMembersAsync_ShouldReturnOrderedList()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        context.People.AddRange(
            new PersonModel { PersonId = 1, FirstName = "Charlie", LastName = "Zulu", IsCastMember = true, },
            new PersonModel { PersonId = 2, FirstName = "Alice", LastName = "Alpha", IsCastMember = true, }
        );
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var result = await handler.GetCastMembersAsync();

        result.Should().HaveCount(2);
        result.First().FirstName.Should().Be("Alice"); // ordered by FirstName then LastName
    }

    [Fact]
    public async Task GetDirectorsAsync_ShouldReturnOrderedList()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        context.People.AddRange(
            new PersonModel { PersonId = 1, FirstName = "Charlie", LastName = "Zulu", IsDirector = true, },
            new PersonModel { PersonId = 2, FirstName = "Alice", LastName = "Alpha", IsDirector = true, }
        );
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var result = await handler.GetDirectorsAsync();

        result.Should().HaveCount(2);
        result.First().FirstName.Should().Be("Alice"); // ordered by FirstName then LastName
    }

    [Fact]
    public async Task GetDirectorsAsync_ShouldIncludePeopleWhoAreBothDirectorAndCastMember()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        context.People.AddRange(
            new PersonModel { FirstName = "Greta", LastName = "Gerwig", IsDirector = true, IsCastMember = true },
            new PersonModel { FirstName = "Ridley", LastName = "Scott", IsDirector = true, IsCastMember = false }
        );
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var result = await handler.GetDirectorsAsync();

        result.Should().HaveCount(2);
        result.Should().Contain(p => p.FirstName == "Greta");
        result.Should().Contain(p => p.FirstName == "Ridley");
    }

    [Fact]
    public async Task GetCastMembersAsync_ShouldIncludePeopleWhoAreBothDirectorAndCastMember()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        context.People.AddRange(
            new PersonModel { FirstName = "Greta", LastName = "Gerwig", IsDirector = true, IsCastMember = true },
            new PersonModel { FirstName = "Ridley", LastName = "Scott", IsDirector = false, IsCastMember = true }
        );
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var result = await handler.GetCastMembersAsync();

        result.Should().HaveCount(2);
        result.Should().Contain(p => p.FirstName == "Greta");
        result.Should().Contain(p => p.FirstName == "Ridley");
    }

    [Fact]
    public async Task GetPersonAsync_ShouldReturnPersonWithFilms_WhenExists()
    {
        using var context = DbContextHelper.GetInMemoryContext();

        var genre = new GenreModel { Name = "Drama" };
        var studio = new StudioModel { Name = "A24" };
        var film = new FilmModel { Name = "Test Film", Genre = genre, Studio = studio };
        var person = new PersonModel { FirstName = "David", LastName = "Jones" };

        var fp = new FilmPersonModel { Film = film, Person = person };

        context.AddRange(genre, studio, film, person, fp);
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var result = await handler.GetPersonAsync(person.PersonId);

        result.Should().NotBeNull();
        result.PersonId.Should().Be(person.PersonId);
        result.FirstName.Should().Be("David");
        result.LastName.Should().Be("Jones");

        result.FilmPerson.Should().NotBeNull();
        result.FilmPerson.Should().HaveCount(1);
        result.FilmPerson.Single().Film.Name.Should().Be("Test Film");
        result.FilmPerson.Single().Film.Genre.Name.Should().Be("Drama");
        result.FilmPerson.Single().Film.Studio.Name.Should().Be("A24");
    }

    [Fact]
    public async Task GetPersonAsync_ShouldReturnPersonWithMultipleFilms_OrderedByName()
    {
        using var context = DbContextHelper.GetInMemoryContext();

        var genre1 = new GenreModel { Name = "Drama" };
        var genre2 = new GenreModel { Name = "Comedy" };
        var studio1 = new StudioModel { Name = "A24" };
        var studio2 = new StudioModel { Name = "Universal" };

        var filmA = new FilmModel { Name = "Alpha Film", Genre = genre1, Studio = studio1 };
        var filmB = new FilmModel { Name = "Beta Film", Genre = genre2, Studio = studio2 };

        var person = new PersonModel { FirstName = "David", LastName = "Jones" };

        var fp1 = new FilmPersonModel { Film = filmA, Person = person };
        var fp2 = new FilmPersonModel { Film = filmB, Person = person };

        context.AddRange(genre1, genre2, studio1, studio2, filmA, filmB, person, fp1, fp2);
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var result = await handler.GetPersonAsync(person.PersonId);

        result.Should().NotBeNull();
        result.PersonId.Should().Be(person.PersonId);
        result.FirstName.Should().Be("David");
        result.LastName.Should().Be("Jones");

        result.FilmPerson.Should().NotBeNull();
        result.FilmPerson.Should().HaveCount(2);

        // Verify ordering by Film.Name
        var orderedFilms = result.FilmPerson.Select(fp => fp.Film.Name).ToList();
        orderedFilms.Should().ContainInOrder("Alpha Film", "Beta Film");

        // Verify details of each film
        result.FilmPerson.First().Film.Genre.Name.Should().Be("Drama");
        result.FilmPerson.First().Film.Studio.Name.Should().Be("A24");
        result.FilmPerson.Last().Film.Genre.Name.Should().Be("Comedy");
        result.FilmPerson.Last().Film.Studio.Name.Should().Be("Universal");
    }

    [Fact]
    public async Task GetPersonAsync_ShouldReturnEmptyPerson_WhenNotFound()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new PersonHandler(context);

        var result = await handler.GetPersonAsync(99);

        result.Should().NotBeNull();
        result.PersonId.Should().Be(0); // default new PersonModel
    }
}
