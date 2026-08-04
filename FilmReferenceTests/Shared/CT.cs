namespace FilmReferenceTests.Shared;

public static class CT
{
    public static CancellationToken Token => TestContext.Current.CancellationToken;
}
