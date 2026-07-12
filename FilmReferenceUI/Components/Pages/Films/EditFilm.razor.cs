namespace FilmReferenceUI.Components.Pages.Films;

public partial class EditFilm
{
    protected override async Task OnInitializedAsync()
    {
        FilmModel = await FilmHandler.GetFilmAsync(FilmId);

        if (FilmModel.FilmId == 0)
        {
            MainLayout.SetHeaderValue(FilmNotFoundMessage);
            return;
        }

        GenreModelsLightweight = await GenreHandler.GetGenresLightweightAsync();
        StudioModelsLightweight = await StudioHandler.GetStudiosLightweightAsync();
        CastMemberModelsLightweight = await PersonHandler.GetCastMembersLightweightAsync();
        DirectorModelsLightweight = await PersonHandler.GetDirectorsLightweightAsync();
        CopyModelToDisplayModel();
        MainLayout.SetHeaderValue($"Edit {FilmModel.Name}");

        _isLoaded = true;
    }

    private async Task UpdateFilmAsync()
    {
        try
        {
            await CopyDisplayModelToModelAsync();
            await FilmHandler.UpdateFilmAsync(FilmModel, FilmDisplayModel.SelectedCastMemberIds);
            Snackbar.Add($"{FilmModel.Name} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo($"/film/view/{FilmModel.FilmId}");
        }
        catch
        {
            Snackbar.Add($"An error occurred updating film {FilmModel.Name}. Please try again.", Severity.Error);
        }
    }
}
