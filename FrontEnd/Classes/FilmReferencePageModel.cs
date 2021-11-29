using FilmReference.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FilmReference.FrontEnd.Classes
{
    public class FilmReferencePageModel : PageModel
    {
        protected FilmReferenceContext _context;
    
        public FilmReferencePageModel(FilmReferenceContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public int Count { get; set; }

        public int PageSize { get; set; } = 5;

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
    }
}
