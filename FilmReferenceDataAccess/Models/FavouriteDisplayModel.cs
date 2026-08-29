namespace FilmReferenceDataAccess.Models;

public class FavouriteDisplayModel
{
    public int FavouriteId { get; set; }

    public int EntityTypeId { get; set; }

    public string EntityTypeName { get; set; } = string.Empty;

    public int EntityId { get; set; }

    public string EntityName { get; set; } = string.Empty;

    public string? EntityDescription { get; set; } = null!;

    public byte[]? EntityImage { get; set; }
}