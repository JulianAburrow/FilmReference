using FilmReference.DataAccess;
using FilmReference.FrontEnd.Classes;
using FilmReference.FrontEnd.Classes.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmReference.FrontEnd.Pages.DirectorPages
{
    public class IndexModel : FilmReferencePageModel
    {
        #region Injected

        public ReplacementHelper _replacementHelper;

        #endregion

        #region Constructor

        public IndexModel(FilmReferenceContext context, ReplacementHelper replacementHelper)
            : base (context)
        {
            _replacementHelper = replacementHelper;
        }

        #endregion

        #region Properties

        public IList<Person> Directors { get;set; }

        #endregion

        #region Get
        
        public async Task OnGetAsync()
        {
            Count = _context.Person
                .Where(p => p.IsDirector)
                .Count();
            Directors = await _context.Person
                .Include(p => p.Film)
                .Where(p => p.IsDirector)
                .OrderBy(p => p.FullName)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }

        #endregion
    }
}
