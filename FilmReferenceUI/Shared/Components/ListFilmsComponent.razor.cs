namespace FilmReferenceUI.Shared.Components;

public partial class ListFilmsComponent
{
    [Parameter] public List<FilmModel> FilmModels { get; set; } = null!;
}
