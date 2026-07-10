namespace FilmReferenceUI.Components.Pages.Genres;

public partial class ViewGenre
{
    protected override async Task OnInitializedAsync()
    {
        if (!RendererInfo.IsInteractive)
            return;

        GenreModel = await GenreHandler.GetGenreAsync(GenreId);
        PreventDeleting = GenreModel.Films.Any();
        MainLayout.SetHeaderValue(GenreModel.Name);
        var isFavourite = await FavouriteHandler.IsFavouriteAsync((int)FavouriteEntityEnum.Genre, GenreId);
        MainLayout.ConfigureFavouriteButton(FavouriteEntityEnum.Genre, GenreId, isFavourite);

        NextSortDirection = SortDirection.Descending;

        _isLoaded = true;
    }

    private void ResortList()
    {
        switch (NextSortDirection)
        {
            case SortDirection.Ascending:
                GenreModel.Films = [.. GenreModel.Films.OrderBy(f => f.Name)];
                NextSortDirection = SortDirection.Descending;
                break;
            case SortDirection.Descending:
                GenreModel.Films = [.. GenreModel.Films.OrderByDescending(f => f.Name)];
                NextSortDirection = SortDirection.Ascending;
                break;
            default:
                GenreModel.Films = [.. GenreModel.Films.OrderBy(f => f.Name)];
                NextSortDirection = SortDirection.Descending;
                break;
        }
    }
}
