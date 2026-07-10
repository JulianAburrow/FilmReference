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
        CastMemberModels = await PersonHandler.GetCastMembersAsync();
        DirectorModels = await PersonHandler.GetDirectorsAsync();
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
            await FilmHandler.CreateFilmAsync(FilmModel, FilmDisplayModel.SelectedCastMemberIds);
            Snackbar.Add($"{FilmModel.Name} successfully created.", Severity.Success);
            NavigationManager.NavigateTo($"/film/view/{FilmModel.FilmId}");
        }
        catch
        {
            Snackbar.Add($"An error occurred creating {FilmModel.Name}. Please try again.", Severity.Error);
        }
    }
}
