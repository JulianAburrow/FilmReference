using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace FilmReferenceUI.Shared.BasePageClasses;

public class BasePageClass : ComponentBase
{
    [Inject] protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject] protected ISnackbar Snackbar { get; set; } = null!;

    [CascadingParameter] public MainLayout MainLayout { get; set; } = new();

    protected bool PreventDeleting;

    protected IBrowserFile Image = null!;

    protected string FileName = string.Empty;

    protected override void OnInitialized()
    {
        MainLayout.SetHeaderValue(string.Empty);
    }

    protected void UploadFile(IBrowserFile file)
    {
        if (file == null)
        {
            return;
        }
        try
        {
            Image = file;
            FileName = file.Name;
            Snackbar.Add($"File {FileName} successfully uploaded", Severity.Success);
        }
        catch
        {
            Snackbar.Add($"An error occurred uploading {file.Name}. Please try again.", Severity.Error);
        }
    }

    protected void RemoveImage()
    {
        Image = null!;
        FileName = string.Empty;
        StateHasChanged();
    }
}
