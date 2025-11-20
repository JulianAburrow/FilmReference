namespace FilmReferenceUI.Components.Pages.Films;

public partial class ViewFilm
{
    protected override async Task OnInitializedAsync()
    {
        FilmModel = await FilmHandler.GetFilmAsync(FilmId);
        MainLayout.SetHeaderValue("View Film");
    }
}
