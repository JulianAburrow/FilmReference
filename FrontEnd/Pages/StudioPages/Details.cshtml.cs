using FilmReference.DataAccess;
using FilmReference.FrontEnd.Classes;
using FilmReference.FrontEnd.Classes.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmReference.FrontEnd.Pages.StudioPages
{
    public class DetailsModel : FilmReferencePageModel
    {
        #region Injected

        public ImageHelper _imageHelper;

        #endregion

        #region Constructor

        public DetailsModel(FilmReferenceContext context, ImageHelper imageHelper)
            : base (context)
        {
            _imageHelper = imageHelper;
        }

        #endregion

        #region Properties

        public Studio Studio { get; set; }

        public List<Film> Films { get; set; }

        #endregion

        #region Get

        public async Task<IActionResult> OnGetAsync(int? studioId)
        {
            if (studioId == null)
            {
                return NotFound();
            }

            Studio = await _context.Studio
                .Include(s => s.Film)
                .FirstOrDefaultAsync(m => m.StudioId == studioId);

            if (Studio == null)
            {
                return NotFound();
            }

            Films = Studio.Film
                .OrderBy(f => f.Name)
                .ToList();

            return Page();
        }

        #endregion
    }
}
