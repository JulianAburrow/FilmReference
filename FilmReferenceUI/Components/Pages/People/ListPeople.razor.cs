using System.ComponentModel;

namespace FilmReferenceUI.Components.Pages.People;

public partial class ListPeople
{
    [Parameter] public string PersonRole { get; set; } = string.Empty;

    private List<PersonModel> AllPersonModels { get; set; } = null!;

    private List<PersonModel> FilteredPersonModels { get; set; } = null!;

    private List<NationalityListModel> Nationalities { get; set; } = null!;

    private Dictionary<string, int> FilterOptions { get; set; } = new();

    private string Initial { get; set; } = "All";

    private string Country { get; set; } = "All";

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

        if (role == RoleEnum.CastMembers)
        {
            MainLayout.SetHeaderValue("Cast Members");
            AllPersonModels = await PersonHandler.GetCastMembersAsync();
        }
        else if (role == RoleEnum.Directors)
        {
            MainLayout.SetHeaderValue("Directors");
            AllPersonModels = await PersonHandler.GetDirectorsAsync();
        }

        BuildFilterOptions();
        ApplyFilter("All");

        Nationalities = await NationalityHandler.GetNationalitiesInUseAsync();

        _isLoaded = true;
    }


    // ------------------------------------------------------------
    // BUILD FILTER OPTIONS (Initials OR Nationalities)
    // ------------------------------------------------------------
    private void BuildFilterOptions()
    {
        if (CurrentFilterMode == FilterMode.Initials)
        {
            FilterOptions = AllPersonModels
                .GroupBy(p => p.FirstName[0].ToString().ToUpper())
                .ToDictionary(g => g.Key, g => g.Count());
        }
        else
        {
            FilterOptions = AllPersonModels
                .Where(p => p.Nationality is not null)
                .GroupBy(p => p.Nationality!.Name)
                .ToDictionary(g => g.Key, g => g.Count());

            // Add unknown nationality count
            var unknownCount = AllPersonModels.Count(p => p.Nationality is null);

            if (unknownCount > 0)
            {
                FilterOptions.Add("Unknown", unknownCount);
            }
        }
    }


    // ------------------------------------------------------------
    // APPLY FILTER (Unified)
    // ------------------------------------------------------------
    private void ApplyFilter(string value)
    {
        NextSortDirection = SortDirection.Ascending;

        if (CurrentFilterMode == FilterMode.Initials)
        {
            Initial = value;

            FilteredPersonModels =
                value == "All"
                    ? AllPersonModels
                    : AllPersonModels.Where(p =>
                        p.FirstName.StartsWith(value, StringComparison.OrdinalIgnoreCase)).ToList();

            Snackbar.Add(
                $"{FilteredPersonModels.Count} {(FilteredPersonModels.Count == 1 ? "person" : "people")} found{(value == "All" ? "" : $" with initial {value}")}.",
                FilteredPersonModels.Count > 0 ? Severity.Info : Severity.Warning);
        }
        else
        {
            Country = value;

            FilteredPersonModels =
                value == "All"
                    ? AllPersonModels
                    : value == "Unknown"
                        ? AllPersonModels.Where(p => p.Nationality is null).ToList()
                        : AllPersonModels.Where(p => p.Nationality?.Name == value).ToList();

            Snackbar.Add(
                value == "All"
                ? $"{FilteredPersonModels.Count} people found."
                : $"{FilteredPersonModels.Count} {value} {(FilteredPersonModels.Count == 1 ? "person" : "people")} found.",
                FilteredPersonModels.Count > 0 ? Severity.Info : Severity.Warning);
        }

        NextSortDirection = SortDirection.Descending;
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
        CurrentFilterMode = CurrentFilterMode == FilterMode.Initials
            ? FilterMode.Nationalities
            : FilterMode.Initials;

        InitialsOrNationalities = CurrentFilterMode == FilterMode.Initials
            ? "Nationalities"
            : "Initials";

        BuildFilterOptions();
        ApplyFilter("All");

        StateHasChanged();
    }
}
