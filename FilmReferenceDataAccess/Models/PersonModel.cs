using System.ComponentModel.DataAnnotations.Schema;

namespace FilmReferenceDataAccess.Models;

public class PersonModel
{
    public int PersonId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public string FullName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsActor { get; set; }

    public bool IsDirector { get; set; }

    public byte[] Picture { get; set; } = null!;

    public ICollection<FilmModel> Film { get; set; } = null!;

    public ICollection<FilmPersonModel> FilmPerson { get; set; } = null!;
}
