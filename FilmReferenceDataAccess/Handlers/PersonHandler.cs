namespace FilmReferenceDataAccess.Handlers;

public class PersonHandler(IDbContextFactory<FilmReferenceContext> factory) : IPersonHandler
{
    private static readonly Random _random = new();

    public async Task CreatePersonAsync(PersonModel person)
    {
        await using var context = await factory.CreateDbContextAsync();
        context.People.Add(person);
        
        await context.SaveChangesAsync();
    }

    public async Task DeletePersonAsync(int personId)
    {
        await using var context = await factory.CreateDbContextAsync();

        var personToDelete = await context.People
            .FirstOrDefaultAsync(p => p.PersonId == personId);

        if (personToDelete is null)
        {
            return;
        }

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var favourite = await context.Favourites
            .FirstOrDefaultAsync(f =>
                                    f.EntityTypeId == (int)FavouriteEntityEnum.Person &&
                                    f.EntityId == personId);
        if (favourite is not null)
        {
            context.Remove(favourite);
        }

        context.People.Remove(personToDelete);
        await context.SaveChangesAsync();

        scope.Complete();
    }

    public async Task<FeaturedPersonModel> GetFeaturedPersonAsync()
    {
        await using var context = await factory.CreateDbContextAsync();
        var castQuery = context.People
            .Include(p => p.FilmPerson)
            .Where(p => p.IsCastMember && p.Picture != null);

        var castCount = await castQuery.CountAsync();

        if (castCount == 0)
            return new FeaturedPersonModel();

        var randomIndex = _random.Next(castCount);

        var person = await castQuery
            .Skip(randomIndex)
            .FirstOrDefaultAsync();

        if (person is null)
            return new FeaturedPersonModel();

        return new FeaturedPersonModel
        {
            PersonId = person.PersonId,
            FirstName = person.FirstName,
            LastName = person.LastName,
            Description = person.Description,
            FilmCount = person.FilmPerson.Count,
            Picture = person.Picture
        };
    }

    public async Task<List<PersonModel>> GetCastMembersAsync()
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.People
            .Include(p => p.Films)
            .Include(p => p.FilmPerson)
            .Where(p => p.IsCastMember)
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .AsNoTracking()
            .ToListAsync();
    }        

    public async Task<List<PersonModel>> GetDirectorsAsync()
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.People
            .Include(p => p.Films)
            .Include(p => p.FilmPerson)
            .Where(p => p.IsDirector)
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<PersonModel> GetPersonAsync(int personId)
    {
        await using var context = await factory.CreateDbContextAsync();
        var person = await context.People
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
            .AsSplitQuery()
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.PersonId == personId);

        person?.FilmPerson = person.FilmPerson
                .OrderBy(fp => fp.Film.Name)
                .ToList();

        return person ?? new PersonModel();
    }

    public async Task UpdatePersonAsync(PersonModel person)
    {
        await using var context = await factory.CreateDbContextAsync();
        var personToUpdate = await context.People
            .Where(p => p.PersonId == person.PersonId)
            .FirstOrDefaultAsync();
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

        await context.SaveChangesAsync();
    }

    public async Task<List<PersonModelLightweight>> GetCastMembersLightweightAsync()
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.People
            .Where(p => p.IsCastMember)
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .Select(p => new PersonModelLightweight
            {
                PersonId = p.PersonId,
                FirstName = p.FirstName,
                LastName = p.LastName
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<PersonModelLightweight>> GetDirectorsLightweightAsync()
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.People
            .Where(p => p.IsDirector)
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .Select(p => new PersonModelLightweight
            {
                PersonId = p.PersonId,
                FirstName = p.FirstName,
                LastName = p.LastName
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<PersonModel>> GetBirthdaysForDateAsync(int day, int month)
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.People
            .Where(p =>
                p.IsCastMember &&
                p.DateOfBirth.HasValue &&
                (
                    (p.DateOfBirth.Value.Month == month &&
                     p.DateOfBirth.Value.Day == day)
                )
            )
            .Include(p => p.FilmPerson)
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .AsNoTracking()
            .ToListAsync();
    }
}
