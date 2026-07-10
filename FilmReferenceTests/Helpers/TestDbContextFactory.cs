namespace FilmReferenceTests.Helpers;

public class TestDbContextFactory : IDbContextFactory<FilmReferenceContext>
{
    private readonly DbContextOptions<FilmReferenceContext> _options;

    public TestDbContextFactory(DbContextOptions<FilmReferenceContext> options)
    {
        _options = options;
    }

    public FilmReferenceContext CreateDbContext()
    {
        return new FilmReferenceContext(_options);
    }
}
