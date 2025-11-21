namespace FilmReferenceUI.Components.Pages.Films;

public partial class ViewFilm
{
    private List<PersonModel> Cast { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        FilmModel = await FilmHandler.GetFilmAsync(FilmId);
        MainLayout.SetHeaderValue("View Film");
        if (FilmModel.FilmPerson is null)
        {
            return;
        }
        foreach (var filmPerson in FilmModel.FilmPerson)
        {
            Cast.Add(filmPerson.Person);
        }
    }
}
