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

    public GenreModel Genre { get; set; } = null!;

    public PersonModel Director { get; set; } = null!;

    public StudioModel Studio { get; set; } = null!;

    public ICollection<FilmPersonModel> FilmPerson { get; set; } = [];
}