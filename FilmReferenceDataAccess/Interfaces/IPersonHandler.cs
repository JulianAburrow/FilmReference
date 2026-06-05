namespace FilmReferenceDataAccess.Interfaces;

public interface IPersonHandler
{
    Task<List<PersonModel>> GetCastMembersAsync();

    Task<List<PersonModel>> GetDirectorsAsync();

    Task<PersonModel> GetPersonAsync(int personId);

    Task CreatePersonAsync(PersonModel person, bool saveChanges);
    
    Task UpdatePersonAsync(PersonModel person, bool saveChanges);

    Task DeletePersonAsync(int personId, bool saveChanges);

    Task<RandomPersonModel?> GetRandomPersonAsync();

    Task SaveChangesAsync();
}
