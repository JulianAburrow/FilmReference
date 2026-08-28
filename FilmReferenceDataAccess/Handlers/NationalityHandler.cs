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
}
