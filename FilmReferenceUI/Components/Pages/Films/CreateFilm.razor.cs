namespace FilmReferenceUI.Components.Pages.Films;

public partial class CreateFilm
{
    protected override async Task OnInitializedAsync()
    {
        GenreModelsLightweight = await GenreHandler.GetGenresLightweightAsync();
        GenreModelsLightweight.Insert(0, new GenreModelLightweight
        {
            GenreId = SharedValues.PleaseSelectValue,
            Name = SharedValues.PleaseSelectText,
        });
        StudioModelsLightweight = await StudioHandler.GetStudiosLightweightAsync();
        StudioModelsLightweight.Insert(0, new StudioModelLightweight
        {
            StudioId = SharedValues.PleaseSelectValue,
            Name = SharedValues.PleaseSelectText,
        });
        CastMemberModelsLightweight = await PersonHandler.GetCastMembersLightweightAsync();
        DirectorModelsLightweight = await PersonHandler.GetDirectorsLightweightAsync();
        DirectorModelsLightweight.Insert(0, new PersonModelLightweight
        {
            PersonId = SharedValues.PleaseSelectValue,
            FirstName = SharedValues.PleaseSelectText,
        });
        MainLayout.SetHeaderValue("Create Film");
        FilmDisplayModel.GenreId = SharedValues.PleaseSelectValue;
        FilmDisplayModel.StudioId = SharedValues.PleaseSelectValue;
        FilmDisplayModel.DirectorId = SharedValues.PleaseSelectValue;

        _isLoaded = true;
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
