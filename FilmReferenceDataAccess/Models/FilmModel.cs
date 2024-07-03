namespace FilmReferenceDataAccess.Models;

public class FilmModel
{
    public int FilmId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public byte[] Picture { get; set; } = null!;

    public int GenreId { get; set; }

    public int DirectorId { get; set; }

    public int StudioId { get; set; }

    public GenreModel Genre { get; set; }

    public PersonModel Director { get; set; }

    public StudioModel Studio { get; set; }

    public ICollection<FilmPersonModel> FilmPerson { get; set; }
}