namespace FilmReferenceUI.Shared.Components;

public partial class ListPeopleComponent
{
    [Parameter] public List<PersonModel> PersonModels { get; set; } = null!;

    [Parameter] public string PersonRole { get; set; } = string.Empty;

    [Parameter] public bool ShowEditButton { get; set; } = true;
}
