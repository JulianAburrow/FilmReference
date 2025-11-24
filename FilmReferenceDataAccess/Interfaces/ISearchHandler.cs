namespace FilmReferenceDataAccess.Interfaces;

public interface ISearchHandler
{
    Task<List<GenreModel>> SearchGenresAsync(string searchText);

    Task<List<FilmModel>> SearchFilmsAsync(string searchText);

    Task<List<PersonModel>> SearchPeopleAsync(string searchText);

    Task<List<StudioModel>> SearchStudiosAsync(string searchText);
}
