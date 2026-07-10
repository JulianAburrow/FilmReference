namespace FilmReferenceUI.Components.Pages.Genres;

public partial class CreateGenre
{
    protected override async Task OnInitializedAsync() =>
        MainLayout.SetHeaderValue("Create Genre");

    private async Task CreateGenreAsync()
    {
        try
        {
            CopyDisplayModelToModel();
            await GenreHandler.CreateGenreAsync(GenreModel);
            Snackbar.Add($"{GenreModel.Name} successfully created.", Severity.Success);
            NavigationManager.NavigateTo($"/genre/view/{GenreModel.GenreId}");
        }
        catch
        {
            Snackbar.Add($"An error occurred creating Genre {GenreModel.Name}. Please try again.", Severity.Error);
        }
    }
    
}
