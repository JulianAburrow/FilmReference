namespace FilmReferenceUI.Shared.Components;

public partial class ListPeopleComponent
{
    [Parameter] public List<PersonModel> PersonModels { get; set; } = null!;
}
