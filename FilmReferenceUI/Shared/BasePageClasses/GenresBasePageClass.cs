namespace FilmReferenceUI.Shared.BasePageClasses;

public class GenresBasePageClass : BasePageClass
{
    [Inject] protected IGenreHandler GenreHandler { get; set; } = null!;

    [Parameter] public int GenreId { get; set; }

    protected GenreModel GenreModel { get; set; } = new();

    protected GenreDisplayModel GenreDisplayModel { get; set; } = new();

    protected void CopyDisplayModelToModel()
    {
        GenreModel.Name = GenreDisplayModel.Name;
        GenreModel.Description = GenreDisplayModel.Description;
    }

    protected void CopyModelToDisplayModel()
    {
        GenreDisplayModel.Name = GenreModel.Name;
        GenreDisplayModel.Description = GenreModel.Description;
    }
}
