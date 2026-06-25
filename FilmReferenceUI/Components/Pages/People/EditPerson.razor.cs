namespace FilmReferenceUI.Components.Pages.People;

public partial class EditPerson
{
    private bool ShowRoleError { get; set; }

    protected override async Task OnInitializedAsync()
    {
        PersonModel = await PersonHandler.GetPersonAsync(PersonId);
        CopyModelToDisplayModel();
        MainLayout.SetHeaderValue($"Edit Person {PersonModel.FirstName} {PersonModel.LastName}");
    }

    private async void Update()
    {
        var personName = string.Empty;

        try
        {
            await CopyDisplayModelToModel();
            personName = PersonModel.LastName is not null
                ? $"{PersonModel.FirstName} {PersonModel.LastName}"
                : PersonModel.FirstName;
            if (!PersonModel.IsDirector && !PersonModel.IsCastMember)
            {
                ShowRoleError = true;
                return;
            }
            await PersonHandler.UpdatePersonAsync(PersonModel, true);
            Snackbar.Add($"{personName} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo($"/person/view/{PersonModel.PersonId}");
        }
        catch
        {
            Snackbar.Add($"An error occurred editing {personName}. Please try again.", Severity.Error);
        }
    }
}
