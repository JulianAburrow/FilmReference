namespace FilmReferenceUI.Components.Pages.Studios;

public partial class DeleteStudio
{
    protected override async Task OnInitializedAsync()
    {
        StudioModel = await StudioHandler.GetStudioAsync(StudioId);
        PreventDeleting = StudioModel.Films.Count != 0;
        MainLayout.SetHeaderValue("Delete Studio");
    }

    private async Task DeleteStudioAsync()
    {
        try
        {
            await StudioHandler.DeleteStudioAsync(StudioId, true);
            Snackbar.Add($"Studio {StudioModel.Name} successfully deleted.", Severity.Success);
            NavigationManager.NavigateTo("studios/liststudios");
        }
        catch
        {
            Snackbar.Add($"An error occurred deleting studio {StudioModel.Name}. Please try again.", Severity.Error);
        }
    }
}
