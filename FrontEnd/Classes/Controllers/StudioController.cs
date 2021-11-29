using FilmReference.DataAccess;
using FilmReference.FrontEnd.Classes.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FilmReference.FrontEnd.Classes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudioController : Controller
    {
        private readonly FilmReferenceContext _context;
        private readonly ImageHelper _imageHelper;

        public StudioController(FilmReferenceContext context, ImageHelper imageHelper)
        {
            _context = context;
            _imageHelper = imageHelper;
        }

        // GET: /Studio/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            var imageData = _context.Studio.Find(id).Picture;
            return imageData != null
                ? _imageHelper.ImageSource(imageData)
                : "";
        }
    }
}
