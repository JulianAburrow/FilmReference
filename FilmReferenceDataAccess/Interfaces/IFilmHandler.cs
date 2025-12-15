namespace FilmReferenceDataAccess.Interfaces;

public interface IFilmHandler
{
    Task<List<FilmModel>> GetAllFilmsAsync();

    Task<FilmModel> GetFilmAsync(int filmId);

    Task CreateFilmAsync(FilmModel film, IEnumerable<int> selectedActressIds, bool saveChanges);

    Task UpdateFilmAsync(FilmModel film, IEnumerable<int> selectedActressIds, bool saveChanges);

    Task DeleteFilmAsync(int filmId, bool saveChanges);

    Task SaveChangesAsync();   
}
