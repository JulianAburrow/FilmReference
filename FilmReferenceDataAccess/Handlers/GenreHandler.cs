namespace FilmReferenceDataAccess.Handlers;

public class GenreHandler(FilmReferenceContext context) : IGenreHandler
{
    private readonly FilmReferenceContext _context = context;

    public async Task CreateGenreAsync(GenreModel genre, bool saveChanges)
    {
        _context.Genres.Add(genre);
        if (saveChanges)
        {
            await SaveChangesAsync();
        }
    }

    public async Task DeleteGenreAsync(int genreId, bool saveChanges)
    {
        var genreToDelete = _context.Genres
            .FirstOrDefault(g => g.GenreId == genreId);
        if (genreToDelete == null)
        {
            return;
        }
        _context.Genres.Remove(genreToDelete);
        if (saveChanges)
        {
            await SaveChangesAsync();
        }
    }

    public async Task<GenreModel> GetGenreAsync(int genreId) =>
        await _context.Genres
            .Include(g => g.Films)
        .AsNoTracking()
        .SingleOrDefaultAsync(g => g.GenreId == genreId);

    public async Task<List<GenreModel>> GetGenresAsync() =>
        await _context.Genres
            .Include(g => g.Films)
            .OrderBy(g => g.Name)
        .AsNoTracking()
        .ToListAsync();

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public async Task UpdateGenreAsync(GenreModel genre, bool saveChanges)
    {
        var genreToUpdate = _context.Genres
            .FirstOrDefault(g => g.GenreId == genre.GenreId);
        if (genreToUpdate == null)
        {
            return;
        }
        genreToUpdate.Name = genre.Name;
        genreToUpdate.Description = genre.Description;
        
        if (saveChanges)
        {
            await SaveChangesAsync();
        }
    }
}
