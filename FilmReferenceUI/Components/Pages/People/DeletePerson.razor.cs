namespace FilmReferenceUI.Components.Pages.People;

public partial class DeletePerson
{
    protected override async Task OnInitializedAsync()
    {
        PersonModel = await PersonHandler.GetPersonAsync(PersonId);
        PreventDeleting = PersonModel.FilmPerson.Count != 0 || PersonModel.Films.Count != 0;
        SetLinkForReturn();
        MainLayout.SetHeaderValue("Delete Person");
    }

    private async Task DeletePersonAsync()
    {
        try
        {
            await PersonHandler.DeletePersonAsync(PersonId, true);
            Snackbar.Add($"Person {PersonModel.FirstName} successfully deleted.", Severity.Success);
            NavigationManager.NavigateTo("people/listpeople");
        }
        catch
        {
            Snackbar.Add($"An error occurred deleting person {PersonModel.FirstName}. Please try again.", Severity.Error);
        }
    }
}
