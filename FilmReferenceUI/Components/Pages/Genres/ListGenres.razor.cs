namespace FilmReferenceUI.Components.Pages.Genres;

public partial class ListGenres
{
    List<GenreModel> GenreModels { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        GenreModels = await GenreHandler.GetGenresAsync();
        Snackbar.Add($"{GenreModels.Count} {(GenreModels.Count == 1 ? "genre" : "genres")} found.", GenreModels.Count > 0 ? Severity.Info : Severity.Warning);
        MainLayout.SetHeaderValue("View Genres");
    }
}
