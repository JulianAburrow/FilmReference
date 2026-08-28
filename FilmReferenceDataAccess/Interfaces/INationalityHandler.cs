namespace FilmReferenceDataAccess.Interfaces;

public interface INationalityHandler
{
    Task<List<NationalityModel>> GetNationalitiesAsync();
}