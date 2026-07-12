namespace FilmReferenceUI.Components.Pages.Films;

public partial class DeleteFilm
{
    protected override async Task OnInitializedAsync()
    {
        FilmModel = await FilmHandler.GetFilmAsync(FilmId);

        if (FilmModel.FilmId == 0)
        {
            MainLayout.SetHeaderValue(FilmNotFoundMessage);
            return;
        }
            
        MainLayout.SetHeaderValue($"Delete {FilmModel.Name}");

        _isLoaded = true;
    }

    private async Task DeleteFilmAsync()
    {
        try
        {
            await FilmHandler.DeleteFilmAsync(FilmId);
            Snackbar.Add($"{FilmModel.Name} successfully deleted.", Severity.Success);
            NavigationManager.NavigateTo("films/listfilms");
        }
        catch
        {
            Snackbar.Add($"An error occurred deleting {FilmModel.Name}. Please try again.", Severity.Error);
        }
    }
}
