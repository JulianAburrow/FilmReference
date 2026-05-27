namespace FilmReferenceUI.Components.Pages.People;

public partial class ListPeople
{
    private string Initial { get; set; } = "All";

    private List<PersonModel> AllPersonModels { get; set; } = null!;

    private List<PersonModel> FilteredPersonModels { get; set; } = null!;

    private Dictionary<string, int> LetterCounts { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
        AllPersonModels = await PersonHandler.GetPeopleAsync();
        MainLayout.SetHeaderValue("View People");
        FilterPeople(Initial);
        BuildLetterCounts();
    }

    private void BuildLetterCounts()
    {
        LetterCounts = Enumerable.Range('A', 26)
            .Select(c => ((char)c).ToString())
            .ToDictionary(
                letter => letter,
                letter => AllPersonModels.Count(p =>
                    p.FirstName.StartsWith(letter, StringComparison.OrdinalIgnoreCase)
                )
            );
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

        var message = string.IsNullOrWhiteSpace(initialText)
            ? $"{FilteredPersonModels.Count} {peopleWord} found."
            : $"{FilteredPersonModels.Count} {peopleWord} found {initialText}.";

        Snackbar.Add(
            message,
            FilteredPersonModels.Count > 0 ? Severity.Info : Severity.Warning);
    }
}

