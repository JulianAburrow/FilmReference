namespace FilmReferenceUI.Components.Pages.Studios;

public partial class ListStudios
{
    protected List<StudioModel> StudioModels { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        StudioModels = await StudioHandler.GetStudiosAsync();
        Snackbar.Add($"{StudioModels.Count} studio(s) found.", StudioModels.Count > 0 ? Severity.Info : Severity.Warning);
        MainLayout.SetHeaderValue("Studios");
    }
}

