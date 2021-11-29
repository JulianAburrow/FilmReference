using FilmReference.DataAccess;
using FilmReference.FrontEnd.Classes;
using FilmReference.FrontEnd.Classes.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmReference.FrontEnd.Pages.GenrePages
{
    public class IndexModel : FilmReferencePageModel
    {
        #region Injected

        public ReplacementHelper _replacementHelper;

        public StringHelper _stringHelper;

        #endregion

        #region Constructor

        public IndexModel(FilmReferenceContext context,
            ReplacementHelper replacementHelper,
            StringHelper stringHelper)
            : base (context)
        {
            _replacementHelper = replacementHelper;
            _stringHelper = stringHelper;
        }

        #endregion

        #region Properties

        public IList<Genre> Genres { get;set; }

        #endregion

        #region Get

        public async Task OnGetAsync()
        {
            Count = _context.Genre.Count();
            Genres = await _context.Genre
                .Include(g => g.Film)
                .OrderBy(g => g.Name)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }

        #endregion
    }
}
