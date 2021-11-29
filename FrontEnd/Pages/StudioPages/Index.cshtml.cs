using FilmReference.DataAccess;
using FilmReference.FrontEnd.Classes;
using FilmReference.FrontEnd.Classes.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmReference.FrontEnd.Pages.StudioPages
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

        public IList<Studio> Studios { get;set; }

        #endregion

        #region Get
        
        public async Task OnGetAsync()
        {
            Count = _context.Studio.Count();
            Studios = await _context.Studio
                .Include(s => s.Film)
                .OrderBy(s => s.Name)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }

        #endregion
    }
}
