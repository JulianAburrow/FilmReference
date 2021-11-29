using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmReference.DataAccess
{
    [Table("Person")]
    public partial class Person
    {
        [Key]
        public int PersonId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string FullName { get; set; }

        public string Description { get; set; }

        public bool IsActor { get; set; }

        public bool IsDirector { get; set; }

        public byte[] Picture { get; set; }

        public ICollection<Film> Film { get; set; }

        public ICollection<FilmPerson> FilmPerson { get; set; }
    }
}
