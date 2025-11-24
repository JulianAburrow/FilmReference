using System.ComponentModel.DataAnnotations;

namespace FilmReferenceDataAccess.Models;

public class SearchModel
{
    [Range(1, int.MaxValue, ErrorMessage = "{0} is required")]
    [Display(Name = "Search Type")]
    public int SearchType { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Search Text")]
    public string SearchText { get; set; } = string.Empty;
}
