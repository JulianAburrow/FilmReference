namespace FilmReferenceUI.Components.Studios;

public partial class ViewStudio
{
    protected override async Task OnInitializedAsync()
    {
        StudioModel = await StudioHandler.GetStudioAsync(StudioId);
        PreventDeleting = StudioModel.Films.Any();
        MainLayout.SetHeaderValue("View Studio");
    }
}
