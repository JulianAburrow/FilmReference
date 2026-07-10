namespace FilmReferenceUI.Components.Pages.Genres;

public partial class EditGenre
{
    protected override async Task OnInitializedAsync()
    {
        if (!RendererInfo.IsInteractive)
            return;

        GenreModel = await GenreHandler.GetGenreAsync(GenreId);
        CopyModelToDisplayModel();
        MainLayout.SetHeaderValue($"Edit {GenreModel.Name}");

        _isLoaded = true;
    }

    private async void Update()
    {
        try
        {
            CopyDisplayModelToModel();
            await GenreHandler.UpdateGenreAsync(GenreModel);
            Snackbar.Add($"{GenreModel.Name} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo($"/genre/view/{GenreModel.GenreId}");
        }
        catch
        {
            Snackbar.Add($"An error occurred editing {GenreModel.Name}. Please try again.", Severity.Error);
        }
    }
}
