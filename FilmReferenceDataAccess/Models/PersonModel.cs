using System.ComponentModel.DataAnnotations.Schema;

namespace FilmReferenceDataAccess.Models;

public class PersonModel
{
    public int PersonId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string? LastName { get; set; }

    public string? Description { get; set; }

    public bool IsActor { get; set; }

    public bool IsDirector { get; set; }

    public byte[]? Picture { get; set; }

    public ICollection<FilmModel> Films { get; set; } = null!;

    public ICollection<FilmPersonModel> FilmPerson { get; set; } = null!;
}
