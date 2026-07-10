

namespace FilmReferenceDataAccess.Handlers;

public class FavouriteHandler(IDbContextFactory<FilmReferenceContext> filmReferenceContextFactory) : IFavouriteHandler
{
    public async Task CreateFavouriteAsync(FavouriteModel favourite)
    {
        await using var context = await filmReferenceContextFactory.CreateDbContextAsync();
        if (await context.Favourites.AnyAsync(f => f.EntityTypeId == favourite.EntityTypeId && f.EntityId == favourite.EntityId))
        {
            return;
        }

        context.Favourites.Add(favourite);
        await context.SaveChangesAsync();
    }

    public async Task DeleteFavouriteAsync(int entityTypeId, int entityId)
    {
        await using var context = await filmReferenceContextFactory.CreateDbContextAsync();
        var favouriteToDelete = await context.Favourites
            .FirstOrDefaultAsync(f => f.EntityTypeId == entityTypeId
                                   && f.EntityId == entityId);

        if (favouriteToDelete is null)
            return;

        context.Favourites.Remove(favouriteToDelete);

        await context.SaveChangesAsync();
    }

    public async Task<List<FavouriteDisplayModel>> GetAllFavouritesAsync()
    {
        await using var context = await filmReferenceContextFactory.CreateDbContextAsync();
        var favourites = await context.Favourites
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
                    lookup = await context.Films
                        .Where(f => entityIds.Contains(f.FilmId))
                        .ToDictionaryAsync(
                            f => f.FilmId,
                            f => (Name: f.Name, Image: f.BoxCover)
                        );
                    break;

                case (int)FavouriteEntityEnum.Genre:
                    lookup = await context.Genres
                        .Where(g => entityIds.Contains(g.GenreId))
                        .ToDictionaryAsync(
                            g => g.GenreId,
                            g => (Name: g.Name, Image: g.Logo)
                        );
                    break;

                case (int)FavouriteEntityEnum.Person:
                    lookup = await context.People
                        .Where(p => entityIds.Contains(p.PersonId))
                        .ToDictionaryAsync(
                            p => p.PersonId,
                            p => (Name: p.FirstName + " " + p.LastName, Image: p.Picture)
                        );
                    break;

                case (int)FavouriteEntityEnum.Studio:
                    lookup = await context.Studios
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
        await using var context = await filmReferenceContextFactory.CreateDbContextAsync();
        return await context.Favourites.AnyAsync(f => f.EntityTypeId == entityTypeId && f.EntityId == entityId);
    }
}
