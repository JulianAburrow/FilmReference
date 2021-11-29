using FilmReference.DataAccess;
using FilmReference.FrontEnd.Classes;
using FilmReference.FrontEnd.Classes.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmReference.FrontEnd.Pages
{
    public class IndexModel : FilmReferencePageModel
    {
        #region Injected

        public readonly ImageHelper _imageHelper;

        #endregion

        #region Constructor

        public IndexModel(FilmReferenceContext context,
            ImageHelper imageHelper) : base(context)
        {
            _imageHelper = imageHelper;
        }

        #endregion

        #region Properties

        public IList<Film> Films { get; set; }

        #endregion

        public async Task OnGetAsync()
        {
            Films = await _context.Film
                .OrderBy(f => f.Name)
                .ToListAsync();
        }
    }
}
