using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FilmReference.DataAccess
{
    [ModelMetadataType(typeof(PersonModelMetadata))]
    public partial class Person : IValidatableObject
    {
        private readonly FilmReferenceContext _context;

        public Person()
        {}

        public Person(FilmReferenceContext context)
        {
            _context = context;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            // Must be either a director or an actor
            if (IsActor == false && IsDirector == false)
            {
                results.Add(new ValidationResult(
                    "Must be either an actor or a director",
                    new List<string>
                    {
                        nameof(IsActor)
                    }));
            }

            var fullName = FirstName.ToLower().Replace(" ", "");
            if (!string.IsNullOrWhiteSpace(LastName))
            {
                fullName = $"{fullName}{LastName.ToLower().Replace(" ", "")}";
            }
            var duplicates = _context.Person
                .Where(
                    p =>
                        p.FullName.ToLower().Replace(" ", "") == fullName);

            if (PersonId > 0) // It's an update
            {
                duplicates = duplicates.Where(p => p.PersonId != PersonId);
            }
            if (duplicates.Any())
            {
                results.Add(new ValidationResult(
                    "A Person with this first and last name already exists in the database",
                    new List<string>
                    {
                        nameof(LastName)
                    }));
            }

            return results;
        }
    }
}
