using FilmReference.DataAccess;
using FilmReference.FrontEnd.Classes;
using FilmReference.FrontEnd.Config;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FilmReference.FrontEnd.Pages.GenrePages
{
    public class CreateModel : FilmReferencePageModel
    {
        #region Constructor

        public CreateModel(FilmReferenceContext context)
            : base (context)
        {
        }

        #endregion

        #region Properties

        public Genre Genre { get; set; }

        #endregion

        #region Get

        public IActionResult OnGet()
        {
            return Page();
        }

        #endregion

        #region Post

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newGenre = new Genre(_context);

            if (await TryUpdateModelAsync(
                newGenre,
                nameof(Genre),
                g => g.GenreId, g => g.Name, g => g.Description))
            {
                _context.Add(newGenre);
                await _context.SaveChangesAsync();
                return RedirectToPage(ConfigValues.StringValues.GenreIndexPage);
            }

            return Page();
        }

        #endregion
    }
}
