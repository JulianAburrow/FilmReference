namespace FilmReferenceUI.Components.Genres;

public partial class CreateGenre
{
    protected override void OnInitialized() =>
        MainLayout.SetHeaderValue("Create Genre");

    private async Task Create()
    {
        try
        {
            CopyDisplayModelToModel();
            await GenreHandler.CreateGenreAsync(GenreModel, true);
            Snackbar.Add($"Genre {GenreModel.Name} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("/genres/listgenres");
        }
        catch
        {
            Snackbar.Add($"An error occurred creating Genre {GenreModel.Name}. Please try again.", Severity.Error);
        }
    }
    
}
