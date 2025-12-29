namespace FilmReferenceUI.Components.Pages.People;

public partial class EditPerson
{
    protected override async Task OnInitializedAsync()
    {
        PersonModel = await PersonHandler.GetPersonAsync(PersonId);
        CopyModelToDisplayModel();
        MainLayout.SetHeaderValue("Edit Person");
        SetLinkForReturn();
    }

    private async void Update()
    {
        try
        {
            await CopyDisplayModelToModel();
            await PersonHandler.UpdatePersonAsync(PersonModel, true);
            Snackbar.Add($"Person {PersonModel.FirstName} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo($"/people/listpeople/{LinkForReturn}/{PersonModel.FirstName[..1]}");
        }
        catch
        {
            Snackbar.Add($"An error occurred editing {PersonModel.FirstName}. Please try again.", Severity.Error);
        }
    }
}
