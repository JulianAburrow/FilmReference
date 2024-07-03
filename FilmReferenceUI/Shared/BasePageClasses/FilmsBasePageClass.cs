namespace FilmReferenceUI.Shared.BasePageClasses;

public class FilmsBasePageClass : BasePageClass
{
    [Inject] protected IFilmHandler FilmHandler { get; set; } = null!;
}
