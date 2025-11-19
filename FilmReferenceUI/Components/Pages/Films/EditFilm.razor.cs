namespace FilmReferenceUI.Components.Pages.Films;

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

    private async Task UpdateFilmAsync()
    {
        try
        {
            await CopyDisplayModelToModelAsync();
            await FilmHandler.UpdateFilmAsync(FilmModel, SelectedActors, true);
            Snackbar.Add($"Film {FilmModel.Name} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("films/listfilms");
        }
        catch
        {
            Snackbar.Add($"An error occurred updating film {FilmModel.Name}. Please try again.", Severity.Error);
        }
    }

    private string GetMultiSelectionText(List<string> selectedValues)
    {
        return $"{selectedValues.Count} actor{(selectedValues.Count > 1 ? "s have" : " has")} been selected";
    }

    private void RemoveBoxCoverAndImage()
    {
        FilmDisplayModel.BoxCover = null;
        Image = null;
        ImageName = null;
    }
}
