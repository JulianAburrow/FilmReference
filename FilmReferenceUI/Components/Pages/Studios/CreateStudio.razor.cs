namespace FilmReferenceUI.Components.Pages.Studios;

public partial class CreateStudio
{
    protected override async Task OnInitializedAsync() =>
        MainLayout.SetHeaderValue("Create Studio");

    private async Task CreateStudioAsync()
    {
        try
        {
            await CopyDisplayModelToModel();
            await StudioHandler.CreateStudioAsync(StudioModel, true);
            Snackbar.Add($"Studio {StudioModel.Name} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("/studios/liststudios");
        }
        catch
        {
            Snackbar.Add($"An error occurred creating studio {StudioModel.Name}. Please try again.", Severity.Error);
        }
    }
}
