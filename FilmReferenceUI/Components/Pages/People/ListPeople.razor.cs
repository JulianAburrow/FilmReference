namespace FilmReferenceUI.Components.Pages.People;

public partial class ListPeople
{
    [Parameter] public string Role { get; set; } = string.Empty;

    protected List<PersonModel> PersonModels { get; set; } = null!;

    protected List<PersonModel> FilteredPersonModels { get; set;  } = null!;

    private string SelectedFilter = "A";

    private bool Loading = false;

    protected override async Task OnParametersSetAsync()
    {
        if (Loading)
        {
            return;
        }

        Loading = true;

        _ = Enum.TryParse<RoleEnum>(Role, true, out var roleEnum);

        var (header, people) = roleEnum switch
        {
            RoleEnum.CastMembers => ("Cast Members", await PersonHandler.GetCastMembersAsync()),
            RoleEnum.Directors => ("Directors", await PersonHandler.GetDirectorsAsync()),
            _ => ("Error - no role selected", []),
        };

        MainLayout.SetHeaderValue(header);
        PersonModels = people;
        FilterPeople(SelectedFilter);
    }

    private void FilterPeople(string filter)
    {
        SelectedFilter = filter;
        if (filter == "All")
        {
            FilteredPersonModels = PersonModels;
            return;
        }

        FilteredPersonModels = PersonModels
            .Where(p =>
                p.FirstName.StartsWith(filter, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var count = FilteredPersonModels.Count;
        var message = count == 1
            ? $"1 person found for filter '{filter}'"
            : $"{count} people found for filter '{filter}'";

        Snackbar.Add(message, count > 0 ? Severity.Info : Severity.Warning);
    }

    private int GetPersonCountForInitial(string initialLetter)
    {
        return PersonModels.Count(p => p.FirstName.StartsWith(initialLetter));
    }
}

