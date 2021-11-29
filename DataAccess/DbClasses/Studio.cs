using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmReference.DataAccess
{
    [Table("Studio")]
    public partial class Studio
    {
        [Key]
        public int StudioId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public byte[] Picture { get; set; }

        public ICollection<Film> Film { get; set; }
    }
}
