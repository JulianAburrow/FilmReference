namespace FilmReferenceTests;

public class NationalityTests
{
    [Fact]
    public async Task GetNationalitiesAsync_ShouldReturnNationalitiesOrderedByName()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new NationalityHandler(factory);

        context.Nationalities.AddRange(
            new NationalityModel { Name = "Zanzibar" },
            new NationalityModel { Name = "Albania" }
        );
        await context.SaveChangesAsync(CT.Token);

        var result = await handler.GetNationalitiesAsync();

        result.Should().HaveCount(2);
        result.Select(n => n.Name).Should()
            .ContainInOrder("Albania", "Zanzibar");
    }

    [Fact]
    public async Task GetNationalitiesInUseForCastMembersAsync_ShouldReturnOnlyNationalitiesWithPeople()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new NationalityHandler(factory);

        var hungarian = new NationalityModel { NationalityId = 1, Name = "Hungarian" };
        var spanish = new NationalityModel { NationalityId = 2, Name = "Spanish" };
        var empty = new NationalityModel { NationalityId = 3, Name = "Martian" };

        context.Nationalities.AddRange(hungarian, spanish, empty);

        context.People.AddRange(
            new PersonModel { FirstName = "A", LastName = "B", NationalityId = 1, IsCastMember = true },
            new PersonModel { FirstName = "C", LastName = "D", NationalityId = 2, IsCastMember = true }
        );

        await context.SaveChangesAsync(CT.Token);

        var result = await handler.GetNationalitiesInUseForCastMembersAsync();

        result.Should().HaveCount(2);
        result.Select(n => n.Name).Should().Contain(new[] { "Hungarian", "Spanish" });
        result.Select(n => n.Name).Should().NotContain("Martian");
    }

    [Fact]
    public async Task GetNationalitiesInUseForCastMembersAsync_ShouldReturnCorrectPersonCounts()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new NationalityHandler(factory);

        var hungarian = new NationalityModel { NationalityId = 1, Name = "Hungarian" };
        var martian = new NationalityModel { NationalityId = 2, Name = "Martian" };
        context.Nationalities.AddRange(hungarian, martian);

        context.People.AddRange(
            new PersonModel { FirstName = "A", LastName = "B", NationalityId = 1, IsCastMember = true },
            new PersonModel { FirstName = "C", LastName = "D", NationalityId = 1, IsCastMember = true },
            new PersonModel { FirstName = "E", LastName = "F", NationalityId = 1, IsCastMember = true }
        );

        await context.SaveChangesAsync(CT.Token);

        var result = await handler.GetNationalitiesInUseForCastMembersAsync();

        result.Should().ContainSingle();
        result.Single().PersonCount.Should().Be(3);
    }

    [Fact]
    public async Task GetNationalitiesInUseForCastMembersAsync_ShouldReturnNationalitiesOrderedByName()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new NationalityHandler(factory);

        context.Nationalities.AddRange(
            new NationalityModel { NationalityId = 1, Name = "Zanzibar" },
            new NationalityModel { NationalityId = 2, Name = "Albania" }
        );

        context.People.AddRange(
            new PersonModel { FirstName = "A", LastName = "B", NationalityId = 1, IsCastMember = true },
            new PersonModel { FirstName = "C", LastName = "D", NationalityId = 2, IsCastMember = true }
        );

        await context.SaveChangesAsync(CT.Token);

        var result = await handler.GetNationalitiesInUseForCastMembersAsync();

        result.Select(n => n.Name).Should().ContainInOrder("Albania", "Zanzibar");
    }

    [Fact]
    public async Task GetNationalitiesInUseForCastMembersAsync_ShouldProjectCorrectFields()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new NationalityHandler(factory);

        var nat = new NationalityModel
        {
            NationalityId = 10,
            Name = "French",
            IsoCode = "FR"
        };

        context.Nationalities.Add(nat);
        context.People.Add(new PersonModel { FirstName = "A", LastName = "B", NationalityId = 10, IsCastMember = true });

        await context.SaveChangesAsync(CT.Token);

        var result = await handler.GetNationalitiesInUseForCastMembersAsync();
        var item = result.Single();

        item.NationalityId.Should().Be(10);
        item.Name.Should().Be("French");
        item.IsoCode.Should().Be("FR");
    }

    [Fact]
    public async Task GetNationalitiesInUseForDirectors_ShouldReturnOnlyNationalitiesWithDirectors()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new NationalityHandler(factory);

        var hungarian = new NationalityModel { NationalityId = 1, Name = "Hungarian" };
        var spanish = new NationalityModel { NationalityId = 2, Name = "Spanish" };
        var empty = new NationalityModel { NationalityId = 3, Name = "Martian" };

        context.Nationalities.AddRange(hungarian, spanish, empty);

        context.People.AddRange(
            new PersonModel { FirstName = "A", LastName = "B", NationalityId = 1, IsDirector = true },
            new PersonModel { FirstName = "C", LastName = "D", NationalityId = 2, IsDirector = true }
        );

        await context.SaveChangesAsync(CT.Token);

        var result = await handler.GetNationalitiesInUseForDirectorsAsync();

        result.Should().HaveCount(2);
        result.Select(n => n.Name).Should().Contain(new[] { "Hungarian", "Spanish" });
        result.Select(n => n.Name).Should().NotContain("Martian");
    }

    [Fact]
    public async Task GetNationalitiesInUseForDirectors_ShouldReturnCorrectPersonCounts()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new NationalityHandler(factory);

        var hungarian = new NationalityModel { NationalityId = 1, Name = "Hungarian" };
        var martian = new NationalityModel { NationalityId = 2, Name = "Martian" };

        context.Nationalities.AddRange(hungarian, martian);

        context.People.AddRange(
            new PersonModel { FirstName = "A", LastName = "B", NationalityId = 1, IsDirector = true },
            new PersonModel { FirstName = "C", LastName = "D", NationalityId = 1, IsDirector = true },
            new PersonModel { FirstName = "E", LastName = "F", NationalityId = 1, IsDirector = true }
        );

        await context.SaveChangesAsync(CT.Token);

        var result = await handler.GetNationalitiesInUseForDirectorsAsync();

        result.Should().ContainSingle();
        result.Single().PersonCount.Should().Be(3);
    }

    [Fact]
    public async Task GetNationalitiesInUseForDirectors_ShouldReturnNationalitiesOrderedByName()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new NationalityHandler(factory);

        context.Nationalities.AddRange(
            new NationalityModel { NationalityId = 1, Name = "Zanzibar" },
            new NationalityModel { NationalityId = 2, Name = "Albania" }
        );

        context.People.AddRange(
            new PersonModel { FirstName = "A", LastName = "B", NationalityId = 1, IsDirector = true },
            new PersonModel { FirstName = "C", LastName = "D", NationalityId = 2, IsDirector = true }
        );

        await context.SaveChangesAsync(CT.Token);

        var result = await handler.GetNationalitiesInUseForDirectorsAsync();

        result.Select(n => n.Name).Should().ContainInOrder("Albania", "Zanzibar");
    }

    [Fact]
    public async Task GetNationalitiesInUseForDirectors_ShouldProjectCorrectFields()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new NationalityHandler(factory);

        var nat = new NationalityModel
        {
            NationalityId = 10,
            Name = "French",
            IsoCode = "FR"
        };

        context.Nationalities.Add(nat);
        context.People.Add(new PersonModel { FirstName = "A", LastName = "B", NationalityId = 10, IsDirector = true });

        await context.SaveChangesAsync(CT.Token);

        var result = await handler.GetNationalitiesInUseForDirectorsAsync();
        var item = result.Single();

        item.NationalityId.Should().Be(10);
        item.Name.Should().Be("French");
        item.IsoCode.Should().Be("FR");
    }

    [Fact]
    public async Task GetNationalitiesInUseForCastMembersAsync_ShouldNotIncludeDirectorNationalities()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new NationalityHandler(factory);

        var castNat = new NationalityModel { NationalityId = 1, Name = "CastNation" };
        var directorNat = new NationalityModel { NationalityId = 2, Name = "DirectorNation" };

        context.Nationalities.AddRange(castNat, directorNat);

        context.People.AddRange(
            new PersonModel { FirstName = "A", LastName = "B", NationalityId = 1, IsCastMember = true },
            new PersonModel { FirstName = "C", LastName = "D", NationalityId = 2, IsDirector = true }
        );

        await context.SaveChangesAsync(CT.Token);

        var result = await handler.GetNationalitiesInUseForCastMembersAsync();

        result.Select(n => n.Name).Should().Contain("CastNation");
        result.Select(n => n.Name).Should().NotContain("DirectorNation");
    }

    [Fact]
    public async Task GetNationalitiesInUseForDirectorsAsync_ShouldNotIncludeCastMemberNationalities()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new NationalityHandler(factory);

        var directorNat = new NationalityModel { NationalityId = 1, Name = "DirectorNation" };
        var castNat = new NationalityModel { NationalityId = 2, Name = "CastNation" };

        context.Nationalities.AddRange(directorNat, castNat);

        context.People.AddRange(
            new PersonModel { FirstName = "A", LastName = "B", NationalityId = 1, IsDirector = true },
            new PersonModel { FirstName = "C", LastName = "D", NationalityId = 2, IsCastMember = true }
        );

        await context.SaveChangesAsync(CT.Token);

        var result = await handler.GetNationalitiesInUseForDirectorsAsync();

        result.Select(n => n.Name).Should().Contain("DirectorNation");
        result.Select(n => n.Name).Should().NotContain("CastNation");
    }
}
