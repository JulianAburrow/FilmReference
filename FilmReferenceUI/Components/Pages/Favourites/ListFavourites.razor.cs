namespace FilmReferenceUI.Components.Pages.Favourites;

public partial class ListFavourites
{
    private List<FavouriteDisplayModel> FavouriteDisplayModels { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await GetFavouriteDisplayModels();
        MainLayout.SetHeaderValue("View Favourites");
        
        var count = FavouriteDisplayModels.Count;
        var word = count == 1 ? "favourite" : "favourites";

        Snackbar.Add(
            $"{count} {word} found.",
            count > 0 ? Severity.Info : Severity.Warning
        );
    }

    private async Task HandleRemoved()
    {
        await GetFavouriteDisplayModels();
    }

    private async Task GetFavouriteDisplayModels()
    {
        FavouriteDisplayModels = await FavouriteHandler.GetAllFavouritesAsync();
    }
}