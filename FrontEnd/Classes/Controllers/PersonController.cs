using FilmReference.DataAccess;
using FilmReference.FrontEnd.Classes.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace FilmReference.FrontEnd.Classes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : Controller
    {
        private readonly FilmReferenceContext _context;
        private readonly ImageHelper _imageHelper;

        public PersonController(FilmReferenceContext context, ImageHelper imageHelper)
        {
            _context = context;
            _imageHelper = imageHelper;
        }


        // GET <controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            var imageData = _context.Person.Find(id).Picture;
            return imageData != null
                ? _imageHelper.ImageSource(imageData)
                : "";
        }
    }
}
