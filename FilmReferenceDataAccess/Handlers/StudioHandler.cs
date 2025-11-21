namespace FilmReferenceDataAccess.Handlers;

public class StudioHandler(FilmReferenceContext context) : IStudioHandler
{
    private readonly FilmReferenceContext _context = context;

    public async Task CreateStudioAsync(StudioModel studio, bool saveChanges)
    {
        _context.Studios.Add(studio);
        if (saveChanges)
        {
            await SaveChangesAsync();
        }
    }

    public async Task DeleteStudioAsync(int studioId, bool saveChanges)
    {
        var studioToDelete = _context.Studios
            .FirstOrDefault(s => s.StudioId == studioId);
        if (studioToDelete is null)
        {
            return;
        }
        _context.Studios.Remove(studioToDelete);
        if (saveChanges)
        {
            await SaveChangesAsync();
        }
    }

    public async Task<StudioModel> GetStudioAsync(int studioId)
    {
        var studio = await _context.Studios
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
        

    public async Task<List<StudioModel>> GetStudiosAsync() =>
        await _context.Studios
            .Include(s => s.Films)
            .OrderBy(s => s.Name)
            .AsNoTracking()
            .ToListAsync();        

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public async Task UpdateStudioAsync(StudioModel studio, bool saveChanges)
    {
        var studioToUpdate = _context.Studios
            .Where(s => s.StudioId == studio.StudioId)
            .FirstOrDefault();
        if (studioToUpdate is null)
        {
            return;
        }
        studioToUpdate.Name = studio.Name;
        studioToUpdate.Description = studio.Description;
        studioToUpdate.Logo = studio.Logo;

        if (saveChanges)
        {
            await SaveChangesAsync();
        }
    }
}
