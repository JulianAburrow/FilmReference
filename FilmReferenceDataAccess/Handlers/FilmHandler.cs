namespace FilmReferenceDataAccess.Handlers;

public class FilmHandler(FilmReferenceContext context) : IFilmHandler
{
    private readonly FilmReferenceContext _context = context;

    public async Task CreateFilmAsync(FilmModel film, bool saveChanges)
    {
        _context.Films.Add(film);
        if (saveChanges)
        {
            await SaveChangesAsync();
        }
    }

    public async Task DeleteFilmAsync(int filmId, bool saveChanges)
    {
        var filmToDelete = _context.Films
            .FirstOrDefault(f => f.FilmId == filmId);
        if (filmToDelete == null)
        {
            return;
        }
        _context.Films.Remove(filmToDelete);
        if (saveChanges)
        {
            await SaveChangesAsync();
        }
        
    }

    public async Task<FilmModel> GetFilmModelAsync(int filmId) =>
        await _context.Films
            .AsNoTracking()
            .SingleOrDefaultAsync(f => f.FilmId == filmId);

    public async Task<List<FilmModel>> GetFilmsAsync() =>
        await _context.Films
            .OrderBy(f => f.Name)
            .AsNoTracking()
            .ToListAsync();

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public async Task UpdateFilmAsync(FilmModel film, bool saveChanges)
    {
        var filmToUpdate = _context.Films
            .Where(f => f.FilmId == film.FilmId)
            .FirstOrDefault();
        if (filmToUpdate == null)
        {
            return;
        }
        filmToUpdate.Name = film.Name;
        filmToUpdate.Description = film.Description;
        filmToUpdate.Picture = film.Picture;
        filmToUpdate.StudioId = film.StudioId;
        filmToUpdate.DirectorId = film.DirectorId;
        filmToUpdate.FilmPerson = film.FilmPerson;

        if (saveChanges)
        {
            await SaveChangesAsync();
        }
    }
}
