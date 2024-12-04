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
        if (studioToDelete == null)
        {
            return;
        }
        _context.Studios.Remove(studioToDelete);
        if (saveChanges)
        {
            await SaveChangesAsync();
        }
    }

    public async Task<StudioModel> GetStudioAsync(int studioId) =>
        await _context.Studios
            .Include(s => s.Films)
            .AsNoTracking()
            .SingleOrDefaultAsync(s => s.StudioId == studioId);

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
        if (studioToUpdate == null)
        {
            return;
        }
        studioToUpdate.Name = studio.Name;
        studioToUpdate.Description = studio.Description;
        studioToUpdate.PictureName = studio.PictureName;

        if (saveChanges)
        {
            await SaveChangesAsync();
        }
    }
}
