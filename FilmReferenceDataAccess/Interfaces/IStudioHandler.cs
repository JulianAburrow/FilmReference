namespace FilmReferenceDataAccess.Interfaces;

public interface IStudioHandler
{
    Task<List<StudioModel>> GetStudiosAsync();

    Task<List<StudioModelLightweight>> GetStudiosLightweightAsync();

    Task<StudioModel> GetStudioAsync(int studioId);

    Task CreateStudioAsync(StudioModel studio);

    Task UpdateStudioAsync(StudioModel studio);

    Task DeleteStudioAsync(int studioId);
}
