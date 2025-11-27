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

        Snackbar.Add($"{FilteredPersonModels.Count} person(s) found for filter '{filter}'", FilteredPersonModels.Count > 0 ? Severity.Info : Severity.Warning);
    }
}

