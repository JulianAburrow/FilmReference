namespace FilmReferenceDataAccess.Models;

public class FeaturedPersonModel
{
    public int PersonId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string? LastName { get; set; } = null!;

    public string? Description { get; set; } = null!;

    public int FilmCount { get; set; }

    public byte[]? Picture { get; set; }
}
