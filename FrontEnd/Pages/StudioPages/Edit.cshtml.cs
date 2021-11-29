using FilmReference.DataAccess;
using FilmReference.FrontEnd.Classes;
using FilmReference.FrontEnd.Classes.Helpers;
using FilmReference.FrontEnd.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace FilmReference.FrontEnd.Pages.StudioPages
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

        public Studio Studio { get; set; }

        #endregion

        #region Get

        public async Task<IActionResult> OnGetAsync(int? studioId)
        {
            if (studioId == null)
            {
                return NotFound();
            }

            Studio = await _context.Studio.FirstOrDefaultAsync(m => m.StudioId == studioId);

            if (Studio == null)
            {
                return NotFound();
            }
            return Page();
        }

        #endregion

        #region Post

        public async Task<IActionResult> OnPostAsync(int? studioId)
        {
            if (studioId == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Studio = await _context.Studio.FirstOrDefaultAsync(s => s.StudioId == studioId);

            if (Studio == null)
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
                        ModelState.AddModelError(ConfigValues.StringValues.StudioPicture, errorMessage);
                        return Page();
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        Studio.Picture = memoryStream.ToArray();
                    }
                }
            }

            if (await TryUpdateModelAsync(
                Studio,
                nameof(Studio),
                s => s.StudioId, s => s.Name, s => s.Description, s => s.Picture))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage(ConfigValues.StringValues.StudioIndexPage);
            }

            return Page();
        }

        #endregion
    }
}
