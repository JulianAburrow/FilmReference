namespace FilmReferenceTests.Helpers;

public static class DbContextHelper
{
    public static IDbContextFactory<FilmReferenceContext> GetInMemoryFactory()
    {
        var options = BuildOptions();
        return new TestDbContextFactory(options);
    }

    private static DbContextOptions<FilmReferenceContext> BuildOptions() =>
        new DbContextOptionsBuilder<FilmReferenceContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique DB per test
            .Options;
}
