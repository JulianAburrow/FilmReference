namespace FilmReferenceUI.Components.Pages.People;

public partial class ListPeople
{
    private string Initial { get; set; } = "All";

    private List<PersonModel> AllPersonModels { get; set; } = null!;

    private List<PersonModel> FilteredPersonModels { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        AllPersonModels = await PersonHandler.GetPeopleAsync();
        MainLayout.SetHeaderValue("View People");
        FilterPeople(Initial);
    }

    private void FilterPeople(string initial)
    {
        Initial = initial;

        if (string.IsNullOrWhiteSpace(initial) || initial == "All")
        {
            FilteredPersonModels = AllPersonModels;
        }
        else
        {
            FilteredPersonModels = AllPersonModels
                .Where(p => p.FirstName.StartsWith(initial, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        var peopleWord = FilteredPersonModels.Count == 1 ? "person" : "people";
        var initialText = initial == "All" ? "" : $"with initial {initial}";
        Snackbar.Add(
            $"{FilteredPersonModels.Count} {peopleWord} found {initialText}.",
            FilteredPersonModels.Count > 0 ? Severity.Info : Severity.Warning);
    }
}

