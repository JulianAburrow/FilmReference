namespace FilmReferenceUI.Shared.Components;

public partial class ListGenresComponent
{
    [Parameter] public List<GenreModel> GenreModels { get; set; } = null!;
}
