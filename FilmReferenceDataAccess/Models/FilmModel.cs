namespace FilmReferenceDataAccess.Models;

public class FilmModel
{
    public int FilmId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int GenreId { get; set; }

    public int DirectorId { get; set; }

    public int StudioId { get; set; }

    public byte[]? BoxCover { get; set; }

    public GenreModel Genre { get; set; } = new();

    public PersonModel Director { get; set; } = new();

    public StudioModel Studio { get; set; } = new();

    public ICollection<FilmPersonModel> FilmPerson { get; set; } = [];
}