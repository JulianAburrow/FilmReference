namespace FilmReferenceUI.Components.Pages.People;

public partial class CreatePerson
{
    private bool ShowRoleError { get; set; }

    protected override async Task OnInitializedAsync() =>
        MainLayout.SetHeaderValue("Create Person");

    private async Task CreatePersonAsync()
    {
        try
        {
            await CopyDisplayModelToModel();
            if (!PersonModel.IsDirector && !PersonModel.IsCastMember)
            {
                ShowRoleError = true;
                return;
            }
            await PersonHandler.CreatePersonAsync(PersonModel, true);
            Snackbar.Add($"Person {PersonModel.FirstName} successfully created.", Severity.Success);
            NavigationManager.NavigateTo($"/people/listpeople/{(PersonModel.IsCastMember ? "castmembers" : "directors")}");
        }
        catch
        {
            Snackbar.Add($"An error occurred creating person {PersonModel.FirstName}. Please try again.", Severity.Error);
        }
    }
}
