namespace FilmReferenceDataAccess.Models;

public class PersonModel
{
    public int PersonId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string? LastName { get; set; }

    public string? Description { get; set; }

    public string? Notes { get; set; }

    public bool IsCastMember { get; set; }

    public bool IsDirector { get; set; }

    public int? NationalityId { get; set; }

    public NationalityModel? Nationality { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public DateOnly? DateOfDeath { get; set; }

    public int? Age
    {
        get
        {
            if (DateOfBirth is null)
                return null;

            var end = DateOfDeath ?? DateOnly.FromDateTime(DateTime.Today);

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
