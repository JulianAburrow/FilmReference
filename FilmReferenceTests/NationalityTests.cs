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
}
