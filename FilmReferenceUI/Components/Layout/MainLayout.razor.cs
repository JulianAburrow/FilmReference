namespace FilmReferenceUI.Components.Layout;

public partial class MainLayout
{
    [Inject] IGenreHandler GenreHandler { get; set; } = null!;

    bool _drawerOpen = true;

    void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    private readonly List<BreadcrumbItem> BreadCrumbs = [];

    private string FirstGenre { get; set; } = string.Empty;

    private string HeaderText { get; set; } = null!;

    public void SetHeaderValue(string headerText)
    {
        HeaderText = headerText;
        StateHasChanged();
    }

    public void SetBreadcrumbs(List<BreadcrumbItem> breadcrumbs)
    {
        BreadCrumbs.Clear();
        BreadCrumbs.AddRange(breadcrumbs);
    }

    protected override async Task OnInitializedAsync()
    {
        FirstGenre = await GenreHandler.GetFirstGenreAsync();
        FirstGenre = FirstGenre.Replace(" ", "").ToLower();
    }
}
