
namespace FilmReferenceDataAccess.Handlers;

public class SearchHandler(FilmReferenceContext context) : ISearchHandler
{
    private readonly FilmReferenceContext _context = context;

    public async Task<List<FilmModel>> SearchFilmsAsync(string searchText) =>
        await _context.Films
            .Include(f => f.Genre)
            .Include(f => f.Studio)
            .Where(f =>
                f.Name.Contains(searchText) ||
                f.Description.Contains(searchText))
            .OrderBy(f => f.Name)
            .AsNoTracking()
            .ToListAsync();

    public async Task<List<GenreModel>> SearchGenresAsync(string searchText) =>
        await _context.Genres
            .Include(g => g.Films)
            .Where(g =>
                g.Name.Contains(searchText) ||
                (g.Description != null && g.Description.Contains(searchText)))
            .OrderBy(g => g.Name)
            .AsNoTracking()
            .ToListAsync();

    public async Task<List<PersonModel>> SearchPeopleAsync(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return [];

        var parts = searchText
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Trim())
            .ToArray();

        IQueryable<PersonModel> query = _context.People;

        foreach (var part in parts)
        {
            var p = part; // avoid modified closure

            query = query.Where(person =>
                EF.Functions.Like(person.FirstName, $"%{p}%") ||
                EF.Functions.Like(person.LastName, $"%{p}%") ||
                EF.Functions.Like(person.Description, $"%{p}%")
            );
        }

        return await query
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<StudioModel>> SearchStudiosAsync(string searchText) =>
        await _context.Studios
            .Include(s => s.Films)
            .Where(s =>
                s.Name.Contains(searchText) ||
                (s.Description != null && s.Description.Contains(searchText)))
            .OrderBy(s => s.Name)
            .AsNoTracking()
            .ToListAsync();
}
