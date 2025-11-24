
namespace FilmReferenceDataAccess.Handlers;

public class SearchHandler(FilmReferenceContext context) : ISearchHandler
{
    private readonly FilmReferenceContext _context = context;

    public async Task<List<FilmModel>> SearchFilmsAsync(string searchText) =>
        await _context.Films
            .Where(f =>
                f.Name.Contains(searchText) ||
                f.Description.Contains(searchText))
            .AsNoTracking()
            .ToListAsync();

    public async Task<List<GenreModel>> SearchGenresAsync(string searchText) =>
        await _context.Genres
            .Where(g =>
                g.Name.Contains(searchText) ||
                (g.Description != null && g.Description.Contains(searchText)))
            .AsNoTracking()
            .ToListAsync();

    public async Task<List<PersonModel>> SearchPeopleAsync(string searchText) =>
        await _context.People
            .Where(p =>
                p.FirstName.Contains(searchText) ||
                (p.LastName != null && p.LastName.Contains(searchText)) ||
                (p.Description != null && p.Description.Contains(searchText)))
            .AsNoTracking()
            .ToListAsync();

    public async Task<List<StudioModel>> SearchStudiosAsync(string searchText) =>
        await _context.Studios
            .Where(s =>
                s.Name.Contains(searchText) ||
                (s.Description != null && s.Description.Contains(searchText)))
            .AsNoTracking()
            .ToListAsync();
}
