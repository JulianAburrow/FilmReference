using FilmReference.DataAccess;
using FilmReference.FrontEnd.Classes;
using FilmReference.FrontEnd.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FilmReference.FrontEnd.Pages.GenrePages
{
    public class EditModel : FilmReferencePageModel
    {
        #region Constructor

        public EditModel(FilmReferenceContext context)
            : base (context)
        {
        }

        #endregion

        #region Properties

        public Genre Genre { get; set; }

        #endregion

        #region Get

        public async Task<IActionResult> OnGetAsync(int? genreId)
        {
            if (genreId == null)
            {
                return NotFound();
            }

            Genre = await _context.Genre.FirstOrDefaultAsync(m => m.GenreId == genreId);

            if (Genre == null)
            {
                return NotFound();
            }
            return Page();
        }

        #endregion

        #region Post

        public async Task<IActionResult> OnPostAsync(int? genreId)
        {
            if (genreId == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Genre = await _context.Genre.FirstOrDefaultAsync(g => g.GenreId == genreId);

            if (Genre == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync(
                Genre,
                nameof(Genre),
                g => g.GenreId, g => g.Name, g => g.Description))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage(ConfigValues.StringValues.GenreIndexPage);
            }

            return Page();
        }

        #endregion
    }
}
