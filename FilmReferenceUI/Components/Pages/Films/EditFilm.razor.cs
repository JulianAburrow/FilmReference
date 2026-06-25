namespace FilmReferenceUI.Components.Pages.Films;

public partial class EditFilm
{
    protected override async Task OnInitializedAsync()
    {
        FilmModel = await FilmHandler.GetFilmAsync(FilmId);
        GenreModels = await GenreHandler.GetGenresAsync();
        StudioModels = await StudioHandler.GetStudiosAsync();
        CastMemberModels = await PersonHandler.GetCastMembersAsync();
        DirectorModels = await PersonHandler.GetDirectorsAsync();
        CopyModelToDisplayModel();
        MainLayout.SetHeaderValue($"Edit Film {FilmModel.Name}");
    }

    private async Task UpdateFilmAsync()
    {
        try
        {
            await CopyDisplayModelToModelAsync();
            await FilmHandler.UpdateFilmAsync(FilmModel, FilmDisplayModel.SelectedCastMemberIds, true);
            Snackbar.Add($"{FilmModel.Name} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo($"/film/view/{FilmModel.FilmId}");
        }
        catch
        {
            Snackbar.Add($"An error occurred updating film {FilmModel.Name}. Please try again.", Severity.Error);
        }
    }
}
