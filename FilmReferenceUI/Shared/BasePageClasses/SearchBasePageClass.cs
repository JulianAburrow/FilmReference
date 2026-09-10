namespace FilmReferenceUI.Shared.BasePageClasses;

public class SearchBasePageClass : BasePageClass
{
    [Inject] protected ISearchHandler SearchHandler { get; set; } = null!;

    [Inject] protected IGenreHandler GenreHandler { get; set; } = null!;
}
