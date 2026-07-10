using Microsoft.EntityFrameworkCore.Infrastructure;

namespace FilmReferenceDataAccess.Handlers;

public class SearchHandler(IDbContextFactory<FilmReferenceContext> factory) : ISearchHandler
{
    private const string collation = "SQL_Latin1_General_CP1_CI_AI";

    private static bool IsInMemory(DatabaseFacade db)
        => db.ProviderName == "Microsoft.EntityFrameworkCore.InMemory";

    // ----------------------------------------------------------------------
    // FILMS
    // ----------------------------------------------------------------------
    public async Task<List<FilmModel>> SearchFilmsAsync(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return [];

        using var _context = await factory.CreateDbContextAsync();

        var term = searchText.Trim();

        IQueryable<FilmModel> query = _context.Films
            .Include(f => f.Genre)
            .Include(f => f.Studio);

        if (!IsInMemory(_context.Database))
        {
            query = query.Where(f =>
                EF.Functions.Collate(f.Name, collation).Contains(term));
        }
        else
        {
            query = query.Where(f =>
                f.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        return await query
            .OrderBy(f => f.Name)
            .AsNoTracking()
            .ToListAsync();
    }

    // ----------------------------------------------------------------------
    // GENRES
    // ----------------------------------------------------------------------
    public async Task<List<GenreModel>> SearchGenresAsync(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return [];

        using var _context = await factory.CreateDbContextAsync();

        var term = searchText.Trim();

        IQueryable<GenreModel> query = _context.Genres
            .Include(g => g.Films);

        if (!IsInMemory(_context.Database))
        {
            query = query.Where(g =>
                EF.Functions.Collate(g.Name, collation).Contains(term));
        }
        else
        {
            query = query.Where(g =>
                g.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        return await query
            .OrderBy(g => g.Name)
            .AsNoTracking()
            .ToListAsync();
    }

    // ----------------------------------------------------------------------
    // PEOPLE
    // ----------------------------------------------------------------------
    public async Task<List<PersonModel>> SearchPeopleAsync(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return [];

        using var _context = await factory.CreateDbContextAsync();

        var parts = searchText
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Trim())
            .ToArray();

        IQueryable<PersonModel> query = _context.People;

        foreach (var part in parts)
        {
            var p = part;

            if (!IsInMemory(_context.Database))
            {
                query = query.Where(person =>
                    EF.Functions.Collate(person.FirstName, collation).Contains(p) ||
                    (person.LastName != null &&
                     EF.Functions.Collate(person.LastName, collation).Contains(p)));
            }
            else
            {
                query = query.Where(person =>
                    person.FirstName.Contains(p, StringComparison.OrdinalIgnoreCase) ||
                    (person.LastName != null &&
                     person.LastName.Contains(p, StringComparison.OrdinalIgnoreCase)));
            }
        }

        return await query
            .Include(p => p.Films)
            .Include(p => p.FilmPerson)
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .AsNoTracking()
            .ToListAsync();
    }

    // ----------------------------------------------------------------------
    // STUDIOS
    // ----------------------------------------------------------------------
    public async Task<List<StudioModel>> SearchStudiosAsync(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return [];

        using var _context = await factory.CreateDbContextAsync();

        var term = searchText.Trim();

        IQueryable<StudioModel> query = _context.Studios
            .Include(s => s.Films);

        if (!IsInMemory(_context.Database))
        {
            query = query.Where(s =>
                EF.Functions.Collate(s.Name, collation).Contains(term));
        }
        else
        {
            query = query.Where(s =>
                s.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        return await query
            .OrderBy(s => s.Name)
            .AsNoTracking()
            .ToListAsync();
    }
}