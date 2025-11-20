namespace FilmReferenceUI.Shared.Components;

public partial class FilmComponent
{
    [Parameter] public FilmModel Film { get; set; } = null!;

    private string src = string.Empty;
    private string title = string.Empty;

    protected override void OnInitialized()
    {
        
    }

}
