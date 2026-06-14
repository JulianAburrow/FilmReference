using FilmReferenceDataAccess.Enums;

namespace FilmReferenceUI.Components.Pages.Studios;

public partial class ViewStudio
{
    protected override async Task OnInitializedAsync()
    {
        StudioModel = await StudioHandler.GetStudioAsync(StudioId);
        PreventDeleting = StudioModel.Films.Count != 0;
        MainLayout.SetHeaderValue(StudioModel.Name);
        var isFavourite = await FavouriteHandler.IsFavouriteAsync((int)FavouriteEntityEnum.Studio, StudioId);
        MainLayout.ConfigureFavouriteButton(FavouriteEntityEnum.Studio, StudioId, isFavourite);
    }
}
