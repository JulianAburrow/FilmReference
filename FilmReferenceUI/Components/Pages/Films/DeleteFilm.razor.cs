namespace FilmReferenceUI.Components.Pages.Films;

public partial class DeleteFilm
{
    protected override async Task OnInitializedAsync()
    {
        FilmModel = await FilmHandler.GetFilmAsync(FilmId);
        MainLayout.SetHeaderValue("Delete Film");
    }

    private async Task DeleteFilmAsync()
    {
        try
        {
            await FilmHandler.DeleteFilmAsync(FilmId, true);
            Snackbar.Add($"Film {FilmModel.Name} successfully deleted.", Severity.Success);
            NavigationManager.NavigateTo("films/listfilms");
        }
        catch
        {
            Snackbar.Add($"An error occurred deleting film {FilmModel.Name}. Please try again.", Severity.Error);
        }
    }
}
