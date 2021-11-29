using FilmReference.DataAccess;
using FilmReference.FrontEnd.Classes;
using FilmReference.FrontEnd.Classes.Helpers;
using FilmReference.FrontEnd.Config;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace FilmReference.FrontEnd
{
    public class CreateModel : FilmReferencePageModel
    {
        #region Injected

        private readonly ImageHelper _imageHelper;

        #endregion

        #region Constructor

        public CreateModel(FilmReferenceContext context, ImageHelper imageHelper)
            : base (context)
        {
            _imageHelper = imageHelper;
        }

        #endregion

        #region Get

        public IActionResult OnGet()
        {
            return Page();
        }

        #endregion

        #region Properties

        public Person Person { get; set; }

        #endregion

        #region Post

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newPerson = new Person(_context);

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

                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        newPerson.Picture = memoryStream.ToArray();
                    }
                }
            }

            if (await TryUpdateModelAsync(
                newPerson,
                nameof(Person),
                    p => p.PersonId,
                    p => p.FirstName,
                    p => p.LastName,
                    p => p.Description,
                    p => p.IsActor,
                    p => p.IsDirector,
                    p => p.Picture))
            {
                _context.Add(newPerson);
                await _context.SaveChangesAsync();
                if (!newPerson.IsActor && newPerson.IsDirector)
                {
                    return RedirectToPage(ConfigValues.StringValues.DirectorIndexPage);
                }
                else
                {
                    return RedirectToPage(ConfigValues.StringValues.PersonIndexPage, new { firstinitial = newPerson.FirstName.Substring(0, 1) });
                }
            }
            return Page();
        }

        #endregion
    }
}
