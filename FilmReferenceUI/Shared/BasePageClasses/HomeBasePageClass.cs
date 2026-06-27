namespace FilmReferenceUI.Shared.BasePageClasses;

public abstract class HomeBasePageClass : BasePageClass
{
    [Inject] protected ISearchHandler SearchHandler { get; set; } = null!;

    [Inject] protected IGenreHandler GenreHandler { get; set; } = null!;

    [Inject] protected IPersonHandler PersonHandler { get; set; } = null!;
}
