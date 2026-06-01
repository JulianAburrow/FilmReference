using MudBlazor.Extensions;

namespace FilmReferenceUI.Shared.Components;

public partial class FavouriteComponent
{
    [Inject] protected IFavouriteHandler FavouriteHandler { get; set; } = null!;

    [Inject] protected ISnackbar Snackbar { get; set; } = null!;

    [Parameter] public FavouriteDisplayModel Favourite { get; set; } = null!;

    [Parameter] public EventCallback OnRemoved { get; set; }

    private async Task RemoveFavourite()
    {
        try
        {
            await FavouriteHandler.DeleteFavouriteAsync(Favourite.EntityTypeId, Favourite.EntityId, true);
            Snackbar.Add("Favourite removed", Severity.Success);
            await OnRemoved.InvokeAsync(Favourite.FavouriteId);
        }
        catch
        {
            Snackbar.Add("An error occurred removing Favourite. Please try again.", Severity.Error);
        }
    }
}