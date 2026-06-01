

namespace FilmReferenceDataAccess.Handlers;

public class FavouriteHandler(FilmReferenceContext context) : IFavouriteHandler
{
    private readonly FilmReferenceContext _context = context;

    public async Task CreateFavouriteAsync(FavouriteModel favourite, bool saveChanges)
    {
        if (_context.Favourites.Any(f => f.EntityTypeId == favourite.EntityTypeId && f.EntityId == favourite.EntityId))
        {
            return;
        }

        _context.Favourites.Add(favourite);
        if (saveChanges)
        {
            await SaveChangesAsync();
        }
    }

    public async Task DeleteFavouriteAsync(int entityTypeId, int entityId, bool saveChanges)
    {
        var favouriteToDelete = await _context.Favourites
            .FirstOrDefaultAsync(f => f.EntityTypeId == entityTypeId
                                   && f.EntityId == entityId);

        if (favouriteToDelete is null)
            return;

        _context.Favourites.Remove(favouriteToDelete);

        if (saveChanges)
            await SaveChangesAsync();
    }

    public async Task<List<FavouriteDisplayModel>> GetAllFavouritesAsync()
    {
        var favourites = await _context.Favourites
            .OrderBy(f => f.EntityTypeId)
            .AsNoTracking()
            .ToListAsync();

        var favouriteDisplayModels = new List<FavouriteDisplayModel>();
        
        var grouped = favourites
                        .GroupBy(f => f.EntityTypeId)
                        .OrderBy(g => g.Key);

        foreach (var group in grouped)
        {
            var entityTypeId = group.Key;
            var entityIds = group.Select(f => f.EntityId).ToList();

            Dictionary<int, (string Name, byte[]? Image)> lookup;

            switch (entityTypeId)
            {
                case (int)FavouriteEntityEnum.Film:
                    lookup = await _context.Films
                        .Where(f => entityIds.Contains(f.FilmId))
                        .ToDictionaryAsync(
                            f => f.FilmId,
                            f => (Name: f.Name, Image: f.BoxCover)
                        );
                    break;

                case (int)FavouriteEntityEnum.Genre:
                    lookup = await _context.Genres
                        .Where(g => entityIds.Contains(g.GenreId))
                        .ToDictionaryAsync(
                            g => g.GenreId,
                            g => (Name: g.Name, Image: (byte[]?)null)
                        );
                    break;

                case (int)FavouriteEntityEnum.Person:
                    lookup = await _context.People
                        .Where(p => entityIds.Contains(p.PersonId))
                        .ToDictionaryAsync(
                            p => p.PersonId,
                            p => (Name: p.FirstName + " " + p.LastName, Image: p.Picture)
                        );
                    break;

                case (int)FavouriteEntityEnum.Studio:
                    lookup = await _context.Studios
                        .Where(s => entityIds.Contains(s.StudioId))
                        .ToDictionaryAsync(
                            s => s.StudioId,
                            s => (Name: s.Name, Image: s.Logo)
                        );
                    break;

                default:
                    continue;
            }

            foreach (var favourite in group.OrderBy(f => lookup[f.EntityId].Name))
            {
                if (!lookup.TryGetValue(favourite.EntityId, out var details))
                {
                    continue;
                }

                favouriteDisplayModels.Add(new FavouriteDisplayModel
                {
                    FavouriteId = favourite.FavouriteId,
                    EntityTypeId = favourite.EntityTypeId,
                    EntityTypeName = Enum.GetName(typeof(FavouriteEntityEnum), favourite.EntityTypeId) ?? "Unknown",
                    EntityId = favourite.EntityId,
                    EntityName = details.Name,
                    EntityImage = details.Image
                });
            }
        }

        return favouriteDisplayModels;
        
    }

    public async Task<bool> IsFavouriteAsync(int entityTypeId, int entityId)
    {
        return _context.Favourites.Any(f => f.EntityTypeId == entityTypeId && f.EntityId == entityId);
    }

    public Task SaveChangesAsync() =>
        _context.SaveChangesAsync();
}
