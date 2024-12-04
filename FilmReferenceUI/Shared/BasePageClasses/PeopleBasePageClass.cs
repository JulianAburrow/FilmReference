using System.Net.WebSockets;

namespace FilmReferenceUI.Shared.BasePageClasses;

public class PeopleBasePageClass : BasePageClass
{
    [Inject] protected IPersonHandler PersonHandler { get; set; } = null!;

    [Parameter] public int PersonId { get; set; }

    protected PersonModel PersonModel { get; set; } = new();

    protected PersonDisplayModel PersonDisplayModel { get; set; } = new();

    protected async void CopyDisplayModelToModel()
    {
        PersonModel.FirstName = PersonDisplayModel.FirstName;
        PersonModel.LastName = PersonDisplayModel.LastName;
        PersonModel.Description = PersonDisplayModel.Description;
        PersonModel.IsActor = PersonDisplayModel.IsActor;
        PersonModel.IsDirector = PersonDisplayModel.IsDirector;

        if (Image != null )
        {
            //PersonModel.Picture = ToByteArray(Image.OpenReadStream());
            //PersonModel.Picture = imageMemoryStream;
        }
    }

    protected void CopyModelToDisplayModel()
    {
        PersonDisplayModel.FirstName = PersonModel.FirstName;
        PersonDisplayModel.LastName = PersonModel.LastName;
        PersonDisplayModel.Description = PersonModel.Description;
        PersonDisplayModel.IsActor = PersonModel.IsActor;
        PersonDisplayModel.IsDirector = PersonModel.IsDirector;
        PersonDisplayModel.PictureName = PersonModel.PictureName;
        PersonDisplayModel.Films = PersonModel.Films;
    }
}
