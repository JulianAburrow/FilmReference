using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmReference.DataAccess
{
    [Table("Film")]
    public partial class Film
    {
        [Key]
        public int FilmId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }

        public int GenreId { get; set; }

        public int DirectorId { get; set; }

        public int StudioId { get; set; }

        public Genre Genre { get; set; }

        public Person Director { get; set; }

        public Studio Studio { get; set; }

        public ICollection<FilmPerson> FilmPerson { get; set; } 
    }
}
