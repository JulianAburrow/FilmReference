namespace FilmReferenceDataAccess.Interfaces;

public interface IFilmHandler
{
    Task<List<FilmModel>> GetAllFilmsAsync();

    Task<FilmModel> GetFilmAsync(int filmId);

    Task CreateFilmAsync(FilmModel film, IEnumerable<int> selectedCastMemberIds);

    Task UpdateFilmAsync(FilmModel film, IEnumerable<int> selectedCastMemberIds);

    Task DeleteFilmAsync(int filmId);
}
