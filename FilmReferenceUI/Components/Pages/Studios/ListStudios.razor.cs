namespace FilmReferenceUI.Components.Pages.Studios;

public partial class ListStudios
{
    protected List<StudioModel> StudioModels { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        StudioModels = await StudioHandler.GetStudiosAsync();
        Snackbar.Add($"{StudioModels.Count} {(StudioModels.Count == 1 ? "studio" : "studios")} found.", StudioModels.Count > 0 ? Severity.Info : Severity.Warning);
        MainLayout.SetHeaderValue("View Studios");
        NextSortDirection = SortDirection.Descending;
    }

    private void ResortList()
    {
        switch (NextSortDirection)
        {
            case SortDirection.Ascending:
                StudioModels = [.. StudioModels.OrderBy(g => g.Name)];
                NextSortDirection = SortDirection.Descending;
                break;
            case SortDirection.Descending:
                StudioModels = [.. StudioModels.OrderByDescending(g => g.Name)];
                NextSortDirection = SortDirection.Ascending;
                break;
            default:
                StudioModels = [.. StudioModels.OrderBy(g => g.Name)];
                NextSortDirection = SortDirection.Descending;
                break;
        }
    }
}

