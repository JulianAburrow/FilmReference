
namespace FilmReferenceDataAccess.Handlers;

public class SearchHandler(FilmReferenceContext context) : ISearchHandler
{
    private readonly FilmReferenceContext _context = context;

    private const string collation = "SQL_Latin1_General_CP1_CI_AI";

    public async Task<List<FilmModel>> SearchFilmsAsync(string searchText) =>
        await _context.Films
            .Include(f => f.Genre)
            .Include(f => f.Studio)
            .Where(f => EF.Functions.Collate(f.Name, collation)
                .Contains(searchText))
            .OrderBy(f => f.Name)
            .AsNoTracking()
            .ToListAsync();

    public async Task<List<GenreModel>> SearchGenresAsync(string searchText) =>
        await _context.Genres
            .Include(g => g.Films)
            .Where(g => EF.Functions.Collate(g.Name, collation)
                .Contains(searchText))
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
                EF.Functions.Collate(person.FirstName, collation).Contains(p) ||
                (person.LastName != null &&
                    EF.Functions.Collate(person.LastName, collation).Contains(p))

            );
        }

        return await query
            .Include(p => p.Films)
            .Include(p => p.FilmPerson)
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<StudioModel>> SearchStudiosAsync(string searchText) =>
        await _context.Studios
            .Include(s => s.Films)
            .Where(s => EF.Functions.Collate(s.Name, collation)
                .Contains(searchText))
            .OrderBy(s => s.Name)
            .AsNoTracking()
            .ToListAsync();
}
