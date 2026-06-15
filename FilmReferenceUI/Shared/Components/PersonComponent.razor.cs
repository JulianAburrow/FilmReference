namespace FilmReferenceUI.Shared.Components;

public partial class PersonComponent
{
    [Parameter] public PersonModel Person { get; set; } = null!;

    [Parameter] public string PersonRole { get; set; } = string.Empty;
}
