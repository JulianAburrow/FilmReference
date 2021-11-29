using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FilmReference.DataAccess
{
    [ModelMetadataType(typeof(FilmModelMetadata))]
    public partial class Film : IValidatableObject
    {
        private readonly FilmReferenceContext _context;

        public Film(FilmReferenceContext context)
        {
            _context = context;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            var duplicates = _context.Film
                .Where(
                    f =>
                        f.Name.ToLower().Replace(" ", "") == Name.ToLower().Replace(" ", ""));
            if (FilmId > 0) // It's an update
            {
                duplicates = duplicates
                    .Where(
                        f =>
                            f.FilmId != FilmId);
            }
            if (duplicates.Any())
            {
                results.Add(new ValidationResult(
                    "A Film with this name already exists in the database",
                    new List<string>
                    {
                        "Film.Name"
                    }));
            }

            return results;
        }
    }
}
