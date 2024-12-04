namespace FilmReferenceUI.Components.People;

public partial class CreatePerson
{
    protected override void OnInitialized() =>
        MainLayout.SetHeaderValue("Create Person");

    private async Task Create()
    {
        try
        {
            CopyDisplayModelToModel();
            await PersonHandler.CreatePersonAsync(PersonModel, true);
            Snackbar.Add($"Person {PersonModel.FirstName} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("/people/listpeople");
        }
        catch
        {
            Snackbar.Add($"An error occurred creating person {PersonModel.FirstName}. Please try again.", Severity.Error);
        }
    }
}
