using FilmReference.DataAccess;
using FilmReference.FrontEnd.Classes;
using FilmReference.FrontEnd.Classes.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmReference.FrontEnd
{
    public class IndexModel : FilmReferencePageModel
    {
        #region Injected

        public ImageHelper _imageHelper;
        public ReplacementHelper _replacementHelper;

        #endregion

        #region Constructor

        public IndexModel(FilmReferenceContext context, ImageHelper imageHelper, ReplacementHelper replacementHelper)
            : base (context)
        {
            _imageHelper = imageHelper;
            _replacementHelper = replacementHelper;
        }

        #endregion

        #region Properties

        public List<Person> Actors { get;set; }

        public List<string> AtoZ { get; set; }

        public string FirstInitial { get; set; }

        #endregion

        #region Get

        public async Task OnGetAsync(string firstInitial)
        {
            FirstInitial = string.IsNullOrEmpty(firstInitial)
                ? "A"
                : firstInitial.ToUpper();

            Actors = await _context.Person
                .Include(p => p.FilmPerson)
                .Where(p =>
                    p.IsActor &&
                    p.FullName.StartsWith(FirstInitial))
                .OrderBy(p => p.FullName)
                .ToListAsync();

            AtoZ = new List<string>();

            for (var i = 65; i <= 90; i ++)
            {
                AtoZ.Add(((char)i).ToString());
            }
        }

        #endregion
    }
}
