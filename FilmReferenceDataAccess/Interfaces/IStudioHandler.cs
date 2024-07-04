namespace FilmReferenceDataAccess.Interfaces;

public interface IStudioHandler
{
    Task<List<StudioModel>> GetStudiosAsync();

    Task<StudioModel> GetStudioAsync(int studioId);

    Task CreateStudioAsync(StudioModel studio, bool saveChanges);

    Task UpdateStudioAsync(StudioModel studio, bool saveChanges);

    Task DeleteStudioAsync(int studioId, bool saveChanges);

    Task SaveChangesAsync();
}
