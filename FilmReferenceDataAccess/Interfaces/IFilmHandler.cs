namespace FilmReferenceDataAccess.Interfaces;

public interface IFilmHandler
{
    Task<List<FilmModel>> GetFilmsAsync();

    Task<FilmModel> GetFilmModelAsync(int filmId);

    Task CreateFilmAsync(FilmModel film, bool saveChanges);

    Task UpdateFilmAsync(FilmModel film, bool saveChanges);

    Task DeleteFilmAsync(int filmId, bool saveChanges);

    Task SaveChangesAsync();   
}
