namespace FilmReferenceDataAccess.Interfaces;

public interface IFavouriteHandler
{
    Task<List<FavouriteDisplayModel>> GetAllFavouritesAsync();

    Task CreateFavouriteAsync(FavouriteModel favourite);

    Task DeleteFavouriteAsync(int entityTypeId, int entityId);

    Task<bool> IsFavouriteAsync(int entityTypeId, int entityId);
}
