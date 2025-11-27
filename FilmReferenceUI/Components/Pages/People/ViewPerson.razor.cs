namespace FilmReferenceUI.Components.Pages.People;

public partial class ViewPerson
{
    private List<FilmModel> FilmsStarredIn { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        PersonModel = await PersonHandler.GetPersonAsync(PersonId);
        PreventDeleting = PersonModel.FilmPerson.Count != 0;
        MainLayout.SetHeaderValue("View Person");
        if (PersonModel.FilmPerson is null)
        {
            return;
        }
        foreach (var filmPerson in PersonModel.FilmPerson)
        {
            FilmsStarredIn.Add(filmPerson.Film);
        }
    }
}
