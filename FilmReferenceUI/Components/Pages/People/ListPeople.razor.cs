namespace FilmReferenceUI.Components.Pages.People;

public partial class ListPeople
{
    protected List<PersonModel> PersonModels { get; set; } = null!;

    protected List<PersonModel> FilteredPersonModels { get; set;  } = null!;

    private string SelectedFilter = "A";

    protected override async Task OnInitializedAsync()
    {
        PersonModels = await PersonHandler.GetPeopleAsync();
        await FilterPeople(SelectedFilter);
        MainLayout.SetHeaderValue("People");
    }

    private async Task FilterPeople(string filter)
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

