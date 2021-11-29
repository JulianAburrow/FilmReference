using FilmReference.DataAccess;
using FilmReference.FrontEnd.Classes;
using FilmReference.FrontEnd.Classes.Helpers;
using FilmReference.FrontEnd.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilmReference.FrontEnd.Pages.FilmPages
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

        #region Properties

        public Film Film { get; set; }

        public List<int> SelectedActorIds { get; set; }

        public MultiSelectList SlActor { get; set; }

        public SelectList SlDirector { get; set; }

        public SelectList SlGenre { get; set; }

        public SelectList SlStudio { get; set; }

        #endregion

        #region Get

        public async Task<IActionResult> OnGet()
        {
            await DoPopulationsAsync();        
            return Page();
        }

        #endregion

        #region Post

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await DoPopulationsAsync();
                return Page();
            }

            var newFilm = new Film(_context)
            {
                FilmPerson = new List<FilmPerson>()
            };

            var selectedActorIds = Request.Form[nameof(SelectedActorIds)];
            var files = Request.Form.Files;
            if (files.Count > 0)
            {
                var file = files[0];
                if (file.Length > 0)
                {
                    if (!_imageHelper.FileTypeOk(file, out var errorMessage))
                    {
                        ModelState.AddModelError(ConfigValues.StringValues.FilmPicture, errorMessage);
                        await DoPopulationsAsync();
                        return Page();
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        newFilm.Picture = memoryStream.ToArray();
                    }
                }
            }
            
            if (await TryUpdateModelAsync(
                newFilm,
                nameof(Film),
                f => f.FilmId,
                f => f.Name,
                f => f.Description,
                f => f.Picture,
                f => f.GenreId,
                f => f.DirectorId,
                f => f.StudioId))
            {
                using (var trans = await _context.Database.BeginTransactionAsync())
                {
                    foreach (var actorId in selectedActorIds)
                    {
                        newFilm.FilmPerson.Add(new FilmPerson
                        {
                            FilmId = newFilm.FilmId,
                            PersonId = Convert.ToInt32(actorId)
                        });
                    }

                    _context.Add(newFilm);
                    await _context.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                return RedirectToPage(ConfigValues.StringValues.FilmIndexPage, new { genreid = newFilm.GenreId });
            }

            await DoPopulationsAsync();
            return Page();
        }

        #endregion

        #region Private Methods

        private async Task DoPopulationsAsync()
        {
            await PopulateActorsAsync();
            await PopulateDirectorsAsync();
            await PopulateGenresAsync();
            await PopulateStudiosAsync();
        }

        private async Task PopulateActorsAsync()
        {
            SlActor = new MultiSelectList(
                await _context.Person
                                .Where(p => p.IsActor)
                                .OrderBy(p => p.FullName)
                                .ToListAsync(),
                nameof(Person.PersonId),
                nameof(Person.FullName));
        }

        private async Task PopulateDirectorsAsync()
        {
            var directors = await _context.Person
                .Where(p => p.IsDirector)
                .OrderBy(p => p.FullName)
                .ToListAsync();
            directors.Insert(0, new Person()
            {
                PersonId = ConfigValues.PleaseSelect.Int,
                FullName = ConfigValues.PleaseSelect.Text
            });
            SlDirector = new SelectList(
                    directors,
                    nameof(Person.PersonId),
                    nameof(Person.FullName));
        }

        private async Task PopulateGenresAsync()
        {
            var genres = await _context.Genre
                                    .OrderBy(g => g.Name)
                                    .ToListAsync();
            genres.Insert(0, new Genre()
            {
                GenreId = ConfigValues.PleaseSelect.Int,
                Name = ConfigValues.PleaseSelect.Text
            });
            SlGenre = new SelectList(
                    genres,
                    nameof(Genre.GenreId),
                    nameof(Genre.Name));
        }

        private async Task PopulateStudiosAsync()
        {
            var studios = await _context.Studio
                                    .OrderBy(s => s.Name)
                                    .ToListAsync();
            studios.Insert(0, new Studio()
            {
                StudioId = ConfigValues.PleaseSelect.Int,
                Name = ConfigValues.PleaseSelect.Text
            });
            SlStudio = new SelectList(
                studios,
                nameof(Studio.StudioId),
                nameof(Studio.Name));

        }

        #endregion
    }
}
