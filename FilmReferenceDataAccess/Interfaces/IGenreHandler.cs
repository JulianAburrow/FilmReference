namespace FilmReferenceDataAccess.Interfaces;

public interface IGenreHandler
{
    Task<List<GenreModel>> GetGenresAsync();

    Task<GenreModel> GetGenreAsync(int genreId);

    Task CreateGenreAsync(GenreModel genre, bool saveChanges);

    Task UpdateGenreAsync(GenreModel genre, bool saveChanges);

    Task DeleteGenreAsync(int genreId, bool saveChanges);

    Task SaveChangesAsync();
}
