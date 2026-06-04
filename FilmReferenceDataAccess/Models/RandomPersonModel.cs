namespace FilmReferenceDataAccess.Models;

public class RandomPersonModel
{
    public int PersonId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string? LastName { get; set; } = null!;

    public byte[]? Picture { get; set; }
}
