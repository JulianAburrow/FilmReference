namespace FilmReferenceUI.Shared.Components;

public partial class ListFavouritesComponent
{
    [Parameter] public List<FavouriteDisplayModel> FavouriteDisplayModels { get; set; } = null!;

    [Parameter] public EventCallback OnRemoved { get; set; }

    private Dictionary<int, List<FavouriteDisplayModel>> _groupedFavourites = new();

    protected override void OnParametersSet()
    {
        _groupedFavourites = FavouriteDisplayModels
            .GroupBy(f => f.EntityTypeId)
            .ToDictionary(
                g => g.Key,
                g => g.ToList()
            );
    }

    private async Task HandleRemoved()
    {
        await OnRemoved.InvokeAsync();
    }
}