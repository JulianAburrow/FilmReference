using FilmReference.DataAccess;
using FilmReference.FrontEnd.Classes;
using FilmReference.FrontEnd.Classes.Helpers;
using FilmReference.FrontEnd.Config;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmReference.FrontEnd.Pages.FilmPages
{
    public class IndexModel : FilmReferencePageModel
    {
        #region Injected

        public ImageHelper _imageHelper;
        public ReplacementHelper _replacementHelper;
        public StringHelper _stringHelper;

        #endregion

        #region Constructor

        public IndexModel(FilmReferenceContext context,
            ImageHelper imageHelper,
            ReplacementHelper replacementHelper,
            StringHelper stringHelper)
            : base (context)
        {
            _imageHelper = imageHelper;
            _replacementHelper = replacementHelper;
            _stringHelper = stringHelper;
        }

        #endregion

        #region Properties

        public IList<Film> Films { get;set; }

        public IList<Genre> Genres { get; set; }

        public int GenreId { get; set; }

        public string GenreName { get; set; }

        #endregion

        #region Get
        public async Task OnGetAsync(int? genreId)
        {
            await DoPopulations(genreId);
        }

        #endregion

        #region Post

        public async Task OnPostAsync(int? genreId)
        {
            await DoPopulations(genreId);
        }

        #endregion

        #region Private Methods

        private async Task DoPopulations(int? genreId)
        {
            Films = await _context.Film
                .OrderBy(f => f.Name)
                .ToListAsync();

            GenreId = genreId != null
                ? Convert.ToInt32(genreId)
                : 0;

            if (GenreId > 0)
            {
                Films = Films.Where(f => f.GenreId == GenreId).ToList();
            }
            
            GenreName = GenreId > 0
                    ? _context.Genre.Find(GenreId).Name
                    : ConfigValues.All.Text;

            Genres = await _context.Genre
                .OrderBy(g => g.Name)
                .ToListAsync();
            Genres.Insert(0, new Genre()
            {
                GenreId = ConfigValues.All.Int,
                Name = ConfigValues.All.Text
            });
        }

        #endregion
    }
}
