using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FilmReference.DataAccess
{
    [ModelMetadataType(typeof(GenreModelMetadata))]
    public partial class Genre : IValidatableObject
    {
        private readonly FilmReferenceContext _context;

        public Genre()
        { }

        public Genre(FilmReferenceContext context)
        {
            _context = context;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            var duplicates = _context.Genre
                .Where(
                    g =>
                        g.Name.ToLower().Replace(" ", "") == Name.ToLower().Replace(" ", ""));
            if (GenreId > 0) // It's an edit
            {
                duplicates = duplicates.Where(g => g.GenreId != GenreId);
            }
            if (duplicates.Any())
            {
                results.Add(new ValidationResult(
                    "A Genre with this name already exists in the database",
                    new List<string>
                    {
                        nameof(Name)
                    }));
            }
            return results;
        }
    }
}
