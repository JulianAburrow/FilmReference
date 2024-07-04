namespace FilmReferenceUI.Components.Studios;

public partial class ListStudios
{
    protected List<StudioModel> StudioModels { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        StudioModels = await StudioHandler.GetStudiosAsync();
        Snackbar.Add($"{StudioModels.Count} item(s) found.", StudioModels.Count > 0 ? Severity.Info : Severity.Warning);
        MainLayout.SetHeaderValue("Studios");
    }
}

