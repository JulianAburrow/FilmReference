namespace FilmReferenceBlazor.Tests.Helpers;

public static class DbContextHelper
{
    public static FilmReferenceContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<FilmReferenceContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) // unique DB per test
            .Options;
        return new FilmReferenceContext(options);
    }
}
