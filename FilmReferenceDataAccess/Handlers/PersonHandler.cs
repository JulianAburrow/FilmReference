namespace FilmReferenceDataAccess.Handlers;

public class PersonHandler(FilmReferenceContext context) : IPersonHandler
{
    private readonly FilmReferenceContext _context = context;

    private static readonly Random _random = new();

    public async Task CreatePersonAsync(PersonModel person, bool saveChanges)
    {
        _context.People.Add(person);
        if (saveChanges)
        {
            await SaveChangesAsync();
        }
    }

    public async Task DeletePersonAsync(int personId, bool saveChanges)
    {
        var personToDelete = _context.People
            .FirstOrDefault(p => p.PersonId == personId);
        if (personToDelete is null)
        {
            return;
        }
        _context.People.Remove(personToDelete);
        if (saveChanges)
        {
            await SaveChangesAsync();
        }
    }

    public async Task<RandomPersonModel?> GetRandomPersonAsync()
    {
        var castQuery = _context.People
            .Include(p => p.FilmPerson)
            .Where(p => p.IsCastMember && p.Picture != null);

        var castCount = await castQuery.CountAsync();

        if (castCount == 0)
            return null;

        var randomIndex = _random.Next(castCount);

        var person = await castQuery
            .Skip(randomIndex)
            .FirstOrDefaultAsync();

        if (person is null)
            return null;

        return new RandomPersonModel
        {
            PersonId = person.PersonId,
            FirstName = person.FirstName,
            LastName = person.LastName,
            FilmCount = person.FilmPerson.Count,
            Picture = person.Picture
        };
    }

    public async Task<List<PersonModel>> GetCastMembersAsync() =>
        await _context.People
            .Include(p =>p.Films)
            .Include(p => p.FilmPerson)
            .Where(p => p.IsCastMember)
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .AsNoTracking()
            .ToListAsync();

    public async Task<List<PersonModel>> GetDirectorsAsync() =>
        await _context.People
            .Include(p => p.Films)
            .Include(p => p.FilmPerson)
            .Where(p => p.IsDirector)
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .AsNoTracking()
            .ToListAsync();

    public async Task<PersonModel> GetPersonAsync(int personId)
    {
        var person = await _context.People
            .Include(p => p.FilmPerson)
                .ThenInclude(fp => fp.Film)
                    .ThenInclude(f => f.Genre)
            .Include(p => p.FilmPerson)
                .ThenInclude(fp => fp.Film)
                    .ThenInclude(f => f.Studio)
            .Include(p => p.Films)
                .ThenInclude(f => f.Studio)
            .Include(p => p.Films)
                .ThenInclude(f => f.Genre)
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.PersonId == personId);

        person?.FilmPerson = person.FilmPerson
                .OrderBy(fp => fp.Film.Name)
                .ToList();

        return person ?? new PersonModel();
    }

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public async Task UpdatePersonAsync(PersonModel person, bool saveChanges)
    {
        var personToUpdate = _context.People
            .Where(p => p.PersonId == person.PersonId)
            .FirstOrDefault();
        if (personToUpdate is null)
        {
            return;
        }
        personToUpdate.FirstName = person.FirstName;
        personToUpdate.LastName = person.LastName;
        personToUpdate.Description = person.Description;
        personToUpdate.IsCastMember = person.IsCastMember;
        personToUpdate.IsDirector = person.IsDirector;
        personToUpdate.DateOfBirth = person.DateOfBirth;
        personToUpdate.DateOfDeath = person.DateOfDeath;
        personToUpdate.Picture = person.Picture;

        if (saveChanges)
        {
            await SaveChangesAsync();
        }
    }
}
