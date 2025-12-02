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
        }
        else
        {
            FilteredFilmModels = FilmModels
                .Where(f =>
                    f.GenreId == genreId)
                .ToList();
        }

        var genreName = GenreModels.FirstOrDefault(g => g.GenreId == genreId)?.Name ?? "All";

        Snackbar.Add(
            $"{FilteredFilmModels.Count} {(FilteredFilmModels.Count == 1 ? "film" : "films")} found for filter '{genreName}'",
            FilteredFilmModels.Count > 0 ? Severity.Info : Severity.Warning);
    }

    private int GetFilmCountForGenre(int genreId)
    {
        return FilmModels.Count(f => f.GenreId == genreId);
    }
}
