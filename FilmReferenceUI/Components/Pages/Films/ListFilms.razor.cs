namespace FilmReferenceUI.Components.Pages.Films;

public partial class ListFilms
{
    private List<FilmModel> AllFilmModels { get; set; } = null!;

    private List<FilmModel> FilteredFilmModels { get; set; } = null!;

    private string Genre { get; set;} = "All";

    protected override async Task OnInitializedAsync()
    {
        AllFilmModels = await FilmHandler.GetAllFilmsAsync();
        GenreModels = await GenreHandler.GetGenresAsync();
        MainLayout.SetHeaderValue("View Films");
        FilterFilms(Genre);
        _isLoaded = true;
    }

    private void FilterFilms(string genreName)
    {
        Genre = genreName;

        if (string.IsNullOrWhiteSpace(genreName) || genreName == "All")
        {
            FilteredFilmModels = [.. AllFilmModels.OrderBy(f => f.Name)];
        }
        else
        {
            FilteredFilmModels = [.. AllFilmModels
                .Where(f => f.Genre.Name.Equals(genreName, StringComparison.OrdinalIgnoreCase))
                .OrderBy(f => f.Name)];
        }

        var filmWord = FilteredFilmModels.Count == 1 ? "film" : "films";
        var genreText = genreName == "All" ? "in all genres" : $"in genre {genreName}";

        Snackbar.Add(
            $"{FilteredFilmModels.Count} {filmWord} found {genreText}.",
            FilteredFilmModels.Count > 0 ? Severity.Info : Severity.Warning);

        NextSortDirection = SortDirection.Descending;
    }

    private void ResortList()
    {
        switch (NextSortDirection)
        {
            case SortDirection.Ascending:
                FilteredFilmModels = [.. FilteredFilmModels.OrderBy(f => f.Name)];
                NextSortDirection = SortDirection.Descending;
                break;
            case SortDirection.Descending:
                FilteredFilmModels = [.. FilteredFilmModels.OrderByDescending(f => f.Name)];
                NextSortDirection = SortDirection.Ascending;
                break;
            default:
                FilteredFilmModels = [.. FilteredFilmModels.OrderBy(f => f.Name)];
                NextSortDirection = SortDirection.Descending;
                break;
        }
    }
}
