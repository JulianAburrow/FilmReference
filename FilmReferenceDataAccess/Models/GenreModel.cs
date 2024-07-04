namespace FilmReferenceDataAccess.Models;

public partial class GenreModel
{
    public int GenreId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<FilmModel> Films { get; set; } = null!;
}
