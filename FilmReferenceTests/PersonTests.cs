namespace FilmReferenceTests;

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
    public async Task GetPeopleAsync_ShouldReturnOrderedList()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        context.People.AddRange(
            new PersonModel { PersonId = 1, FirstName = "Alice", LastName = "Zulu", IsCastMember = true, },
            new PersonModel { PersonId = 2, FirstName = "Alice", LastName = "Alpha", IsCastMember = true, }
        );
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var result = await handler.GetCastMembersAsync();

        result.Should().HaveCount(2);
        result.First().FirstName.Should().Be("Alice"); // ordered by FirstName then LastName
        result.Last().LastName.Should().Be("Zulu");
    }

    [Fact]
    public async Task GetPeopleAsync_ShouldIncludePeopleWhoAreBothDirectorAndCastMember()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        context.People.AddRange(
            new PersonModel { FirstName = "Greta", LastName = "Gerwig", IsDirector = true, IsCastMember = true },
            new PersonModel { FirstName = "Gordon", LastName = "Scott", IsDirector = false, IsCastMember = true }
        );
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var result = await handler.GetCastMembersAsync();

        result.Should().HaveCount(2);
        result.Should().Contain(p => p.FirstName == "Greta");
        result.Should().Contain(p => p.FirstName == "Gordon");
    }

    [Fact]
    public async Task GetCastMemberAsync_ShouldReturnPersonWithFilms_WhenExists()
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
    public async Task GetCastMemberAsync_ShouldReturnPersonWithMultipleFilms_OrderedByName()
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
    public async Task GetCastMemberAsync_ShouldReturnEmptyPerson_WhenNotFound()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new PersonHandler(context);

        var result = await handler.GetPersonAsync(99);

        result.Should().NotBeNull();
        result.PersonId.Should().Be(0); // default new PersonModel
    }

    [Fact]
    public async Task GetDirectorAsync_ShouldReturnPersonWithFilms_WhenExists()
    {
        using var context = DbContextHelper.GetInMemoryContext();

        var genre = new GenreModel { Name = "Drama" };
        var studio = new StudioModel { Name = "A24" };
        var film = new FilmModel { Name = "Test Film", Genre = genre, Studio = studio };
        var person = new PersonModel { FirstName = "David", LastName = "Jones", IsDirector = true };

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
    public async Task GetDirectorAsync_ShouldReturnPersonWithMultipleFilms_OrderedByName()
    {
        using var context = DbContextHelper.GetInMemoryContext();

        var genre1 = new GenreModel { Name = "Drama" };
        var genre2 = new GenreModel { Name = "Comedy" };
        var studio1 = new StudioModel { Name = "A24" };
        var studio2 = new StudioModel { Name = "Universal" };

        var filmA = new FilmModel { Name = "Alpha Film", Genre = genre1, Studio = studio1 };
        var filmB = new FilmModel { Name = "Beta Film", Genre = genre2, Studio = studio2 };

        var person = new PersonModel { FirstName = "David", LastName = "Jones", IsDirector = true };

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

        var orderedFilms = result.FilmPerson.Select(fp => fp.Film.Name).ToList();
        orderedFilms.Should().ContainInOrder("Alpha Film", "Beta Film");

        result.FilmPerson.First().Film.Genre.Name.Should().Be("Drama");
        result.FilmPerson.First().Film.Studio.Name.Should().Be("A24");
        result.FilmPerson.Last().Film.Genre.Name.Should().Be("Comedy");
        result.FilmPerson.Last().Film.Studio.Name.Should().Be("Universal");
    }

    [Fact]
    public async Task GetDirectorAsync_ShouldReturnEmptyPerson_WhenNotFound()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new PersonHandler(context);

        var result = await handler.GetPersonAsync(99);

        result.Should().NotBeNull();
        result.PersonId.Should().Be(0);
    }

    [Fact]
    public async Task GetDirectorsAsync_ShouldReturnOrderedList()
    {
        using var context = DbContextHelper.GetInMemoryContext();

        context.People.AddRange(
            new PersonModel { PersonId = 1, FirstName = "Alice", LastName = "Zulu", IsDirector = true },
            new PersonModel { PersonId = 2, FirstName = "Alice", LastName = "Alpha", IsDirector = true }
        );
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var result = await handler.GetDirectorsAsync();

        result.Should().HaveCount(2);
        result.First().LastName.Should().Be("Alpha");
        result.Last().LastName.Should().Be("Zulu");
    }

    [Fact]
    public async Task GetDirectorsAsync_ShouldIncludePeopleWhoAreBothDirectorAndCastMember()
    {
        using var context = DbContextHelper.GetInMemoryContext();

        context.People.AddRange(
            new PersonModel { FirstName = "Greta", LastName = "Gerwig", IsDirector = true, IsCastMember = true },
            new PersonModel { FirstName = "Gordon", LastName = "Scott", IsDirector = true, IsCastMember = false }
        );
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var result = await handler.GetDirectorsAsync();

        result.Should().HaveCount(2);
        result.Should().Contain(p => p.FirstName == "Greta");
        result.Should().Contain(p => p.FirstName == "Gordon");
    }

    [Fact]
    public async Task GetDirectorsAsync_ShouldReturnEmptyList_WhenNoDirectorsExist()
    {
        using var context = DbContextHelper.GetInMemoryContext();

        context.People.AddRange(
            new PersonModel { FirstName = "Alice", LastName = "Smith", IsCastMember = true },
            new PersonModel { FirstName = "Bob", LastName = "Jones", IsCastMember = true }
        );
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var result = await handler.GetDirectorsAsync();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetDirectorsAsync_ShouldExcludeNonDirectors()
    {
        using var context = DbContextHelper.GetInMemoryContext();

        context.People.AddRange(
            new PersonModel { FirstName = "Director", LastName = "One", IsDirector = true },
            new PersonModel { FirstName = "Actor", LastName = "Two", IsDirector = false }
        );
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var result = await handler.GetDirectorsAsync();

        result.Should().HaveCount(1);
        result.Single().FirstName.Should().Be("Director");
    }

    [Fact]
    public async Task GetCastMembersAsync_ShouldExcludeDirectors()
    {
        using var context = DbContextHelper.GetInMemoryContext();

        context.People.AddRange(
            new PersonModel { FirstName = "Cast", LastName = "Only", IsCastMember = true, IsDirector = false },
            new PersonModel { FirstName = "Director", LastName = "Only", IsCastMember = false, IsDirector = true }
        );
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var result = await handler.GetCastMembersAsync();

        result.Should().HaveCount(1);
        result.Single().FirstName.Should().Be("Cast");
    }

    [Fact]
    public async Task GetRandomPersonAsync_ShouldReturnNewPersonModel_WhenNoCastMembersExist()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new PersonHandler(context);

        var result = await handler.GetRandomPersonAsync();

        result.Should().NotBeNull();
        result.PersonId.Should().Be(0);
    }

    [Fact]
    public async Task GetRandomPersonAsync_ShouldReturnNewPersonModel_WhenCastMembersHaveNoPictures()
    {
        using var context = DbContextHelper.GetInMemoryContext();

        context.People.Add(new PersonModel
        {
            FirstName = "NoPic",
            LastName = "Person",
            IsCastMember = true,
            Picture = null
        });
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var result = await handler.GetRandomPersonAsync();

        result.Should().NotBeNull();
        result.PersonId.Should().Be(0);
    }

    [Fact]
    public async Task GetRandomPersonAsync_ShouldReturnCastMemberWithPicture()
    {
        using var context = DbContextHelper.GetInMemoryContext();

        var pic = new byte[] { 1, 2, 3 };

        context.People.AddRange(
            new PersonModel { PersonId = 1, FirstName = "A", LastName = "One", IsCastMember = true, Picture = pic },
            new PersonModel { PersonId = 2, FirstName = "B", LastName = "Two", IsCastMember = true, Picture = pic }
        );
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var result = await handler.GetRandomPersonAsync();

        result.Should().NotBeNull();
        result.Picture.Should().NotBeNull();
        result.PersonId.Should().BeOneOf(1, 2);
    }

    [Fact]
    public async Task GetRandomPersonAsync_ShouldExcludeNonCastMembersAndPicturelessPeople()
    {
        using var context = DbContextHelper.GetInMemoryContext();

        var pic = new byte[] { 9, 9, 9 };

        context.People.AddRange(
            new PersonModel { PersonId = 1, FirstName = "Cast", LastName = "WithPic", IsCastMember = true, Picture = pic },
            new PersonModel { PersonId = 2, FirstName = "Director", LastName = "Only", IsDirector = true, Picture = pic },
            new PersonModel { PersonId = 3, FirstName = "Cast", LastName = "NoPic", IsCastMember = true, Picture = null }
        );
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var result = await handler.GetRandomPersonAsync();

        result.Should().NotBeNull();
        result.PersonId.Should().Be(1); // the ONLY valid candidate
    }

    [Fact]
    public void Age_ShouldCalculateCorrectly_ForLivingPerson()
    {
        var person = new PersonModel
        {
            DateOfBirth = new DateTime(1980, 1, 1),
            DateOfDeath = null
        };

        var expected = DateTime.Now.Year - 1980;

        person.Age.Should().Be(expected);
    }

    [Fact]
    public void Age_ShouldCalculateCorrectly_ForDeceasedPerson()
    {
        var person = new PersonModel
        {
            DateOfBirth = new DateTime(1950, 1, 1),
            DateOfDeath = new DateTime(2000, 1, 1)
        };

        person.Age.Should().Be(50);
    }

    [Fact]
    public void Age_ShouldBeNull_WhenDateOfBirthIsNull()
    {
        var person = new PersonModel
        {
            DateOfBirth = null,
            DateOfDeath = null
        };

        person.Age.Should().BeNull();
    }

    [Fact]
    public async Task CreatePersonAsync_ShouldPersistDobAndDod()
    {
        using var context = DbContextHelper.GetInMemoryContext();
        var handler = new PersonHandler(context);

        var person = new PersonModel
        {
            PersonId = 1,
            FirstName = "Test",
            LastName = "Person",
            DateOfBirth = new DateTime(1975, 5, 20),
            DateOfDeath = new DateTime(2020, 1, 1)
        };

        await handler.CreatePersonAsync(person, saveChanges: true);

        var result = await context.People.FindAsync(1);

        result.Should().NotBeNull();
        result.DateOfBirth.Should().Be(new DateTime(1975, 5, 20));
        result.DateOfDeath.Should().Be(new DateTime(2020, 1, 1));
    }

    [Fact]
    public async Task UpdatePersonAsync_ShouldUpdateDobAndDod()
    {
        using var context = DbContextHelper.GetInMemoryContext();

        var person = new PersonModel
        {
            PersonId = 1,
            FirstName = "Old",
            LastName = "Name",
            DateOfBirth = new DateTime(1970, 1, 1),
            DateOfDeath = null
        };

        context.People.Add(person);
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var updated = new PersonModel
        {
            PersonId = 1,
            FirstName = "Old",
            LastName = "Name",
            DateOfBirth = new DateTime(1980, 2, 2),
            DateOfDeath = new DateTime(2020, 3, 3)
        };

        await handler.UpdatePersonAsync(updated, saveChanges: true);

        var result = await context.People.FindAsync(1);

        result.Should().NotBeNull();
        result.DateOfBirth.Should().Be(new DateTime(1980, 2, 2));
        result.DateOfDeath.Should().Be(new DateTime(2020, 3, 3));
    }

    [Fact]
    public async Task GetPersonAsync_ShouldReturnDobAndDod()
    {
        using var context = DbContextHelper.GetInMemoryContext();

        var person = new PersonModel
        {
            FirstName = "Test",
            LastName = "Person",
            DateOfBirth = new DateTime(1990, 10, 10),
            DateOfDeath = new DateTime(2020, 10, 10)
        };

        context.People.Add(person);
        context.SaveChanges();

        var handler = new PersonHandler(context);

        var result = await handler.GetPersonAsync(person.PersonId);

        result.DateOfBirth.Should().Be(new DateTime(1990, 10, 10));
        result.DateOfDeath.Should().Be(new DateTime(2020, 10, 10));
    }


}
