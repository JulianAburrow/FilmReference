namespace FilmReferenceDataAccess.Models;

public class FilmPersonModel
{
    public int FilmPersonId { get; set; }

    public int FilmId { get; set; }

    public int PersonId { get; set; }

    public FilmModel Film { get; set; } = null!;

    public PersonModel Person { get; set; } = null!;
}
