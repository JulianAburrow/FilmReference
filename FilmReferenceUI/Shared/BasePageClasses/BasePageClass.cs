using System.Threading.Tasks;

namespace FilmReferenceUI.Shared.BasePageClasses;

public class BasePageClass : ComponentBase
{
    [Inject] protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject] protected ISnackbar Snackbar { get; set; } = null!;

    [CascadingParameter] public MainLayout MainLayout { get; set; } = new();

    protected bool PreventDeleting;

    //protected IBrowserFile? Image = null;

    protected string? ImageName = null;

    protected byte[]? ImageForDisplay = null;

    protected long MaxFileSize = 1024 * 1024 * 3;

    protected override void OnInitialized()
    {
        MainLayout.SetHeaderValue(string.Empty);
    }   

    protected async Task GlobalUploadImage(IBrowserFile? file)
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
            ImageForDisplay = imageMemoryStream.ToArray();
            Snackbar.Add($"File {ImageName} successfully uploaded", Severity.Success);
        }
        catch
        {
            Snackbar.Add($"An error occurred uploading the {ImageName}. Please try again.", Severity.Error);
        }
    }

    protected void GlobalRemoveImage()
    {
        ImageForDisplay = null;
        ImageName = null;
        Snackbar.Add("Image successfully removed.", Severity.Success);
        StateHasChanged();
    }

    protected static async Task<MemoryStream> ToMemoryStreamAsync(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream;
    }
}
