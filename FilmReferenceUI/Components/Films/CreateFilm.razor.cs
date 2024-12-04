namespace FilmReferenceUI.Components.Films;

public partial class CreateFilm
{
    protected override async Task OnInitializedAsync()
    {
        GenreModels = await GenreHandler.GetGenresAsync();
        GenreModels.Insert(0, new GenreModel
        {
            GenreId = SharedValues.PleaseSelectValue,
            Name = SharedValues.PleaseSelectText,
        });
        StudioModels = await StudioHandler.GetStudiosAsync();
        StudioModels.Insert(0, new StudioModel
        {
            StudioId = SharedValues.PleaseSelectValue,
            Name = SharedValues.PleaseSelectText,
        });
        PersonModels = await PersonHandler.GetPeopleAsync();
        ActorModels = PersonModels
            .Where(p => p.IsActor)
            .ToList();
        DirectorModels = PersonModels
            .Where(p => p.IsDirector)
            .ToList();
        DirectorModels.Insert(0, new PersonModel
        {
            PersonId = SharedValues.PleaseSelectValue,
            FirstName = SharedValues.PleaseSelectText,
        });
        MainLayout.SetHeaderValue("Create Film");
        FilmDisplayModel.GenreId = SharedValues.PleaseSelectValue;
        FilmDisplayModel.StudioId = SharedValues.PleaseSelectValue;
        FilmDisplayModel.DirectorId = SharedValues.PleaseSelectValue;
    }


    private async void Create()
    {
        try
        {
            CopyDisplayModelToModel();                
            await FilmHandler.CreateFilmAsync(FilmModel, SelectedActors, true);
            Snackbar.Add($"Film {FilmModel.Name} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("films/listfilms");
        }
        catch (Exception ex)
        {
            Snackbar.Add($"An error occurred creating film {FilmModel.Name}. Please try again.", Severity.Error);
        }
    }

    private string GetMultiSelectionText(List<string> selectedValues)
    {
        return $"{selectedValues.Count} actor{(selectedValues.Count > 1 ? "s have" : " has")} been selected";
    }
}
