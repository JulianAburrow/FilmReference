using FilmReference.DataAccess;
using FilmReference.FrontEnd.Classes;
using FilmReference.FrontEnd.Classes.Helpers;
using FilmReference.FrontEnd.Config;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace FilmReference.FrontEnd.Pages.StudioPages
{
    public class CreateModel : FilmReferencePageModel
    {
        #region Injected

        private ImageHelper _imageHelper;

        #endregion

        #region Constructor

        public CreateModel(FilmReferenceContext context, ImageHelper imageHelper)
            : base (context)
        {
            _imageHelper = imageHelper;
        }

        #endregion

        #region Properties

        public Studio Studio { get; set; }

        #endregion

        #region Get
        
        public IActionResult OnGet()
        {
            return Page();
        }

        #endregion

        #region Post

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var newStudio = new Studio(_context);

            var files = Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                if (file.Length > 0)
                {
                    if (!_imageHelper.FileTypeOk(file, out var errorMessage))
                    {
                        ModelState.AddModelError(ConfigValues.StringValues.StudioPicture, errorMessage);
                        return Page();
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        newStudio.Picture = memoryStream.ToArray();
                    }
                }
            }

            if (await TryUpdateModelAsync(
                newStudio,
                nameof(Studio),
                s => s.StudioId, s => s.Name, s => s.Description, s => s.Picture))
            {
                _context.Add(newStudio);
                await _context.SaveChangesAsync();
                return RedirectToPage(ConfigValues.StringValues.StudioIndexPage);
            }

            return Page();
        }

        #endregion
    }
}
