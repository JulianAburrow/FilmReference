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
        if (personToDelete == null)
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
            .Include(p => p.Films)
            .Include(p => p.FilmPerson)
            .OrderBy(p => p.FirstName)
            .AsNoTracking()
            .ToListAsync();

    public async Task<PersonModel> GetPersonAsync(int personId) =>
        await _context.People
            .Include(p => p.Films)
            .Include (p => p.FilmPerson)
            .OrderBy(p => p.FirstName)
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.PersonId == personId);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public async Task UpdatePersonAsync(PersonModel person, bool saveChanges)
    {
        var personToUpdate = _context.People
            .Where(p => p.PersonId == person.PersonId)
            .FirstOrDefault();
        if (personToUpdate == null)
        {
            return;
        }
        personToUpdate.FirstName = person.FirstName;
        personToUpdate.LastName = person.LastName;
        personToUpdate.Description = person.Description;
        personToUpdate.IsActor = person.IsActor;
        personToUpdate.IsDirector = person.IsDirector;
        personToUpdate.PictureName = person.PictureName;

        if (saveChanges)
        {
            await SaveChangesAsync();
        }
    }
}
