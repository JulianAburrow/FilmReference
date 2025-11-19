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
        Snackbar.Add($"{PersonModels.Count} person(s) found", PersonModels.Count > 0 ? Severity.Info : Severity.Warning);
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
    }
}

