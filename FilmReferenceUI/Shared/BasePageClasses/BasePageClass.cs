using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.IO.Enumeration;

namespace FilmReferenceUI.Shared.BasePageClasses;

public class BasePageClass : ComponentBase
{
    [Inject] protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject] protected ISnackbar Snackbar { get; set; } = null!;

    [CascadingParameter] public MainLayout MainLayout { get; set; } = new();

    protected bool PreventDeleting;

    protected IBrowserFile? Image = null;

    protected string? ImageName = null;

    protected long MaxFileSize = 1024 * 1024 * 3;

    protected override void OnInitialized()
    {
        MainLayout.SetHeaderValue(string.Empty);
    }   

    protected void UploadFile(IBrowserFile file)
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
            Image = file;
            ImageName = file.Name;
            Snackbar.Add($"File {ImageName} successfully uploaded", Severity.Success);
        }
        catch
        {
            Snackbar.Add($"An error occurred uploading the {ImageName}. Please try again.", Severity.Error);
        }
    }

    protected void RemoveImage()
    {
        Image = null;
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
