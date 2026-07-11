namespace FilmReferenceUI.Components.Pages.People;

public partial class ListPeople
{
    [Parameter] public string PersonRole { get; set; } = string.Empty;

    private string Initial { get; set; } = "All";

    private List<PersonModel> AllPersonModels { get; set; } = null!;

    private List<PersonModel> FilteredPersonModels { get; set; } = null!;

    private Dictionary<string, int> LetterCounts { get; set; } = new();

    private RoleEnum? _lastRole;

    protected override async Task OnParametersSetAsync()
    {
        Enum.TryParse<RoleEnum>(PersonRole, true, out var role);

        if(_lastRole == role)
        {
            return;
        }

        _lastRole = role;

        if (role == RoleEnum.CastMembers)
        {
            MainLayout.SetHeaderValue("Cast Members");
            AllPersonModels = await PersonHandler.GetCastMembersAsync();
        }
        if (role == RoleEnum.Directors)
        {
            MainLayout.SetHeaderValue("Directors");
            AllPersonModels = await PersonHandler.GetDirectorsAsync();
        }

        FilterPeople(Initial);
        BuildLetterCounts();

        _isLoaded = true;
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
        NextSortDirection = SortDirection.Ascending;

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

        NextSortDirection = SortDirection.Descending;
    }

    private void ResortList()
    {
        switch (NextSortDirection)
        {
            case SortDirection.Ascending:
                FilteredPersonModels = [.. FilteredPersonModels.OrderBy(p => p.FirstName).ThenBy(p => p.LastName)];
                NextSortDirection = SortDirection.Descending;
                break;
            case SortDirection.Descending:
                FilteredPersonModels = [.. FilteredPersonModels.OrderByDescending(p => p.FirstName).ThenByDescending(p => p.LastName)];
                NextSortDirection = SortDirection.Ascending;
                break;
            default:
                FilteredPersonModels = [.. FilteredPersonModels.OrderBy(p => p.FirstName).ThenBy(p => p.LastName)];
                NextSortDirection = SortDirection.Descending;
                break;
        }
    }
}