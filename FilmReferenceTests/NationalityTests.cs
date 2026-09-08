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
    public async Task GetNationalitiesInUseAsync_ShouldReturnOnlyNationalitiesWithPeople()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new NationalityHandler(factory);

        var hungarian = new NationalityModel { NationalityId = 1, Name = "Hungarian" };
        var spanish = new NationalityModel { NationalityId = 2, Name = "Spanish" };
        var empty = new NationalityModel { NationalityId = 3, Name = "Martian" };

        context.Nationalities.AddRange(hungarian, spanish, empty);

        context.People.AddRange(
            new PersonModel { FirstName = "A", LastName = "B", NationalityId = 1 },
            new PersonModel { FirstName = "C", LastName = "D", NationalityId = 2 }
        );

        await context.SaveChangesAsync(CT.Token);

        var result = await handler.GetNationalitiesInUseAsync();

        result.Should().HaveCount(2);
        result.Select(n => n.Name).Should().Contain(new[] { "Hungarian", "Spanish" });
        result.Select(n => n.Name).Should().NotContain("Martian");
    }

    [Fact]
    public async Task GetNationalitiesInUseAsync_ShouldReturnCorrectPersonCounts()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new NationalityHandler(factory);

        var hungarian = new NationalityModel { NationalityId = 1, Name = "Hungarian" };
        var martian = new NationalityModel { NationalityId = 2, Name = "Martian" };
        context.Nationalities.AddRange(hungarian, martian);

        context.People.AddRange(
            new PersonModel { FirstName = "A", LastName = "B", NationalityId = 1 },
            new PersonModel { FirstName = "C", LastName = "D", NationalityId = 1 },
            new PersonModel { FirstName = "E", LastName = "F", NationalityId = 1 }
        );

        await context.SaveChangesAsync(CT.Token);

        var result = await handler.GetNationalitiesInUseAsync();

        result.Should().ContainSingle();
        result.Single().PersonCount.Should().Be(3);
    }

    [Fact]
    public async Task GetNationalitiesInUseAsync_ShouldReturnNationalitiesOrderedByName()
    {
        var factory = DbContextHelper.GetInMemoryFactory();
        await using var context = await factory.CreateDbContextAsync(CT.Token);
        var handler = new NationalityHandler(factory);

        context.Nationalities.AddRange(
            new NationalityModel { NationalityId = 1, Name = "Zanzibar" },
            new NationalityModel { NationalityId = 2, Name = "Albania" }
        );

        context.People.AddRange(
            new PersonModel { FirstName = "A", LastName = "B", NationalityId = 1 },
            new PersonModel { FirstName = "C", LastName = "D", NationalityId = 2 }
        );

        await context.SaveChangesAsync(CT.Token);

        var result = await handler.GetNationalitiesInUseAsync();

        result.Select(n => n.Name).Should().ContainInOrder("Albania", "Zanzibar");
    }

    [Fact]
    public async Task GetNationalitiesInUseAsync_ShouldProjectCorrectFields()
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
        context.People.Add(new PersonModel { FirstName = "A", LastName = "B", NationalityId = 10 });

        await context.SaveChangesAsync(CT.Token);

        var result = await handler.GetNationalitiesInUseAsync();
        var item = result.Single();

        item.NationalityId.Should().Be(10);
        item.Name.Should().Be("French");
        item.IsoCode.Should().Be("FR");
    }
}
