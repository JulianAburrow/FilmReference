using FilmReferenceDataAccess.Enums;

namespace FilmReferenceUI.Components.Pages.Genres;

public partial class ViewGenre
{
    protected override async Task OnInitializedAsync()
    {
        GenreModel = await GenreHandler.GetGenreAsync(GenreId);
        PreventDeleting = GenreModel.Films.Any();
        MainLayout.SetHeaderValue(GenreModel.Name);
        var isFavourite = await FavouriteHandler.IsFavouriteAsync((int)FavouriteEntityEnum.Genre, GenreId);
        MainLayout.ConfigureFavouriteButton(FavouriteEntityEnum.Genre, GenreId, isFavourite);
    }
}
