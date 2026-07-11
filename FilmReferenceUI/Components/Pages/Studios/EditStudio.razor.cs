namespace FilmReferenceUI.Components.Pages.Studios;

public partial class EditStudio
{
    protected override async Task OnInitializedAsync()
    {
        StudioModel = await StudioHandler.GetStudioAsync(StudioId);
        CopyModelToDisplayModel();
        MainLayout.SetHeaderValue($"Edit {StudioModel.Name}");

        _isLoaded = true;
    }

    private async void UpdateStudioAsync()
    {
        try
        {
            await CopyDisplayModelToModel();
            await StudioHandler.UpdateStudioAsync(StudioModel);
            Snackbar.Add($"{StudioModel.Name} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo($"/studio/view/{StudioModel.StudioId}");
        }
        catch
        {
            Snackbar.Add($"And error occurred updating {StudioModel.Name}. Please try again.", Severity.Error);
        }
    }
}