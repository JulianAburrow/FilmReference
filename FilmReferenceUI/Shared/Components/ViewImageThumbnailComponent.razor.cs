namespace FilmReferenceUI.Shared.Components;

public partial class ViewImageThumbnailComponent
{
    [Parameter] public byte[]? ImageData { get; set; }

    [Parameter] public string ImageTitle { get; set; } = string.Empty;
}