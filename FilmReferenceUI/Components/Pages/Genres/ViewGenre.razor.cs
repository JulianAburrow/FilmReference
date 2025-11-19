namespace FilmReferenceUI.Components.Pages.Genres;

public partial class ViewGenre
{
    protected override async Task OnInitializedAsync()
    {
        GenreModel = await GenreHandler.GetGenreAsync(GenreId);
        PreventDeleting = GenreModel.Films.Any();
        MainLayout.SetHeaderValue("View Genre");
    }
}
