namespace FilmReferenceUI.Components.Genres;

public partial class EditGenre
{
    protected override async Task OnInitializedAsync()
    {
        GenreModel = await GenreHandler.GetGenreAsync(GenreId);
        CopyModelToDisplayModel();
        MainLayout.SetHeaderValue("Edit Genre");
    }

    private async void Update()
    {
        try
        {
            CopyDisplayModelToModel();
            await GenreHandler.UpdateGenreAsync(GenreModel, true);
            Snackbar.Add($"Genre {GenreModel.Name} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("/genres/listgenres");
        }
        catch
        {
            Snackbar.Add($"An error occurred editing genre {GenreModel.Name}. Please try again.", Severity.Error);
        }
    }
}
