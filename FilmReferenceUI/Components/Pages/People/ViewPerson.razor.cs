using FilmReferenceDataAccess.Enums;

namespace FilmReferenceUI.Components.Pages.People;

public partial class ViewPerson
{
    private List<FilmModel> FilmsStarredIn { get; set; } = [];

    private List<FilmModel> FilmsDirected { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        PersonModel = await PersonHandler.GetPersonAsync(PersonId);
        PreventDeleting = PersonModel.FilmPerson.Count != 0 || PersonModel.Films.Count != 0;
        MainLayout.SetHeaderValue("View Person");
        var isFavourite = await FavouriteHandler.IsFavouriteAsync((int)FavouriteEntityEnum.Person, PersonId);
        MainLayout.ConfigureFavouriteButton(FavouriteEntityEnum.Person, PersonId, isFavourite);
        if (PersonModel.FilmPerson is null)
        {
            return;
        }
        foreach (var filmPerson in PersonModel.FilmPerson)
        {
            FilmsStarredIn.Add(filmPerson.Film);
        }
        FilmsDirected = PersonModel.Films
            .OrderBy(f => f.Name)
            .ToList();
    }
}
