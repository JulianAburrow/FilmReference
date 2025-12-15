namespace FilmReferenceUI.Shared.Components;

public partial class CreateUpdateStudioComponent
{
    [Parameter] public new StudioDisplayModel StudioDisplayModel { get; set; } = new();

    protected async Task LocalUploadImage(IBrowserFile file)
    {
        await GlobalUploadImage(file);
        StudioDisplayModel.Logo = ImageForDisplay;
    }

    protected void LocalRemoveImage()
    {
        GlobalRemoveImage();
        StudioDisplayModel.Logo = ImageForDisplay;
    }
}
