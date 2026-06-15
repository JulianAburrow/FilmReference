namespace FilmReferenceUI.Components.Pages.Studios;

public partial class EditStudio
{
    protected override async Task OnInitializedAsync()
    {
        StudioModel = await StudioHandler.GetStudioAsync(StudioId);
        CopyModelToDisplayModel();
        MainLayout.SetHeaderValue($"Edit Studio {StudioModel.Name}");
    }

    private async void UpdateStudioAsync()
    {
        try
        {
            await CopyDisplayModelToModel();
            await StudioHandler.UpdateStudioAsync(StudioModel, true);
            Snackbar.Add($"Studio {StudioModel.Name} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("/studios/liststudios");
        }
        catch
        {
            Snackbar.Add($"And error occurred updating studio {StudioModel.Name}. Please try again.", Severity.Error);
        }
    }
}