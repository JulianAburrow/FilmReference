using System.ComponentModel;

namespace FilmReferenceUI.Components.Pages.People;

public partial class ListPeople
{
    [Parameter] public string PersonRole { get; set; } = string.Empty;

    private List<PersonModel> AllPersonModels { get; set; } = null!;

    private List<PersonModel> FilteredPersonModels { get; set; } = null!;

    private List<NationalityListModel> Nationalities { get; set; } = null!;

    private List<string> Initials { get; set; } = [];

    private Dictionary<string, int> FilterOptions { get; set; } = new();

    private string Initial { get; set; } = "All";

    private string Country { get; set; } = "All";

    public string? SelectedFilter { get; set; }

    private string InitialsOrNationalities = "Nationalities";

    private RoleEnum? _lastRole;

    private enum FilterMode
    {
        Initials,
        Nationalities
    }

    private FilterMode CurrentFilterMode = FilterMode.Initials;

    protected override async Task OnParametersSetAsync()
    {
        Enum.TryParse<RoleEnum>(PersonRole, true, out var role);

        if (_lastRole == role)
            return;

        _lastRole = role;

        MainLayout.SetHeaderValue(role switch
        {
            RoleEnum.CastMembers => "Cast Members",
            RoleEnum.Directors => "Directors",
            _ => "People"
        });

        AllPersonModels = role switch
        {
            RoleEnum.CastMembers => await PersonHandler.GetCastMembersAsync(),
            RoleEnum.Directors => await PersonHandler.GetDirectorsAsync(),
            _ => []
        };

        Nationalities = role switch
        {
            RoleEnum.CastMembers => await NationalityHandler.GetNationalitiesInUseForCastMembersAsync(),
            RoleEnum.Directors => await NationalityHandler.GetNationalitiesInUseForDirectorsAsync(),
            _ => []
        };


        Nationalities.Add(new NationalityListModel
        {
            Name = "Unknown",
        });
        Nationalities = Nationalities.OrderBy(n => n.Name).ToList();

        Initials = AllPersonModels
            .Where(p => !string.IsNullOrWhiteSpace(p.FirstName))
            .Select(p => p.FirstName[0].ToString().ToUpper())
            .Distinct()
            .OrderBy(x => x)
            .ToList();

        BuildFilterOptions();
        ApplyFilter("All");

        

        _isLoaded = true;
    }


    // ------------------------------------------------------------
    // BUILD FILTER OPTIONS (Initials OR Nationalities)
    // ------------------------------------------------------------
    private void BuildFilterOptions()
    {
        FilterOptions = CurrentFilterMode switch
        {
            FilterMode.Initials =>
                AllPersonModels
                    .Where(p => !string.IsNullOrWhiteSpace(p.FirstName))
                    .GroupBy(p => p.FirstName[0].ToString().ToUpper())
                    .ToDictionary(g => g.Key, g => g.Count()),

            FilterMode.Nationalities =>
                AllPersonModels
                    .GroupBy(p => p.Nationality?.Name ?? "Unknown")
                    .ToDictionary(g => g.Key, g => g.Count())
        };
    }

    // ------------------------------------------------------------
    // APPLY FILTER (Unified)
    // ------------------------------------------------------------
    private void ApplyFilter(string value)
    {
        value ??= "All";   // null → "All"

        NextSortDirection = SortDirection.Ascending;

        // Apply the correct filter based on mode
        FilteredPersonModels = CurrentFilterMode switch
        {
            FilterMode.Initials => ApplyInitialFilter(value),
            FilterMode.Nationalities => ApplyNationalityFilter(value),
            _ => AllPersonModels
        };

        ShowSnackbar(value);

        NextSortDirection = SortDirection.Descending;
    }

    private List<PersonModel> ApplyInitialFilter(string value)
    {
        Initial = value;

        return value == "All"
            ? AllPersonModels
            : AllPersonModels.Where(p =>
                p.FirstName.StartsWith(value, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    private List<PersonModel> ApplyNationalityFilter(string value)
    {
        Country = value;

        return value switch
        {
            "All" => AllPersonModels,
            "Unknown" => AllPersonModels.Where(p => p.Nationality is null).ToList(),
            _ => AllPersonModels.Where(p => p.Nationality?.Name == value).ToList()
        };
    }

    private void ShowSnackbar(string value)
    {
        var count = FilteredPersonModels.Count;
        var plural = count == 1 ? "person" : "people";

        var message = CurrentFilterMode switch
        {
            FilterMode.Initials => value == "All"
                ? $"{count} {plural} found."
                : $"{count} {plural} found with initial {value}.",

            FilterMode.Nationalities => value == "All"
                ? $"{count} {plural} found."
                : $"{count} {value} {plural} found."
        };

        Snackbar.Add(message, count > 0 ? Severity.Info : Severity.Warning);
    }

    // ------------------------------------------------------------
    // SORTING
    // ------------------------------------------------------------
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

    // ------------------------------------------------------------
    // SWAP FILTER MODE
    // ------------------------------------------------------------
    private void SwapInitialsForNationalities()
    {
        // Toggle mode
        CurrentFilterMode = CurrentFilterMode switch
        {
            FilterMode.Initials => FilterMode.Nationalities,
            FilterMode.Nationalities => FilterMode.Initials,
        };

        // Update UI label
        InitialsOrNationalities = CurrentFilterMode switch
        {
            FilterMode.Initials => FilterMode.Nationalities.ToString(),
            FilterMode.Nationalities => FilterMode.Initials.ToString(),
        };

        BuildFilterOptions();
        ApplyFilter("All");

        StateHasChanged();
    }
}
