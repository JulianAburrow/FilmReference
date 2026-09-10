namespace FilmReferenceDataAccess.Handlers;

public class NationalityHandler(IDbContextFactory<FilmReferenceContext> factory) : INationalityHandler
{
    public async Task<List<NationalityModel>> GetNationalitiesAsync()
    {
        await using var context = await factory.CreateDbContextAsync();

        return await context.Nationalities
            .AsNoTracking()
            .OrderBy(n => n.Name)
            .ToListAsync();
    }

    public async Task<List<NationalityListModel>> GetNationalitiesInUseForCastMembersAsync()
    {
        await using var context = await factory.CreateDbContextAsync();

        return await context.Nationalities
            .AsNoTracking()
            .Where(n => n.People.Any() && n.People.Any(p => p.IsCastMember))
            .Select(n => new NationalityListModel
            {
                NationalityId = n.NationalityId,
                Name = n.Name,
                IsoCode = n.IsoCode,
                PersonCount = n.People.Count,
            })
            .OrderBy(n => n.Name)
            .ToListAsync();
    }

    public async Task<List<NationalityListModel>> GetNationalitiesInUseForDirectorsAsync()
    {
        await using var context = await factory.CreateDbContextAsync();

        return await context.Nationalities
            .AsNoTracking()
            .Where(n => n.People.Any() && n.People.Any(p => p.IsDirector))
            .Select(n => new NationalityListModel
            {
                NationalityId = n.NationalityId,
                Name = n.Name,
                IsoCode = n.IsoCode,
                PersonCount = n.People.Count,
            })
            .OrderBy(n => n.Name)
            .ToListAsync();
    }
}
