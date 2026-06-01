using FilmReferenceDataAccess.Enums;

namespace FilmReferenceUI.Models;

public class FavouriteListItemViewModel
{
    public int FavouriteId { get; set; }

    public string Title { get; set; } = string.Empty;

    public byte[]? Image { get; set; }

    public FavouriteEntityEnum EntityType { get; set; }

    public int EntityId { get; set; }
}