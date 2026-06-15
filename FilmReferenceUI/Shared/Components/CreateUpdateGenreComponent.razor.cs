namespace FilmReferenceUI.Shared.Components;

public partial class CreateUpdateGenreComponent
{
    [Parameter] public new GenreDisplayModel GenreDisplayModel { get; set; } = new();

    protected async Task LocalUploadImage(IBrowserFile file)
    {
        await GlobalUploadImage(file);
        GenreDisplayModel.Logo = ImageForDisplay;
    }

    protected void LocalRemoveImage()
    {
        GlobalRemoveImage();
        GenreDisplayModel.Logo = ImageForDisplay;
    }
}
