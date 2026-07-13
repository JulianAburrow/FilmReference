namespace FilmReferenceUI.Shared.BasePageClasses;

public abstract class GenresBasePageClass : BasePageClass
{
    [Inject] protected IGenreHandler GenreHandler { get; set; } = null!;

    [Parameter] public int GenreId { get; set; }

    protected GenreModel GenreModel { get; set; } = new();

    protected GenreDisplayModel GenreDisplayModel { get; set; } = new();

    protected string GenreNotFoundMessage = "Genre not found.";

    protected string LoadingGenreMessage = "Loading genre...";

    protected void CopyDisplayModelToModel()
    {
        GenreModel.Name = GenreDisplayModel.Name;
        GenreModel.Description = GenreDisplayModel.Description;
        GenreModel.Logo = GenreDisplayModel.Logo;
    }

    protected void CopyModelToDisplayModel()
    {
        GenreDisplayModel.Name = GenreModel.Name;
        GenreDisplayModel.Description = GenreModel.Description;
        GenreDisplayModel.Logo = GenreModel.Logo;
    }
}
