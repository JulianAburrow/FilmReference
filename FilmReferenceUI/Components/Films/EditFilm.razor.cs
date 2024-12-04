namespace FilmReferenceUI.Components.Films;

public partial class EditFilm
{
    protected override async Task OnInitializedAsync()
    {
        FilmModel = await FilmHandler.GetFilmAsync(FilmId);
        GenreModels = await GenreHandler.GetGenresAsync();
        StudioModels = await StudioHandler.GetStudiosAsync();
        PersonModels = await PersonHandler.GetPeopleAsync();
        DirectorModels = PersonModels.Where(p => p.IsDirector).ToList();
        ActorModels = PersonModels.Where(p => p.IsActor).ToList();
        CopyModelToDisplayModel();
        MainLayout.SetHeaderValue("Edit Film");
    }

    private async void Update()
    {
        try
        {
            CopyDisplayModelToModel();
            await FilmHandler.UpdateFilmAsync(FilmModel, true);
            Snackbar.Add($"Film {FilmModel.Name} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("films/listfilms");
        }
        catch
        {
            Snackbar.Add($"An error occurred updating film {FilmModel.Name}. Please try again.", Severity.Error);
        }
    }
}
