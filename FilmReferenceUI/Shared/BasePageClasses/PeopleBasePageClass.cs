namespace FilmReferenceUI.Shared.BasePageClasses;

public abstract class PeopleBasePageClass : BasePageClass
{
    [Inject] protected IPersonHandler PersonHandler { get; set; } = null!;

    [Parameter] public int PersonId { get; set; }

    protected PersonModel PersonModel { get; set; } = new();

    protected PersonDisplayModel PersonDisplayModel { get; set; } = new();

    protected async Task CopyDisplayModelToModel()
    {
        PersonModel.FirstName = PersonDisplayModel.FirstName;
        PersonModel.LastName = PersonDisplayModel.LastName;
        PersonModel.Description = PersonDisplayModel.Description;
        PersonModel.IsCastMember = PersonDisplayModel.IsCastMember;
        PersonModel.IsDirector = PersonDisplayModel.IsDirector;
        PersonModel.DateOfBirth = PersonDisplayModel.DateOfBirth;
        PersonModel.DateOfDeath = PersonDisplayModel.DateOfDeath;

        if (ImageForDisplay is not null)
        {
            PersonModel.Picture = ImageForDisplay;
        }
        else
        {
            PersonModel.Picture = PersonDisplayModel.Picture;
        }
    }

    protected void CopyModelToDisplayModel()
    {
        PersonDisplayModel.FirstName = PersonModel.FirstName;
        PersonDisplayModel.LastName = PersonModel.LastName;
        PersonDisplayModel.Description = PersonModel.Description;
        PersonDisplayModel.IsCastMember = PersonModel.IsCastMember;
        PersonDisplayModel.IsDirector = PersonModel.IsDirector;
        PersonDisplayModel.DateOfBirth = PersonModel.DateOfBirth;
        PersonDisplayModel.DateOfDeath = PersonModel.DateOfDeath;
        PersonDisplayModel.Age = PersonModel.Age;
        PersonDisplayModel.Picture = PersonModel.Picture;
        PersonDisplayModel.Films = PersonModel.Films;
    }
}
