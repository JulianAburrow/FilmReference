namespace FilmReferenceUI.Shared.BasePageClasses;

public class BasePageClass : ComponentBase
{
    [Inject] protected NavigationManager NavigationManager { get; set; } = null!;

    [Inject] protected ISnackbar Snackbar { get; set; } = null!;

    [CascadingParameter] public MainLayout MainLayout { get; set; } = new();

    protected override void OnInitialized()
    {
        MainLayout.SetHeaderValue(string.Empty);
    }
}
