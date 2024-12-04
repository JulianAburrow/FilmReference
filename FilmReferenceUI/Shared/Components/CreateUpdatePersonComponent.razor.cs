namespace FilmReferenceUI.Shared.Components;

public partial class CreateUpdatePersonComponent
{
    [Parameter] public PersonDisplayModel PersonDisplayModel { get; set; } = new();
}
