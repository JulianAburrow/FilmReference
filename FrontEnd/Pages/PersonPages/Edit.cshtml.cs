using FilmReference.DataAccess;
using FilmReference.FrontEnd.Classes;
using FilmReference.FrontEnd.Classes.Helpers;
using FilmReference.FrontEnd.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilmReference.FrontEnd
{
    public class EditModel : FilmReferencePageModel
    {
        #region Injected

        public readonly ImageHelper _imageHelper;

        #endregion

        #region Constructor

        public EditModel(FilmReferenceContext context, ImageHelper imageHelper)
            : base (context)
        {
            _imageHelper = imageHelper;
        }

        #endregion

        #region Properties

        public Person Person { get; set; }

        #endregion

        #region Get

        public async Task<IActionResult> OnGetAsync(int? personId)
        {
            if (personId == null)
            {
                return NotFound();
            }

            Person = await _context.Person
                .FirstOrDefaultAsync(m => m.PersonId == personId);

            if (Person == null)
            {
                return NotFound();
            }
            return Page();
        }

        #endregion

        #region Post

        public async Task<IActionResult> OnPostAsync(int? personId)
        {
            if (personId == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Person = await _context.Person
                .FirstOrDefaultAsync(p => p.PersonId == personId);

            if (Person == null)
            {
                return NotFound();
            }

            var files = Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                if (file.Length > 0)
                {
                    if (!_imageHelper.FileTypeOk(file, out var errorMessage))
                    {
                        ModelState.AddModelError(ConfigValues.StringValues.PersonPicture, errorMessage);
                        return Page();
                    }
                }

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    Person.Picture = memoryStream.ToArray();
                }
            }

            if (await TryUpdateModelAsync(
                Person,
                nameof(Person),
                p => p.PersonId, p => p.FirstName, p => p.LastName, p => p.Description, p => p.IsActor, p => p.IsDirector, p => p.Picture))
            {
                await _context.SaveChangesAsync();
                if (!Person.IsActor && Person.IsDirector)
                {
                    return RedirectToPage(ConfigValues.StringValues.DirectorIndexPage);
                }
                else
                {
                    return RedirectToPage(ConfigValues.StringValues.PersonIndexPage, new { firstinitial = Person.FirstName.Substring(0, 1) });
                }
            }

            return Page();
        }

        #endregion
    }
}
