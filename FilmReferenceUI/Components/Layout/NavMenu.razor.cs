namespace FilmReferenceUI.Components.Layout;

public partial class NavMenu
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    private void DoPersonNavigation(RoleEnum peopleRequired)
    {
        NavigationManager.NavigateTo($"people/listpeople/{peopleRequired}", forceLoad: true);
    }
}