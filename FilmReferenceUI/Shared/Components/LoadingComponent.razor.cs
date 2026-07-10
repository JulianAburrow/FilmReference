namespace FilmReferenceUI.Shared.Components;

public partial class LoadingComponent
{
    [Parameter] public string DisplayValue { get; set; } = "Loading data";

    /// <summary>
    /// True (default) = shows a spinner for active loading.
    /// False = shows an empty-state icon for "no results found" scenarios.
    /// </summary>
    [Parameter] public bool IsLoading { get; set; } = true;
}