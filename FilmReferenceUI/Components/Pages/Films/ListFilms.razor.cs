namespace FilmReferenceUI.Components.Pages.Films;

public partial class ListFilms
{
    List<FilmModel> FilmModels { get; set; } = null!;

    private List<FilmModel> FilteredFilmModels { get; set; } = null!;

    private int SelectedGenreId = 0;

    protected override async Task OnInitializedAsync()
    {
        FilmModels = await FilmHandler.GetAllFilmsAsync();
        await FilterFilms(SelectedGenreId);
        GenreModels = await GenreHandler.GetGenresAsync();
        MainLayout.SetHeaderValue("Films");
    }

    private async Task FilterFilms(int genreId)
    {
        SelectedGenreId = genreId;
        if (genreId == 0)
        {
            FilteredFilmModels = FilmModels;
            return;
        }
        FilteredFilmModels = FilmModels
            .Where(f =>
                f.GenreId == genreId)
            .ToList();

        Snackbar.Add($"{FilteredFilmModels.Count} film(s) found for genre filter '{GenreModels.First(g => g.GenreId == genreId).Name}'", FilteredFilmModels.Count > 0 ? Severity.Info : Severity.Warning);
    }
}
