namespace FilmReferenceDataAccess.Handlers;

public class FilmHandler(IDbContextFactory<FilmReferenceContext> factory) : IFilmHandler
{
    public async Task CreateFilmAsync(FilmModel film, IEnumerable<int> selectedCastMemberIds)
    {
        await using var context = await factory.CreateDbContextAsync();
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        
        var filmToAdd = new FilmModel
        {
            Name = film.Name,
            Description = film.Description,
            StudioId = film.StudioId,
            DirectorId = film.DirectorId,
            GenreId = film.GenreId,
            BoxCover = film.BoxCover,
        };

        context.Films.Add(filmToAdd);

        await context.SaveChangesAsync();

        film.FilmId = filmToAdd.FilmId;

        if (selectedCastMemberIds != null && selectedCastMemberIds.Any())
        {
            foreach (var selectedCastMemberId in selectedCastMemberIds)
            {
                context.FilmPeople.Add(new FilmPersonModel
                {
                    FilmId = filmToAdd.FilmId,
                    PersonId = selectedCastMemberId,
                });
            }

            await context.SaveChangesAsync();            
        }

        scope.Complete();        
    }

    public async Task DeleteFilmAsync(int filmId)
    {
        await using var context = await factory.CreateDbContextAsync();

        var filmToDelete = await context.Films
            .Include(f => f.FilmPerson)
            .FirstOrDefaultAsync(f => f.FilmId == filmId);
        if (filmToDelete is null)
        {
            return;
        }

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        if (filmToDelete.FilmPerson?.Count > 0)
        {
            foreach (var filmPerson in filmToDelete.FilmPerson)
            {
                context.Remove(filmPerson);
            }
        }

        var favourite = await context.Favourites
            .FirstOrDefaultAsync(f =>
                                    f.EntityTypeId == (int)FavouriteEntityEnum.Film &&
                                    f.EntityId == filmId);
        if (favourite is not null)
        {
            context.Remove(favourite);
        }

        context.Films.Remove(filmToDelete);
        
        await context.SaveChangesAsync();

        scope.Complete();
    }

    public async Task<FilmModel> GetFilmAsync(int filmId)
    {
        await using var context = await factory.CreateDbContextAsync();

        var film = await context.Films
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

    public async Task<List<FilmModel>> GetAllFilmsAsync()
    {
        await using var context = await factory.CreateDbContextAsync();

        return await context.Films
            .AsNoTracking()
            .OrderBy(f => f.Name)
            .Select(f => new FilmModel
            {
                FilmId = f.FilmId,
                Name = f.Name,
                Description = f.Description,
                BoxCover = f.BoxCover,
                GenreId = f.GenreId,
                StudioId = f.StudioId,
                Studio = new StudioModel { StudioId = f.StudioId, Name = f.Studio.Name },
                Genre = new GenreModel { GenreId = f.GenreId, Name = f.Genre.Name }
            })
            .ToListAsync();
    }

    public async Task UpdateFilmAsync(FilmModel film, IEnumerable<int> selectedCastMemberIds)
    {
        using var context = await factory.CreateDbContextAsync();

        var filmToUpdate = await context.Films
            .FirstOrDefaultAsync(f => f.FilmId == film.FilmId);

        if (filmToUpdate is null)
            return;

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        
        filmToUpdate.Name = film.Name;
        filmToUpdate.Description = film.Description;
        filmToUpdate.StudioId = film.StudioId;
        filmToUpdate.DirectorId = film.DirectorId;
        filmToUpdate.GenreId = film.GenreId;
        filmToUpdate.BoxCover = film.BoxCover;

        context.FilmPeople.RemoveRange(
            context.FilmPeople.Where(fp => fp.FilmId == filmToUpdate.FilmId));

        await context.SaveChangesAsync();

        if (selectedCastMemberIds is not null && selectedCastMemberIds.Any())
        {            
            foreach (var selectedCastMemberId in selectedCastMemberIds)
            {
                context.FilmPeople.Add(new FilmPersonModel
                {
                    FilmId = filmToUpdate.FilmId,
                    PersonId = selectedCastMemberId,
                });
            }
            
            await context.SaveChangesAsync();
        }

        scope.Complete();
    }
}
