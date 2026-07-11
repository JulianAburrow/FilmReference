using FilmReferenceDataAccess.Enums;

namespace FilmReferenceUI.Components.Pages.Films;

public partial class ViewFilm
{
    private List<PersonModel> Cast { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        FilmModel = await FilmHandler.GetFilmAsync(FilmId);
        MainLayout.SetHeaderValue(FilmModel.Name);
        var isFavourite = await FavouriteHandler.IsFavouriteAsync((int)FavouriteEntityEnum.Film, FilmId);
        MainLayout.ConfigureFavouriteButton(FavouriteEntityEnum.Film, FilmId, isFavourite);
        if (FilmModel.FilmPerson is null)
        {
            return;
        }
        foreach (var filmPerson in FilmModel.FilmPerson)
        {
            Cast.Add(filmPerson.Person);
        }

        NextSortDirection = SortDirection.Descending;
        _isLoaded = true;
    }

    private void ResortList()
    {
        switch (NextSortDirection)
        {
            case SortDirection.Ascending:
                Cast = [.. Cast.OrderBy(c => c.FirstName).ThenBy(c => c.LastName)];
                NextSortDirection = SortDirection.Descending;
                break;
            case SortDirection.Descending:
                Cast = [.. Cast.OrderByDescending(c => c.FirstName).ThenByDescending(c => c.LastName)];
                NextSortDirection = SortDirection.Ascending;
                break;
            default:
                Cast = [.. Cast.OrderBy(c => c.FirstName).ThenBy(c => c.LastName)];
                NextSortDirection = SortDirection.Descending;
                break;
        }
    }
}
