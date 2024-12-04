namespace FilmReferenceUI.Components.People;

public partial class DeletePerson
{
    protected override async Task OnInitializedAsync()
    {
        PersonModel = await PersonHandler.GetPersonAsync(PersonId);
        PreventDeleting = PersonModel.FilmPerson.Any() || PersonModel.Films.Any();
        MainLayout.SetHeaderValue("Delete Person");
    }

    private async Task Delete()
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
