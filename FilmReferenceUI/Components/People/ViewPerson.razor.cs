namespace FilmReferenceUI.Components.People;

public partial class ViewPerson
{
    protected override async Task OnInitializedAsync()
    {
        PersonModel = await PersonHandler.GetPersonAsync(PersonId);
        PreventDeleting = PersonModel.FilmPerson.Any() || PersonModel.Films.Any();
        MainLayout.SetHeaderValue("View Person");
    }
}
