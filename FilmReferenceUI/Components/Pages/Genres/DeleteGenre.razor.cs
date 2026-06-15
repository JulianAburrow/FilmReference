namespace FilmReferenceUI.Components.Pages.Genres;

public partial class DeleteGenre
{
    protected override async Task OnInitializedAsync()
    {
        GenreModel = await GenreHandler.GetGenreAsync(GenreId);
        PreventDeleting = GenreModel.Films.Any();
        MainLayout.SetHeaderValue($"Delete Genre {GenreModel.Name}");
    }

    private async Task DeleteGenreAsync()
    {
        try
        {
            await GenreHandler.DeleteGenreAsync(GenreId, true);
            Snackbar.Add($"Genre {GenreModel.Name} successfully deleted", Severity.Success);
            NavigationManager.NavigateTo("genres/listgenres");
        }
        catch
        {
            Snackbar.Add($"An error occurred deleting genre {GenreModel.Name}. Please try again.", Severity.Error);
        }
        
    }
}
