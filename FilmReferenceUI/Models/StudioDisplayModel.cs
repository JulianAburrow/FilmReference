namespace FilmReferenceUI.Models;

public class StudioDisplayModel
{
    public int StudioId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(50, ErrorMessage = "{0} cannot be more than {1} characters")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "{0} cannot be more than {1} characters")]
    public string? Description { get; set; } = string.Empty;

    public byte[]? Logo { get; set; }

    public ICollection<FilmModel> Films { get; set; } = null!;
}
