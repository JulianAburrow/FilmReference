using System.Runtime.InteropServices;

namespace FilmReferenceUI.Components.Pages.Genres;

public partial class ListGenres
{
    List<GenreModel> GenreModels { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        GenreModels = await GenreHandler.GetGenresAsync();
        Snackbar.Add($"{GenreModels.Count} {(GenreModels.Count == 1 ? "genre" : "genres")} found.", GenreModels.Count > 0 ? Severity.Info : Severity.Warning);
        MainLayout.SetHeaderValue("View Genres");
        NextSortDirection = SortDirection.Descending;
    }

    private void ResortList()
    {
        switch (NextSortDirection)
        {
            case SortDirection.Ascending:
                GenreModels = [.. GenreModels.OrderBy(g => g.Name)];
                NextSortDirection = SortDirection.Descending;
                break;
            case SortDirection.Descending:
                GenreModels = [.. GenreModels.OrderByDescending(g => g.Name)];
                NextSortDirection = SortDirection.Ascending;
                break;
            default:
                GenreModels = [.. GenreModels.OrderBy(g => g.Name)];
                NextSortDirection = SortDirection.Descending;
                break;
        }
    }
}
