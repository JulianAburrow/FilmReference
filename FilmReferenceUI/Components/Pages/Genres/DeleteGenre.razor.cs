namespace FilmReferenceUI.Components.Pages.Genres;

public partial class DeleteGenre
{
    protected override async Task OnInitializedAsync()
    {
        if (!RendererInfo.IsInteractive)
            return;

        GenreModel = await GenreHandler.GetGenreAsync(GenreId);
        PreventDeleting = GenreModel.Films.Any();
        MainLayout.SetHeaderValue($"Delete {GenreModel.Name}");

        _isLoaded = true;
    }

    private async Task DeleteGenreAsync()
    {
        try
        {
            await GenreHandler.DeleteGenreAsync(GenreId);
            Snackbar.Add($"{GenreModel.Name} successfully deleted", Severity.Success);
            NavigationManager.NavigateTo("genres/listgenres");
        }
        catch
        {
            Snackbar.Add($"An error occurred deleting {GenreModel.Name}. Please try again.", Severity.Error);
        }
        
    }
}
