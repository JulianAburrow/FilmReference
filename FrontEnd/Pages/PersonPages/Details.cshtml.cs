using FilmReference.DataAccess;
using FilmReference.FrontEnd.Classes;
using FilmReference.FrontEnd.Classes.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmReference.FrontEnd
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

        public Person Person { get; set; }

        public List<Film> Films { get; set; }

        #endregion

        #region Get

        public async Task<IActionResult> OnGetAsync(int? personId)
        {
            if (personId == null)
            {
                return NotFound();
            }

            Person = await _context.Person
                .Include(p => p.Film)
                .Include(p => p.FilmPerson)
                    .ThenInclude(fp => fp.Film)
                .FirstOrDefaultAsync(m => m.PersonId == personId);

            if (Person == null)
            {
                return NotFound();
            }

            Films = new List<Film>();
            var filmPersons = Person.FilmPerson.OrderBy(fp => fp.Film.Name);
            foreach (var filmPerson in filmPersons)
            {
                Films.Add(filmPerson.Film);
            }
            if (Person.IsDirector)
            {
                foreach(var film in Person.Film)
                {
                    if (!Films.Contains(film))
                    {
                        Films.Add(film);
                    }
                }
            }
            Films.OrderBy(f => f.Name);

            return Page();
        }

        #endregion
    }
}
