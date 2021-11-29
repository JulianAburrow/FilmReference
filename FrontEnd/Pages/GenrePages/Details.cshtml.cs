using FilmReference.DataAccess;
using FilmReference.FrontEnd.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FilmReference.FrontEnd.Pages.GenrePages
{
    public class DetailsModel : FilmReferencePageModel
    {
        public DetailsModel(FilmReferenceContext context)
            : base (context)
        {
        }

        public Genre Genre { get; set; }

        public async Task<IActionResult> OnGetAsync(int? genreId)
        {
            if (genreId == null)
            {
                return NotFound();
            }

            Genre = await _context.Genre
                .Include(g => g.Film)
                .FirstOrDefaultAsync(m => m.GenreId == genreId);

            if (Genre == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
