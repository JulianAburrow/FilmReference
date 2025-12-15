namespace FilmReferenceUI.Shared.BasePageClasses;

public class PeopleBasePageClass : BasePageClass
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
        PersonModel.IsActor = PersonDisplayModel.IsActor;
        PersonModel.IsDirector = PersonDisplayModel.IsDirector;

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
        PersonDisplayModel.IsActor = PersonModel.IsActor;
        PersonDisplayModel.IsDirector = PersonModel.IsDirector;
        PersonDisplayModel.Picture = PersonModel.Picture;
        PersonDisplayModel.Films = PersonModel.Films;
    }
}
