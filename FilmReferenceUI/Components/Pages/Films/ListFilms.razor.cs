namespace FilmReferenceUI.Components.Pages.Films;

public partial class ListFilms
{
    private List<FilmModel> AllFilmModels { get; set; } = null!;

    private List<FilmModel> FilteredFilmModels { get; set; } = null!;

    public string Genre { get; set;} = "All";
    
    private string Normalised(string value)
    => value.ToLower().Replace(" ", "");

    protected override async Task OnInitializedAsync()
    {
        AllFilmModels = await FilmHandler.GetAllFilmsAsync();
        GenreModels = await GenreHandler.GetGenresAsync();
        MainLayout.SetHeaderValue("Films");
        FilterFilms(Genre);
    }

    private void FilterFilms(string genreName)
    {
        Genre = genreName;

        if (string.IsNullOrWhiteSpace(genreName) || genreName == "All")
        {
            FilteredFilmModels = AllFilmModels;
        }
        else
        {
            FilteredFilmModels = AllFilmModels
                .Where(f => f.Genre.Name.Equals(genreName, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        var filmWord = FilteredFilmModels.Count == 1 ? "film" : "films";
        var genreText = genreName == "All" ? "in all genres" : $"in genre {genreName}";

        Snackbar.Add(
            $"{FilteredFilmModels.Count} {filmWord} found {genreText}.",
            FilteredFilmModels.Count > 0 ? Severity.Info : Severity.Warning);
    }
}
