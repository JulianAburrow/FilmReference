namespace FilmReferenceUI.Shared.Components;

public partial class ListStudiosComponent
{
    [Parameter] public List<StudioModel> StudioModels { get; set; } = null!;
}
