namespace FilmReferenceUI.Components.Pages.People;

public partial class CreatePerson
{
    private bool ShowRoleError { get; set; }

    protected override async Task OnInitializedAsync()
    {
        MainLayout.SetHeaderValue("Create Person");
        NationalityModels = await NationalityHandler.GetNationalitiesAsync();
        NationalityModels.Insert(0, new NationalityModel
        {
            NationalityId = SharedValues.PleaseSelectValue,
            Name = SharedValues.PleaseSelectText
        });
        PersonDisplayModel.NationalityId = SharedValues.PleaseSelectValue;
    }
        

    private async Task CreatePersonAsync()
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
            if (PersonModel.NationalityId == SharedValues.PleaseSelectValue)
            {
                PersonModel.NationalityId = null;
            }
            await PersonHandler.CreatePersonAsync(PersonModel);
            Snackbar.Add($"{personName} successfully created.", Severity.Success);
            NavigationManager.NavigateTo($"/person/view/{PersonModel.PersonId}");
        }
        catch
        {
            Snackbar.Add($"An error occurred creating {personName}. Please try again.", Severity.Error);
        }
    }
}
