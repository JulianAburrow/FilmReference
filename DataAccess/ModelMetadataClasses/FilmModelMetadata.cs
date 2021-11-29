using System.ComponentModel.DataAnnotations;

namespace FilmReference.DataAccess
{
    internal class FilmModelMetadata
    {
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, ErrorMessage = "{0} cannot be more than {1} characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(500, ErrorMessage = "{0} cannot be more than {1} characters")]
        public string Description { get; set; }

        [Display(Name = "Genre")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} is required")]
        public int GenreId { get; set; }

        [Display(Name = "Director")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} is required")]
        public int DirectorId { get; set; }

        [Display(Name = "Studio")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} is required")]
        public int StudioId { get; set; }

        [Display(Name = "Cover Photo")]
        public byte[] Picture { get; set; }
    }
}
