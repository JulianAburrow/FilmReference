namespace FilmReferenceDataAccess.Interfaces;

public interface IPersonHandler
{
    Task<List<PersonModel>> GetCastMembersAsync();

    Task<List<PersonModelLightweight>> GetCastMembersLightweightAsync();

    Task<List<PersonModel>> GetDirectorsAsync();

    Task<List<PersonModelLightweight>> GetDirectorsLightweightAsync();

    Task<PersonModel> GetPersonAsync(int personId);

    Task CreatePersonAsync(PersonModel person);
    
    Task UpdatePersonAsync(PersonModel person);

    Task DeletePersonAsync(int personId);

    Task<RandomPersonModel> GetRandomPersonAsync();
}
