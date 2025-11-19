namespace FilmReferenceDataAccess.Interfaces;

public interface IFilmHandler
{
    Task<List<FilmModel>> GetAllFilmsAsync();

    Task<List<FilmModel>> GetFilmsByGenreAsync(int genreId);

    Task<FilmModel> GetFilmAsync(int filmId);

    Task CreateFilmAsync(FilmModel film, IEnumerable<int> selectedActorIds, bool saveChanges);

    Task UpdateFilmAsync(FilmModel film, IEnumerable<int> selectedActorIds, bool saveChanges);

    Task DeleteFilmAsync(int filmId, bool saveChanges);

    Task SaveChangesAsync();   
}
