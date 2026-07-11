namespace FilmReferenceDataAccess.Handlers;

public class GenreHandler(IDbContextFactory<FilmReferenceContext> factory) : IGenreHandler
{
    public async Task CreateGenreAsync(GenreModel genre)
    {
        await using var context = await factory.CreateDbContextAsync();

        context.Genres.Add(genre);        
        await context.SaveChangesAsync();
    }

    public async Task DeleteGenreAsync(int genreId)
    {
        await using var context = await factory.CreateDbContextAsync();

        var genreToDelete = await context.Genres
            .FirstOrDefaultAsync(g => g.GenreId == genreId);
        if (genreToDelete is null)
        {
            return;
        }
        context.Genres.Remove(genreToDelete);
        await context.SaveChangesAsync();
    }

    public async Task<GenreModel> GetGenreAsync(int genreId)
    {
        await using var context = await factory.CreateDbContextAsync();

        var genre = await context.Genres
            .Include(g => g.Films)
                .ThenInclude(f => f.Studio)
            .AsNoTracking()
            .SingleOrDefaultAsync(g => g.GenreId == genreId);

        genre?.Films = genre.Films
            .OrderBy(f => f.Name)
            .ToList();

        return genre ?? new GenreModel();
    }        

    public async Task<List<GenreModel>> GetGenresAsync()
    {
        await using var context = await factory.CreateDbContextAsync();

        return await context.Genres
            .Include(g => g.Films)
            .OrderBy(g => g.Name)
        .AsNoTracking()
        .ToListAsync();
    }

    public async Task<List<GenreModelLightweight>> GetGenresLightweightAsync()
    {
        await using var context = await factory.CreateDbContextAsync();
        return await context.Genres
            .OrderBy(g => g.Name)
            .Select(g => new GenreModelLightweight
            {
                GenreId = g.GenreId,
                Name = g.Name
            })
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task UpdateGenreAsync(GenreModel genre)
    {
        await using var context = await factory.CreateDbContextAsync();

        var genreToUpdate = await context.Genres
            .FirstOrDefaultAsync(g => g.GenreId == genre.GenreId);

        if (genreToUpdate is null)
        {
            return;
        }
        genreToUpdate.Name = genre.Name;
        genreToUpdate.Description = genre.Description;
        genreToUpdate.Logo = genre.Logo;

        await context.SaveChangesAsync();
    }
}
