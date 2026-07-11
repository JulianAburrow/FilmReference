namespace FilmReferenceDataAccess.Interfaces;

public interface IGenreHandler
{
    Task<List<GenreModel>> GetGenresAsync();

    Task<List<GenreModelLightweight>> GetGenresLightweightAsync();

    Task<GenreModel> GetGenreAsync(int genreId);

    Task CreateGenreAsync(GenreModel genre);

    Task UpdateGenreAsync(GenreModel genre);

    Task DeleteGenreAsync(int genreId);
}
