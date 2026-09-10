namespace FilmReferenceDataAccess.Interfaces;

public interface INationalityHandler
{
    Task<List<NationalityModel>> GetNationalitiesAsync();

    Task<List<NationalityListModel>> GetNationalitiesInUseForCastMembersAsync();

    Task<List<NationalityListModel>> GetNationalitiesInUseForDirectorsAsync();
}