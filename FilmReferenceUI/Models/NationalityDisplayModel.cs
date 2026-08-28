namespace FilmReferenceUI.Models;

public class NationalityDisplayModel
{
    public int NationalityId { get; set; }

    public string Name { get; set; } = string.Empty;

    public ICollection<PersonModel> People { get; set; } = [];
}
