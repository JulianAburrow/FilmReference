namespace FilmReferenceDataAccess.Handlers;

public class StudioHandler(IDbContextFactory<FilmReferenceContext> factory) : IStudioHandler
{
    public async Task CreateStudioAsync(StudioModel studio)
    {
        await using var context = await factory.CreateDbContextAsync();
        context.Studios.Add(studio);
        await context.SaveChangesAsync();
    }

    public async Task DeleteStudioAsync(int studioId)
    {
        await using var context = await factory.CreateDbContextAsync();

        var studioToDelete = await context.Studios
            .FirstOrDefaultAsync(s => s.StudioId == studioId);

        if (studioToDelete is null)
            return;

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var favourite = await context.Favourites
            .FirstOrDefaultAsync(f =>
                                    f.EntityTypeId == (int)FavouriteEntityEnum.Studio &&
                                    f.EntityId == studioId);
        if (favourite is not null)
        {
            context.Remove(favourite);
        }

        context.Studios.Remove(studioToDelete);
        await context.SaveChangesAsync();

        scope.Complete();
    }

    public async Task<StudioModel> GetStudioAsync(int studioId)
    {
        await using var context = await factory.CreateDbContextAsync();

        var studio = await context.Studios
            .Include(s => s.Films)
                .ThenInclude(f => f.Director)
            .Include(s => s.Films)
                .ThenInclude(f => f.Genre)
            .AsNoTracking()
            .SingleOrDefaultAsync(s => s.StudioId == studioId);

        studio?.Films = studio.Films
            .OrderBy(f => f.Name)
            .ToList();

        return studio ?? new StudioModel();
    }        

    public async Task<List<StudioModel>> GetStudiosAsync()
    {
        await using var context = await factory.CreateDbContextAsync();

        return await context.Studios
            .Include(s => s.Films)
            .OrderBy(s => s.Name)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<StudioModelLightweight>> GetStudiosLightweightAsync()
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.Studios
            .OrderBy(s => s.Name)
            .Select(s => new StudioModelLightweight
            {
                StudioId = s.StudioId,
                Name = s.Name
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateStudioAsync(StudioModel studio)
    {
        await using var context = await factory.CreateDbContextAsync();

        var studioToUpdate = await context.Studios
            .FirstOrDefaultAsync(s => s.StudioId == studio.StudioId);

        if (studioToUpdate is null)
            return;

        studioToUpdate.Name = studio.Name;
        studioToUpdate.Description = studio.Description;
        studioToUpdate.Logo = studio.Logo;

        await context.SaveChangesAsync();
    }
}
