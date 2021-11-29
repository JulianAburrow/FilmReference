using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FilmReference.DataAccess
{
    [ModelMetadataType(typeof(StudioModelMetadata))]
    public partial class Studio : IValidatableObject
    {
        private readonly FilmReferenceContext _context;

        public Studio()
        { }

        public Studio(FilmReferenceContext context)
        {
            _context = context;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            var duplicates = _context.Studio
                .Where(
                    s =>
                        s.Name.ToLower().Replace(" ", "") == Name.ToLower().Replace(" ", ""));
            if (StudioId > 0) // It's an edit
            {
                duplicates = duplicates.Where(s => s.StudioId != StudioId);
            }

            if (duplicates.Any())
            {
                results.Add(new ValidationResult(
                    "A Studio with this name already exists in the database",
                    new List<string>
                    {
                            nameof(Name)
                    }));
            }

            return results;
        }
    }
}
