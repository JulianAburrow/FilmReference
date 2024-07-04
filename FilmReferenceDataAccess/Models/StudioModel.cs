namespace FilmReferenceDataAccess.Models;

public class StudioModel
{
    public int StudioId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public byte[]? Picture { get; set; } = null!;

    public ICollection<FilmModel> Films { get; set; } = null!;
}
