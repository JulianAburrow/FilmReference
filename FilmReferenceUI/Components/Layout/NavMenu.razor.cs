namespace FilmReferenceUI.Components.Layout;

public partial class NavMenu
{
    [Parameter] public string FirstGenre { get; set; } = string.Empty;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
}