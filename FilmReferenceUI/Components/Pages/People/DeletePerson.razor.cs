namespace FilmReferenceUI.Components.Pages.People;

public partial class DeletePerson
{
    protected override async Task OnInitializedAsync()
    {
        PersonModel = await PersonHandler.GetPersonAsync(PersonId);

        if (PersonModel.PersonId == 0)
        {
            MainLayout.SetHeaderValue(PersonNotFoundMessage);
            _entityNotFound = true;
            return;
        }

        PreventDeleting = PersonModel.FilmPerson.Count != 0 || PersonModel.Films.Count != 0;
        MainLayout.SetHeaderValue($"Delete {PersonModel.FirstName} {PersonModel.LastName}");

        _isLoaded = true;
    }

    private async Task DeletePersonAsync()
    {
        var personName = PersonModel.LastName is not null
            ? $"{PersonModel.FirstName} {PersonModel.LastName}"
            : PersonModel.FirstName;

        try
        {
            await PersonHandler.DeletePersonAsync(PersonId);
            Snackbar.Add($"{personName} successfully deleted.", Severity.Success);
            NavigationManager.NavigateTo($"/people/listpeople/{(PersonModel.IsCastMember ? RoleEnum.CastMembers : RoleEnum.Directors)}");
        }
        catch
        {
            Snackbar.Add($"An error occurred deleting {personName}. Please try again.", Severity.Error);
        }
    }
}
