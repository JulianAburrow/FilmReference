

namespace FilmReferenceUI.Features.Films;

public partial class ListFilms
{
    List<FilmModel> FilmModels { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        FilmModels = await FilmHandler.GetFilmsAsync();
    }
}
