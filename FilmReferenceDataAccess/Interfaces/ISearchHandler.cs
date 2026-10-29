namespace FilmReferenceDataAccess.Interfaces;

public interface ISearchHandler
{
    Task<List<GenreModel>> SearchGenresAsync(string searchText, bool includeDescriptionAndNotes);

    Task<List<FilmModel>> SearchFilmsAsync(string searchText, bool includeDescriptionAndNotes);

    Task<List<PersonModel>> SearchPeopleAsync(string searchText, bool includeDescriptionAndNotes);

    Task<List<StudioModel>> SearchStudiosAsync(string searchText, bool includeDescriptionAndNotes);
}
