using Microsoft.AspNetCore.Components.Forms;

namespace FilmReferenceUI.Shared.Components;

public partial class CreateUpdatePersonComponent
{
    [Parameter] public PersonDisplayModel PersonDisplayModel { get; set; } = new();

    [Inject] ISnackbar Snackbar { get; set; } = null!;

    private string? ImageName = null;

    protected long MaxFileSize = 1024 * 1024 * 3;

    protected async Task UploadFile(IBrowserFile file)
    {
        if (file is null)
        {
            return;
        }
        if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        {
            Snackbar.Add("Only image files are allowed.", Severity.Warning);
            return;
        }

        try
        {
            ImageName = file.Name;
            var imageMemoryStream = await ImageHelper.ToMemoryStream(file.OpenReadStream(MaxFileSize));
            PersonDisplayModel.Picture = imageMemoryStream.ToArray();
            Snackbar.Add($"File {ImageName} successfully uploaded", Severity.Success);
        }
        catch
        {
            Snackbar.Add($"An error occurred uploading {ImageName}. Please try again.", Severity.Error);
        }
    }
    protected void RemoveImage()
    {
        PersonDisplayModel.Picture = null;
        ImageName = null;
        Snackbar.Add("Image successfully removed.", Severity.Success);
        StateHasChanged();
    }
}
