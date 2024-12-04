namespace FilmReferenceUI.Components.Films;

public partial class ListFilms
{
    List<FilmModel> FilmModels { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        FilmModels = await FilmHandler.GetFilmsAsync();
        Snackbar.Add($"{FilmModels.Count} film(s) found.", FilmModels.Count > 0 ? Severity.Info : Severity.Warning);
        MainLayout.SetHeaderValue("Films");
    }
}
