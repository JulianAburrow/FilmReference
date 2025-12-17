namespace FilmReferenceDataAccess.Handlers;

public class PersonHandler(FilmReferenceContext context) : IPersonHandler
{
    private readonly FilmReferenceContext _context = context;

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

    public async Task<List<PersonModel>> GetPeopleAsync() =>
        await _context.People
            .AsNoTracking()
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .Select(p => new PersonModel
            {
                PersonId = p.PersonId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Description = p.Description,
                IsCastMember = p.IsCastMember,
                IsDirector = p.IsDirector,
                Picture = p.Picture,
                Films = p.Films.Select(f => new FilmModel { FilmId = f.FilmId, Name = f.Name }).ToList()
            })
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
        personToUpdate.Picture = person.Picture;

        if (saveChanges)
        {
            await SaveChangesAsync();
        }
    }
}
