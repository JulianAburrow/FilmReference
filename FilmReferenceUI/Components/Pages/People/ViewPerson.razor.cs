namespace FilmReferenceUI.Components.Pages.People;

public partial class ViewPerson
{
    protected override async Task OnInitializedAsync()
    {
        PersonModel = await PersonHandler.GetPersonAsync(PersonId);
        PreventDeleting = PersonModel.FilmPerson.Count != 0;
        MainLayout.SetHeaderValue("View Person");
    }
}
