namespace FilmReferenceUI.Components.Layout;

public partial class NavMenu
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Parameter] public string FirstGenre { get; set; } = string.Empty;

    private void DoNavigation(RoleEnum role)
    {
        if (role == RoleEnum.CastMembers)
        {
            NavigationManager.NavigateTo($"/people/listpeople/{RoleEnum.CastMembers}");
        }
        if (role == RoleEnum.Directors)
        {
            NavigationManager.NavigateTo($"/people/listpeople/{RoleEnum.Directors}");
        }
    }
}