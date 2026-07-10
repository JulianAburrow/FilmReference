namespace FilmReferenceUI.Components.Pages.Studios;

public partial class ViewStudio
{
    protected override async Task OnInitializedAsync()
    {
        if (!RendererInfo.IsInteractive)
            return;

        StudioModel = await StudioHandler.GetStudioAsync(StudioId);
        PreventDeleting = StudioModel.Films.Count != 0;
        MainLayout.SetHeaderValue(StudioModel.Name);
        var isFavourite = await FavouriteHandler.IsFavouriteAsync((int)FavouriteEntityEnum.Studio, StudioId);
        MainLayout.ConfigureFavouriteButton(FavouriteEntityEnum.Studio, StudioId, isFavourite);
        NextSortDirection = SortDirection.Descending;

        _isLoaded = true;
    }

    private void ResortList()
    {
        switch (NextSortDirection)
        {
            case SortDirection.Ascending:
                StudioModel.Films = [.. StudioModel.Films.OrderBy(f => f.Name)];
                NextSortDirection = SortDirection.Descending;
                break;
            case SortDirection.Descending:
                StudioModel.Films = [.. StudioModel.Films.OrderByDescending(f => f.Name)];
                NextSortDirection = SortDirection.Ascending;
                break;
            default:
                StudioModel.Films = [.. StudioModel.Films.OrderBy(f => f.Name)];
                NextSortDirection = SortDirection.Descending;
                break;
        }
    }
}
