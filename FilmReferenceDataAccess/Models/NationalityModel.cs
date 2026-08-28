namespace FilmReferenceDataAccess.Models;

public class NationalityModel
{
    public int NationalityId { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<PersonModel> People { get; set; } = [];
}
