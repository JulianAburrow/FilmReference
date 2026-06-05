namespace FilmReferenceUI.Components.Pages.People;

public partial class EditPerson
{
    private bool ShowRoleError { get; set; }

    protected override async Task OnInitializedAsync()
    {
        PersonModel = await PersonHandler.GetPersonAsync(PersonId);
        CopyModelToDisplayModel();
        MainLayout.SetHeaderValue("Edit Person");
    }

    private async void Update()
    {
        try
        {
            await CopyDisplayModelToModel();
            if (!PersonModel.IsDirector && !PersonModel.IsCastMember)
            {
                ShowRoleError = true;
                return;
            }
            await PersonHandler.UpdatePersonAsync(PersonModel, true);
            Snackbar.Add($"Person {PersonModel.FirstName} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo($"/people/listpeople/{(PersonModel.IsCastMember ? "castmembers" : "directors")}");
        }
        catch
        {
            Snackbar.Add($"An error occurred editing {PersonModel.FirstName}. Please try again.", Severity.Error);
        }
    }
}
