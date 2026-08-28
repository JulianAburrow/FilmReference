using System.ComponentModel.DataAnnotations.Schema;

namespace FilmReferenceDataAccess.Models;

public class PersonModel
{
    public int PersonId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string? LastName { get; set; }

    public string? Description { get; set; }

    public bool IsCastMember { get; set; }

    public bool IsDirector { get; set; }

    public int? NationalityId { get; set; }

    public NationalityModel Nationality { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public DateTime? DateOfDeath { get; set; }

    public int? Age
    {
        get
        {
            if (DateOfBirth is null)
                return null;

            var end = DateOfDeath ?? DateTime.Now;

            var age = end.Year - DateOfBirth.Value.Year;

            if (end < DateOfBirth.Value.AddYears(age))
                age--;

            return age;
        }
    }


    public byte[]? Picture { get; set; }

    public ICollection<FilmModel> Films { get; set; } = [];

    public ICollection<FilmPersonModel> FilmPerson { get; set; } = [];
}
