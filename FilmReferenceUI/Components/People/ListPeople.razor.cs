namespace FilmReferenceUI.Components.People;

public partial class ListPeople
{
    protected List<PersonModel> PersonModels { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        PersonModels = await PersonHandler.GetPeopleAsync();
        Snackbar.Add($"{PersonModels.Count} person(s) found", PersonModels.Count > 0 ? Severity.Info : Severity.Warning);
        MainLayout.SetHeaderValue("People");
    }
}
