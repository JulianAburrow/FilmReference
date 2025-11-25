
namespace FilmReferenceDataAccess.Handlers;

public class FilmHandler(FilmReferenceContext context) : IFilmHandler
{
    private readonly FilmReferenceContext _context = context;

    public async Task CreateFilmAsync(FilmModel film, IEnumerable<int> selectedActorIds, bool saveChanges)
    {
        var filmToAdd = new FilmModel
        {
            Name = film.Name,
            Description = film.Description,
            StudioId = film.StudioId,
            DirectorId = film.DirectorId,
            GenreId = film.GenreId,
            BoxCover = film.BoxCover,
        };

        _context.Films.Add(filmToAdd);

        if (saveChanges)
        {
            await SaveChangesAsync();
        }

        if (selectedActorIds != null && selectedActorIds.Any())
        {
            foreach (var selectedActorId in selectedActorIds)
            {
                _context.FilmPeople.Add(new FilmPersonModel
                {
                    FilmId = filmToAdd.FilmId,
                    PersonId = selectedActorId,
                });
            }

            if (saveChanges)
            {
                await SaveChangesAsync();
            }
        }
    }

    public async Task DeleteFilmAsync(int filmId, bool saveChanges)
    {
        var filmToDelete = _context.Films
            .Include(f => f.FilmPerson)
            .FirstOrDefault(f => f.FilmId == filmId);
        if (filmToDelete is null)
        {
            return;
        }
        if (filmToDelete.FilmPerson?.Count > 0)
        {
            foreach (var filmPerson in filmToDelete.FilmPerson)
            {
                _context.Remove(filmPerson);
            }
        }
        _context.Films.Remove(filmToDelete);
        if (saveChanges)
        {
            await SaveChangesAsync();
        }
    }

    public async Task<FilmModel> GetFilmAsync(int filmId)
    {
        var film = await _context.Films
            .Include(f => f.Studio)
            .Include(f => f.Director)
            .Include(f => f.Genre)
            .Include(f => f.FilmPerson)
                .ThenInclude(fp => fp.Person)
            .AsNoTracking()
            .SingleOrDefaultAsync(f => f.FilmId == filmId);

        if (film?.FilmPerson != null)
        {
            film.FilmPerson = film.FilmPerson
                .OrderBy(fp => fp.Person?.FirstName ?? string.Empty)
                .ToList();
        }

        return film ?? new FilmModel();
    }

    public async Task<List<FilmModel>> GetAllFilmsAsync() =>
        await _context.Films
            .AsNoTracking()
            .OrderBy(f => f.Name)
            .Select(f => new FilmModel
            {
                FilmId = f.FilmId,
                Name = f.Name,
                Description = f.Description,
                BoxCover = f.BoxCover,
                GenreId = f.GenreId,
                Studio = new StudioModel { StudioId = f.StudioId, Name = f.Studio.Name },
                Genre = new GenreModel { GenreId = f.GenreId, Name = f.Genre.Name }
            })
            .ToListAsync();

    public async Task<List<FilmModel>> GetFilmsByGenreAsync(int genreId) =>
        await _context.Films
            .Include(f => f.Studio)
            .Include(f => f.Director)
            .Include(f => f.Genre)
        .Where(f => f.GenreId == genreId)
        .OrderBy(f => f.Name)
        .AsNoTracking()
        .ToListAsync();

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();

    public async Task UpdateFilmAsync(FilmModel film, IEnumerable<int> selectedActorIds, bool saveChanges)
    {
        var filmToUpdate = _context.Films
            .FirstOrDefault(f => f.FilmId == film.FilmId);
        if (filmToUpdate is null)
        {
            return;
        }
        filmToUpdate.Name = film.Name;
        filmToUpdate.Description = film.Description;
        filmToUpdate.StudioId = film.StudioId;
        filmToUpdate.DirectorId = film.DirectorId;
        filmToUpdate.GenreId = film.GenreId;
        filmToUpdate.BoxCover = film.BoxCover;

        if (saveChanges)
        {
            await SaveChangesAsync();
        }

        if (selectedActorIds != null && selectedActorIds.Any())
        {
            _context.FilmPeople.RemoveRange(_context.FilmPeople.Where(fp => fp.FilmId == filmToUpdate.FilmId));
            foreach (var selectedActorId in selectedActorIds)
            {
                _context.FilmPeople.Add(new FilmPersonModel
                {
                    FilmId = filmToUpdate.FilmId,
                    PersonId = selectedActorId,
                });
            }
            if (saveChanges)
            {
                await SaveChangesAsync();
            }
        }
    }
}
