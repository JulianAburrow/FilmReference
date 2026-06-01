namespace FilmReferenceDataAccess.Interfaces;

public interface IFavouriteHandler
{
    Task<List<FavouriteDisplayModel>> GetAllFavouritesAsync();

    Task CreateFavouriteAsync(FavouriteModel favourite, bool saveChanges);

    Task DeleteFavouriteAsync(int entityTypeId, int entityId, bool saveChanges);

    Task<bool> IsFavouriteAsync(int entityTypeId, int entityId);

    Task SaveChangesAsync();
}
