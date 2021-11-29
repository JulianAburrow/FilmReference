using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmReference.DataAccess
{
    [Table("FilmPerson")]
    public partial class FilmPerson
    {
        [Key]
        public int FilmPersonId { get; set; }

        public int FilmId { get; set; }

        public int PersonId { get; set; }

        public Film Film { get; set; }

        public Person Person { get; set; }
    }
}
