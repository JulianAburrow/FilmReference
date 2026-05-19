namespace FilmReferenceTests.Helpers;

public static class TestDataFactory
{
    public static GenreModel CreateGenre(string name = "Drama") =>
        new()
        { Name = name };

    public static StudioModel CreateStudio(string name = "Universal") =>
        new()
        { Name = name };

    public static PersonModel CreateDirector(string name = "Director") =>
        new()
        { FirstName = name };

    public static FilmModel CreateFilm(
        string name,
        StudioModel studio,
        GenreModel genre,
        PersonModel director) =>
        new()
        {
            Name = name,
            Studio = studio,
            Genre = genre,
            Director = director
        };
}
