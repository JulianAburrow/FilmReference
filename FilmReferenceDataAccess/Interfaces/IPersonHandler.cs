namespace FilmReferenceDataAccess.Interfaces;

public interface IPersonHandler
{
    Task<List<PersonModel>> GetCastMembersAsync(string initial);

    Task<List<PersonModel>> GetDirectorsAsync(string? initial);

    Task<PersonModel> GetPersonAsync(int personId);

    Task CreatePersonAsync(PersonModel person, bool saveChanges);
    
    Task UpdatePersonAsync(PersonModel person, bool saveChanges);

    Task DeletePersonAsync(int personId, bool saveChanges);

    Task SaveChangesAsync();
}
