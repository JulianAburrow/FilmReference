namespace FilmReferenceUI.Components.Pages.People;

public partial class DeletePerson
{
    protected override async Task OnInitializedAsync()
    {
        PersonModel = await PersonHandler.GetPersonAsync(PersonId);
        PreventDeleting = PersonModel.FilmPerson.Count != 0 || PersonModel.Films.Count != 0;
        MainLayout.SetHeaderValue($"Delete {PersonModel.FirstName} {PersonModel.LastName}");
    }

    private async Task DeletePersonAsync()
    {
        var personName = PersonModel.LastName is not null
            ? $"{PersonModel.FirstName} {PersonModel.LastName}"
            : PersonModel.FirstName;

        try
        {
            await PersonHandler.DeletePersonAsync(PersonId, true);
            Snackbar.Add($"{personName} successfully deleted.", Severity.Success);
            NavigationManager.NavigateTo($"/people/listpeople/{(PersonModel.IsCastMember ? RoleEnum.CastMembers : RoleEnum.Directors)}");
        }
        catch
        {
            Snackbar.Add($"An error occurred deleting {personName}. Please try again.", Severity.Error);
        }
    }
}
