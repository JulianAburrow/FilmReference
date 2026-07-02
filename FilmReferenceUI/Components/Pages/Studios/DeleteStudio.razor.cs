namespace FilmReferenceUI.Components.Pages.Studios;

public partial class DeleteStudio
{
    protected override async Task OnInitializedAsync()
    {
        StudioModel = await StudioHandler.GetStudioAsync(StudioId);
        PreventDeleting = StudioModel.Films.Count != 0;
        MainLayout.SetHeaderValue($"Delete {StudioModel.Name}");
    }

    private async Task DeleteStudioAsync()
    {
        try
        {
            await StudioHandler.DeleteStudioAsync(StudioId, true);
            Snackbar.Add($"{StudioModel.Name} successfully deleted.", Severity.Success);
            NavigationManager.NavigateTo("studios/liststudios");
        }
        catch
        {
            Snackbar.Add($"An error occurred deleting {StudioModel.Name}. Please try again.", Severity.Error);
        }
    }
}
