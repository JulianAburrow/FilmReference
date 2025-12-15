namespace FilmReferenceUI.Shared.Components;

public partial class CreateUpdatePersonComponent
{
    [Parameter] public new PersonDisplayModel PersonDisplayModel { get; set; } = new();

    protected async Task LocalUploadImage(IBrowserFile file)
    {
        await GlobalUploadImage(file);
        PersonDisplayModel.Picture = ImageForDisplay;
    }

    protected void LocalRemoveImage()
    {
        GlobalRemoveImage();
        PersonDisplayModel.Picture = ImageForDisplay;
    }
}
