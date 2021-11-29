using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FilmReference.DataAccess
{
    internal class PersonModelMetadata
    {
        [StringLength(50, ErrorMessage = "{0} cannot be more than {1} characters")]
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "{0} cannot be more than {1} characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(200, ErrorMessage = "{0} cannot be more than {1} characters")]
        public string Description { get; set; }

        [Display(Name = "Actor")]
        public bool IsActor { get; set; }

        [Display(Name = "Director")]
        public bool IsDirector { get; set; }

        [Display(Name = "Films")]
        public ICollection<Film> Film { get; set; }
    }
}
