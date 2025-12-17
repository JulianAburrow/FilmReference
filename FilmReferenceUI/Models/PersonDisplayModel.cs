namespace FilmReferenceUI.Models;

public class PersonDisplayModel
{
    public int PersonId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(50, ErrorMessage = "{0} cannot be more than {1} characters")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "{0} cannot be more than {1} characters")]
    [Display(Name = "Last Name")]
    public string? LastName { get; set; }

    [StringLength(500, ErrorMessage = "{0} cannot be more than {1} characters")]
    public string? Description { get; set; }

    public bool IsCastMember { get; set; }

    public bool IsDirector { get; set; }

    public byte[]? Picture { get; set; }

    public ICollection<FilmModel> Films { get; set; } = null!;

    public ICollection<FilmPersonModel> FilmPerson { get; set; } = null!;
}
