namespace FilmReferenceUI.Components.Pages.Films;

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
        ActorModels = [.. PersonModels.Where(p => p.IsCastMember)];
        DirectorModels = [.. PersonModels.Where(p => p.IsDirector)];
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


    private async Task CreateFilmAsync()
    {
        try
        {
            await CopyDisplayModelToModelAsync();
            await FilmHandler.CreateFilmAsync(FilmModel, FilmDisplayModel.SelectedCastMemberIds, true);
            Snackbar.Add($"Film {FilmModel.Name} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("films/listfilms");
        }
        catch
        {
            Snackbar.Add($"An error occurred creating film {FilmModel.Name}. Please try again.", Severity.Error);
        }
    }
}
