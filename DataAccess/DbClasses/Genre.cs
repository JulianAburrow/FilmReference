using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmReference.DataAccess
{
    [Table("Genre")]
    public partial class Genre
    {
        [Key]
        public int GenreId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Film> Film { get; set; }
    }
}
