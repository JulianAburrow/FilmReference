using MudBlazor.Utilities;

namespace FilmReferenceUI.Components.Pages.People;

public partial class EditPerson
{
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
            await PersonHandler.UpdatePersonAsync(PersonModel, true);
            Snackbar.Add($"Person {PersonModel.FirstName} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("people/listpeople");
        }
        catch
        {
            Snackbar.Add($"An error occurred editing {PersonModel.FirstName}. Please try again.", Severity.Error);
        }
    }

    private void RemovePictureAndImage()
    {
        PersonDisplayModel.Picture = null;
        Image = null;
        ImageName = null;
    }
}
