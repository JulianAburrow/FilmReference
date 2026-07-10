namespace FilmReferenceUI.Shared.BasePageClasses;

public abstract class BasePageClass : ComponentBase
{
    [Inject] protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject] protected ISnackbar Snackbar { get; set; } = null!;

    [Inject] protected IFavouriteHandler FavouriteHandler { get; set; } = null!;

    [Inject] protected SearchState SearchState { get; set; } = null!;

    [CascadingParameter] public MainLayout MainLayout { get; set; } = new();

    protected SortDirection NextSortDirection = SortDirection.Ascending;

    protected bool PreventDeleting;

    protected bool _isLoaded;

    protected string? ImageName = null;

    protected byte[]? ImageForDisplay = null;

    protected long MaxFileSize = 1024 * 1024 * 3;

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
            Snackbar.Add($"{ImageName} successfully uploaded.", Severity.Success);
        }
        catch
        {
            Snackbar.Add($"An error occurred uploading {ImageName}. Please try again.", Severity.Error);
        }
    }

    protected void GlobalRemoveImage()
    {
        Snackbar.Add($"{ImageName} successfully removed.", Severity.Success);
        ImageForDisplay = null;
        ImageName = null;
        StateHasChanged();
    }

    protected static async Task<MemoryStream> ToMemoryStreamAsync(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream;
    }
}
