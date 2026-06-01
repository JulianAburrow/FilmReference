using FilmReferenceDataAccess.Enums;

namespace FilmReferenceUI.Components.Layout;

public partial class MainLayout
{
    [Inject] private IGenreHandler GenreHandler { get; set; } = null!;

    [Inject] private IFavouriteHandler FavouriteHandler { get; set; } = null!;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Inject] private ISnackbar Snackbar { get; set; } = null!;

    bool _drawerOpen = true;

    void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }

    private readonly List<BreadcrumbItem> BreadCrumbs = [];

    private string FirstGenre { get; set; } = string.Empty;

    private string HeaderText { get; set; } = null!;

    private bool DisplayFavouriteButton { get; set; }

    private string FavouriteTooltipText =>
                    IsAlreadyFavourite ? "Remove from Favourites" : "Add to Favourites";

    private string FavouriteIcon =>
        IsAlreadyFavourite ? Icons.Material.Filled.Favorite : Icons.Material.Outlined.FavoriteBorder;

    private FavouriteEntityEnum CurrentEntityType { get; set; }

    private int CurrentEntityId { get; set; }

    private bool IsAlreadyFavourite { get; set; }

    public void ConfigureFavouriteButton(FavouriteEntityEnum entityType, int entityId, bool isAlreadyFavourite)
    {
        IsAlreadyFavourite = isAlreadyFavourite;

        DisplayFavouriteButton = true;
        CurrentEntityType = entityType;
        CurrentEntityId = entityId;
        StateHasChanged();
    }

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
        NavigationManager.LocationChanged += (_, __) =>
        {
            DisplayFavouriteButton = false;
            StateHasChanged();
        };
    }

    private async Task AddRemoveFavourite()
    {
        if (IsAlreadyFavourite)
        {
            try
            {
                await FavouriteHandler.DeleteFavouriteAsync((int)CurrentEntityType, CurrentEntityId, true);
                Snackbar.Add($"Favourite {CurrentEntityType} successfully removed from Favourites.", Severity.Success);
                IsAlreadyFavourite = false;
            }
            catch
            {
                Snackbar.Add($"An error occurred removing Favourite {CurrentEntityType}. Please try again.", Severity.Error);
            }
            return;
        }

        try
        {
            var favourite = new FavouriteModel
            {
                EntityTypeId = (int)CurrentEntityType,
                EntityId = CurrentEntityId
            };

            await FavouriteHandler.CreateFavouriteAsync(favourite, true);
            IsAlreadyFavourite = true;
            StateHasChanged() ;

            Snackbar.Add($"Favourite {CurrentEntityType} successfully created.", Severity.Success);
        }
        catch
        {
            Snackbar.Add($"An error occurred creating Favourite {CurrentEntityType}. Please try again.", Severity.Error);
        }
    }
}
