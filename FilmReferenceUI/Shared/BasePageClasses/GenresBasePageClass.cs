namespace FilmReferenceUI.Shared.BasePageClasses;

public class GenresBasePageClass : BasePageClass
{
    [Inject] protected IGenreHandler GenreHandler { get; set; } = null!;

    protected GenreModel GenreModel { get; set; } = new();

    protected GenreDisplayModel GenreDisplayModel { get; set; } = new();

    protected void CopyDisplayModelToModel()
    {
        GenreModel.Name = GenreDisplayModel.Name;
        GenreModel.Description = GenreDisplayModel.Description;
    }
}
