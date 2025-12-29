namespace FilmReferenceUI.Components.Pages.Films;

public partial class ListFilms
{
    private List<FilmModel> FilmModels { get; set; } = null!;

    [Parameter] public string Genre { get; set;} = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        FilmModels = await FilmHandler.GetAllFilmsForGenreAsync(Genre.ToLower().Replace(" ", ""));
        GenreModels = await GenreHandler.GetGenresAsync();
        MainLayout.SetHeaderValue("Films");
        Snackbar.Add(
            $"{FilmModels.Count} {(FilmModels.Count == 1 ? "film" : "films")} found for filter '{FilmModels[0].Genre.Name}'",
            FilmModels.Count > 0 ? Severity.Info : Severity.Warning);
    }

    private void Navigate(string genreName)
    {
        NavigationManager.NavigateTo($"/films/listfilms/{genreName.ToLower()}", forceLoad: true);
    }
}
