namespace FilmReferenceTests;

public class PersonTests
{
    // ----------------------------------------------------------------------
    // CREATE
    // ----------------------------------------------------------------------

    [Fact]
    public async Task CreatePersonAsync_ShouldAddPerson()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new PersonHandler(factory);

        var person = new PersonModel { PersonId = 1, FirstName = "John", LastName = "Doe" };

        await handler.CreatePersonAsync(person);

        var result = await context.People.FindAsync(1);
        result.Should().NotBeNull();
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
    }

    [Fact]
    public async Task CreatePersonAsync_ShouldPersistDobAndDod()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new PersonHandler(factory);

        var person = new PersonModel
        {
            PersonId = 1,
            FirstName = "Test",
            LastName = "Person",
            DateOfBirth = new DateTime(1975, 5, 20),
            DateOfDeath = new DateTime(2020, 1, 1)
        };

        await handler.CreatePersonAsync(person);

        var result = await context.People.FindAsync(1);

        result.Should().NotBeNull();
        result.DateOfBirth.Should().Be(new DateTime(1975, 5, 20));
        result.DateOfDeath.Should().Be(new DateTime(2020, 1, 1));
    }

    // ----------------------------------------------------------------------
    // DELETE
    // ----------------------------------------------------------------------

    [Fact]
    public async Task DeletePersonAsync_ShouldRemovePerson_WhenExists()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        var person = new PersonModel { PersonId = 1, FirstName = "Alice", LastName = "Brown" };
        context.People.Add(person);
        await context.SaveChangesAsync();

        var handler = new PersonHandler(factory);

        await handler.DeletePersonAsync(1);

        context.ChangeTracker.Clear();

        var result = await context.People.FindAsync(1);
        result.Should().BeNull();
    }

    [Fact]
    public async Task DeletePersonAsync_ShouldDoNothing_WhenPersonDoesNotExist()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new PersonHandler(factory);

        await handler.DeletePersonAsync(99);

        context.People.Should().BeEmpty();
    }

    // ----------------------------------------------------------------------
    // UPDATE
    // ----------------------------------------------------------------------

    [Fact]
    public async Task UpdatePersonAsync_ShouldModifyPerson_WhenExists()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        var person = new PersonModel { PersonId = 1, FirstName = "Bob", LastName = "Marley" };
        context.People.Add(person);
        await context.SaveChangesAsync();

        var handler = new PersonHandler(factory);

        var updated = new PersonModel { PersonId = 1, FirstName = "Robert", LastName = "Marley" };
        await handler.UpdatePersonAsync(updated);

        context.ChangeTracker.Clear();

        var result = await context.People.FindAsync(1);
        result?.FirstName.Should().Be("Robert");
        result?.LastName.Should().Be("Marley");
    }

    [Fact]
    public async Task UpdatePersonAsync_ShouldDoNothing_WhenPersonDoesNotExist()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new PersonHandler(factory);

        var updated = new PersonModel { PersonId = 99, FirstName = "Ghost", LastName = "User" };
        await handler.UpdatePersonAsync(updated);

        context.People.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdatePersonAsync_ShouldUpdateDobAndDod()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        var person = new PersonModel
        {
            PersonId = 1,
            FirstName = "Old",
            LastName = "Name",
            DateOfBirth = new DateTime(1970, 1, 1),
            DateOfDeath = null
        };

        context.People.Add(person);
        await context.SaveChangesAsync();

        context.ChangeTracker.Clear();

        var handler = new PersonHandler(factory);

        var updated = new PersonModel
        {
            PersonId = 1,
            FirstName = "Old",
            LastName = "Name",
            DateOfBirth = new DateTime(1980, 2, 2),
            DateOfDeath = new DateTime(2020, 3, 3)
        };

        await handler.UpdatePersonAsync(updated);

        var result = await context.People.FindAsync(1);

        result.Should().NotBeNull();
        result.DateOfBirth.Should().Be(new DateTime(1980, 2, 2));
        result.DateOfDeath.Should().Be(new DateTime(2020, 3, 3));
    }

    // ----------------------------------------------------------------------
    // GET CAST MEMBERS
    // ----------------------------------------------------------------------

    [Fact]
    public async Task GetCastMembersAsync_ShouldReturnOrderedList()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        context.People.AddRange(
            new PersonModel { PersonId = 1, FirstName = "Alice", LastName = "Zulu", IsCastMember = true },
            new PersonModel { PersonId = 2, FirstName = "Alice", LastName = "Alpha", IsCastMember = true }
        );
        await context.SaveChangesAsync();

        var handler = new PersonHandler(factory);

        var result = await handler.GetCastMembersAsync();

        result.Should().HaveCount(2);
        result.First().LastName.Should().Be("Alpha");
        result.Last().LastName.Should().Be("Zulu");
    }

    [Fact]
    public async Task GetCastMembersAsync_ShouldIncludePeopleWhoAreBothDirectorAndCastMember()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        context.People.AddRange(
            new PersonModel { FirstName = "Greta", LastName = "Gerwig", IsDirector = true, IsCastMember = true },
            new PersonModel { FirstName = "Gordon", LastName = "Scott", IsDirector = false, IsCastMember = true }
        );
        await context.SaveChangesAsync();

        var handler = new PersonHandler(factory);

        var result = await handler.GetCastMembersAsync();

        result.Should().HaveCount(2);
        result.Should().Contain(p => p.FirstName == "Greta");
        result.Should().Contain(p => p.FirstName == "Gordon");
    }

    [Fact]
    public async Task GetCastMembersAsync_ShouldExcludeDirectors()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        context.People.AddRange(
            new PersonModel { FirstName = "Cast", LastName = "Only", IsCastMember = true, IsDirector = false },
            new PersonModel { FirstName = "Director", LastName = "Only", IsCastMember = false, IsDirector = true }
        );
        await context.SaveChangesAsync();

        var handler = new PersonHandler(factory);
        var result = await handler.GetCastMembersAsync();

        result.Should().HaveCount(1);
        result.Single().FirstName.Should().Be("Cast");
    }

    // ----------------------------------------------------------------------
    // GET DIRECTORS
    // ----------------------------------------------------------------------

    [Fact]
    public async Task GetDirectorsAsync_ShouldReturnOrderedList()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        context.People.AddRange(
            new PersonModel { PersonId = 1, FirstName = "Alice", LastName = "Zulu", IsDirector = true },
            new PersonModel { PersonId = 2, FirstName = "Alice", LastName = "Alpha", IsDirector = true }
        );
        await context.SaveChangesAsync();

        var handler = new PersonHandler(factory);
        var result = await handler.GetDirectorsAsync();

        result.Should().HaveCount(2);
        result.First().LastName.Should().Be("Alpha");
        result.Last().LastName.Should().Be("Zulu");
    }

    [Fact]
    public async Task GetDirectorsAsync_ShouldIncludePeopleWhoAreBothDirectorAndCastMember()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        context.People.AddRange(
            new PersonModel { FirstName = "Greta", LastName = "Gerwig", IsDirector = true, IsCastMember = true },
            new PersonModel { FirstName = "Gordon", LastName = "Scott", IsDirector = true, IsCastMember = false }
        );
        await context.SaveChangesAsync();

        var handler = new PersonHandler(factory);
        var result = await handler.GetDirectorsAsync();

        result.Should().HaveCount(2);
        result.Should().Contain(p => p.FirstName == "Greta");
        result.Should().Contain(p => p.FirstName == "Gordon");
    }

    [Fact]
    public async Task GetDirectorsAsync_ShouldReturnEmptyList_WhenNoDirectorsExist()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        context.People.AddRange(
            new PersonModel { FirstName = "Alice", LastName = "Smith", IsCastMember = true },
            new PersonModel { FirstName = "Bob", LastName = "Jones", IsCastMember = true }
        );
        await context.SaveChangesAsync();

        var handler = new PersonHandler(factory);
        var result = await handler.GetDirectorsAsync();

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetDirectorsAsync_ShouldExcludeNonDirectors()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        context.People.AddRange(
            new PersonModel { FirstName = "Director", LastName = "One", IsDirector = true },
            new PersonModel { FirstName = "Actor", LastName = "Two", IsDirector = false }
        );
        await context.SaveChangesAsync();

        var handler = new PersonHandler(factory);
        var result = await handler.GetDirectorsAsync();

        result.Should().HaveCount(1);
        result.Single().FirstName.Should().Be("Director");
    }

    // ----------------------------------------------------------------------
    // GET PERSON (single)
    // ----------------------------------------------------------------------

    [Fact]
    public async Task GetPersonAsync_ShouldReturnPersonWithFilms_WhenExists()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        var genre = new GenreModel { Name = "Drama" };
        var studio = new StudioModel { Name = "A24" };
        var film = new FilmModel { Name = "Test Film", Genre = genre, Studio = studio };
        var person = new PersonModel { FirstName = "David", LastName = "Jones" };

        var fp = new FilmPersonModel { Film = film, Person = person };

        context.AddRange(genre, studio, film, person, fp);
        await context.SaveChangesAsync();

        var handler = new PersonHandler(factory);

        var result = await handler.GetPersonAsync(person.PersonId);

        result.Should().NotBeNull();
        result.PersonId.Should().Be(person.PersonId);
        result.FilmPerson.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetPersonAsync_ShouldReturnPersonWithMultipleFilms_OrderedByName()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

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
        await context.SaveChangesAsync();

        var handler = new PersonHandler(factory);

        var result = await handler.GetPersonAsync(person.PersonId);

        result.FilmPerson.Should().HaveCount(2);

        var ordered = result.FilmPerson.Select(fp => fp.Film.Name).ToList();
        ordered.Should().ContainInOrder("Alpha Film", "Beta Film");
    }

    [Fact]
    public async Task GetPersonAsync_ShouldReturnEmptyPerson_WhenNotFound()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new PersonHandler(factory);

        var result = await handler.GetPersonAsync(99);

        result.Should().NotBeNull();
        result.PersonId.Should().Be(0);
    }

    [Fact]
    public async Task GetPersonAsync_ShouldReturnDobAndDod()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        var person = new PersonModel
        {
            FirstName = "Test",
            LastName = "Person",
            DateOfBirth = new DateTime(1990, 10, 10),
            DateOfDeath = new DateTime(2020, 10, 10)
        };

        context.People.Add(person);
        await context.SaveChangesAsync();

        var handler = new PersonHandler(factory);
        var result = await handler.GetPersonAsync(person.PersonId);

        result.DateOfBirth.Should().Be(new DateTime(1990, 10, 10));
        result.DateOfDeath.Should().Be(new DateTime(2020, 10, 10));
    }

    // ----------------------------------------------------------------------
    // RANDOM PERSON
    // ----------------------------------------------------------------------

    [Fact]
    public async Task GetRandomPersonAsync_ShouldReturnNewPersonModel_WhenNoCastMembersExist()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new PersonHandler(factory);

        var result = await handler.GetRandomPersonAsync();

        result.PersonId.Should().Be(0);
    }

    [Fact]
    public async Task GetRandomPersonAsync_ShouldReturnNewPersonModel_WhenCastMembersHaveNoPictures()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        context.People.Add(new PersonModel
        {
            FirstName = "NoPic",
            LastName = "Person",
            IsCastMember = true,
            Picture = null
        });
        await context.SaveChangesAsync();

        var handler = new PersonHandler(factory);
        var result = await handler.GetRandomPersonAsync();

        result.PersonId.Should().Be(0);
    }

    [Fact]
    public async Task GetRandomPersonAsync_ShouldReturnCastMemberWithPicture()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        var pic = new byte[] { 1, 2, 3 };

        context.People.AddRange(
            new PersonModel { PersonId = 1, FirstName = "A", LastName = "One", IsCastMember = true, Picture = pic },
            new PersonModel { PersonId = 2, FirstName = "B", LastName = "Two", IsCastMember = true, Picture = pic }
        );
        await context.SaveChangesAsync();

        var handler = new PersonHandler(factory);
        var result = await handler.GetRandomPersonAsync();

        result.Picture.Should().NotBeNull();
        result.PersonId.Should().BeOneOf(1, 2);
    }

    [Fact]
    public async Task GetRandomPersonAsync_ShouldExcludeNonCastMembersAndPicturelessPeople()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        var pic = new byte[] { 9, 9, 9 };

        context.People.AddRange(
            new PersonModel { PersonId = 1, FirstName = "Cast", LastName = "WithPic", IsCastMember = true, Picture = pic },
            new PersonModel { PersonId = 2, FirstName = "Director", LastName = "Only", IsDirector = true, Picture = pic },
            new PersonModel { PersonId = 3, FirstName = "Cast", LastName = "NoPic", IsCastMember = true, Picture = null }
        );
        await context.SaveChangesAsync();

        var handler = new PersonHandler(factory);
        var result = await handler.GetRandomPersonAsync();

        result.PersonId.Should().Be(1);
    }

    // ----------------------------------------------------------------------
    // AGE CALCULATION
    // ----------------------------------------------------------------------

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

    // ----------------------------------------------------------------------
    // LIGHTWEIGHT CAST MEMBERS
    // ----------------------------------------------------------------------

    [Fact]
    public async Task GetCastMembersLightweightAsync_ShouldReturnOnlyCastMembers()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new PersonHandler(factory);

        context.People.AddRange(
            new PersonModel { PersonId = 1, FirstName = "Tom", LastName = "Hardy", IsCastMember = true },
            new PersonModel { PersonId = 2, FirstName = "Ridley", LastName = "Scott", IsCastMember = false }
        );
        await context.SaveChangesAsync();

        var result = await handler.GetCastMembersLightweightAsync();

        result.Should().HaveCount(1);

        var cast = result.First();
        cast.PersonId.Should().Be(1);
        cast.FirstName.Should().Be("Tom");
        cast.LastName.Should().Be("Hardy");
    }

    // ----------------------------------------------------------------------
    // LIGHTWEIGHT DIRECTORS
    // ----------------------------------------------------------------------

    [Fact]
    public async Task GetDirectorsLightweightAsync_ShouldReturnOnlyDirectors()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();
        var handler = new PersonHandler(factory);

        context.People.AddRange(
            new PersonModel { PersonId = 1, FirstName = "Tom", LastName = "Hardy", IsDirector = false },
            new PersonModel { PersonId = 2, FirstName = "Ridley", LastName = "Scott", IsDirector = true }
        );
        await context.SaveChangesAsync();

        var result = await handler.GetDirectorsLightweightAsync();

        result.Should().HaveCount(1);

        var director = result.First();
        director.PersonId.Should().Be(2);
        director.FirstName.Should().Be("Ridley");
        director.LastName.Should().Be("Scott");
    }

    [Fact]
    public async Task DeletePerson_AlsoDeletesFavourite_WhenPersonIsFavourite()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync();

        context.People.Add(new PersonModel { PersonId = 300, FirstName = "John" });
        context.Favourites.Add(new FavouriteModel { EntityId = 300, EntityTypeId = (int)FavouriteEntityEnum.Person });
        context.Favourites.Add(new FavouriteModel { EntityId = 999, EntityTypeId = (int)FavouriteEntityEnum.Person });

        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();

        var handler = new PersonHandler(factory);

        await handler.DeletePersonAsync(300);

        await using var verify = await factory.CreateDbContextAsync();

        verify.People.Should().BeEmpty();
        verify.Favourites.Where(f => f.EntityId == 300 && f.EntityTypeId == (int)FavouriteEntityEnum.Person).Should().BeEmpty();
        verify.Favourites.Where(f => f.EntityId == 999 && f.EntityTypeId == (int)FavouriteEntityEnum.Person).Should().HaveCount(1);
    }
}