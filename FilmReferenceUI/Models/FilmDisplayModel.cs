namespace FilmReferenceUI.Models
{
    public class FilmDisplayModel
    {
        public int FilmId { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, ErrorMessage = "{0} cannot be more than {1} characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(500, ErrorMessage = "{0} cannot be more than {1} characters")]
        public string Description { get; set; } = string.Empty;

        public byte[]? BoxCover { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "{0} is required")]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "{0} is required")]
        [Display(Name = "Director")]
        public int DirectorId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "{0} is required")]
        [Display(Name = "Studio")]
        public int StudioId { get; set; }

        public IEnumerable<int> SelectedCastMemberIds { get; set; } = [];

        public GenreModel Genre { get; set; } = null!;

        public PersonModel Director { get; set; } = null!;

        public StudioModel Studio { get; set; } = null!;

        public ICollection<FilmPersonModel>? FilmPerson { get; set; } = null!;
    }
}
