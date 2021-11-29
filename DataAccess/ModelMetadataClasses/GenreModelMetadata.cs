using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmReference.DataAccess
{
    internal class GenreModelMetadata
    {
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, ErrorMessage = "{0} cannot be more thtan {1} characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(500, ErrorMessage = "{0} cannot be more than {1} characters")]
        public string Description { get; set; }

        [Display(Name = "Films")]
        public ICollection<Film> Film { get; set; }
    }
}
