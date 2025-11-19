
namespace FilmReferenceUI.Shared.BasePageClasses;

public class FilmsBasePageClass : BasePageClass
{
    [Inject] protected IFilmHandler FilmHandler { get; set; } = null!;
    
    [Inject] protected IGenreHandler GenreHandler { get; set; } = null!;

    [Inject] protected IStudioHandler StudioHandler { get; set; } = null!;

    [Inject] protected IPersonHandler PersonHandler { get; set; } = null!;

    protected int value { get; set; } = -1;

    protected IEnumerable<int> SelectedActors { get; set; } = [];

    [Parameter] public int FilmId { get; set; }

    protected FilmModel FilmModel { get; set; } = new FilmModel
    {
        Genre = new GenreModel(),
        Director = new PersonModel(),
        Studio = new StudioModel()
    };

    protected FilmDisplayModel FilmDisplayModel { get; set; } = new();

    protected List<GenreModel> GenreModels { get; set; } = [];

    protected List<StudioModel> StudioModels { get; set; } = [];

    protected List<PersonModel> PersonModels { get; set; } = [];

    protected List<PersonModel> ActorModels { get; set; } = [];

    protected List<PersonModel> DirectorModels { get; set; } = [];

    protected async Task CopyDisplayModelToModelAsync()
    {
        FilmModel.Name = FilmDisplayModel.Name;
        FilmModel.Description = FilmDisplayModel.Description;
        FilmModel.GenreId = FilmDisplayModel.GenreId;
        FilmModel.StudioId = FilmDisplayModel.StudioId;
        FilmModel.DirectorId = FilmDisplayModel.DirectorId;
        FilmModel.FilmPerson = [];
        foreach (var actorId in SelectedActors)
        {            
            FilmModel.FilmPerson.Add(new FilmPersonModel
            {
                FilmId = FilmModel.FilmId,
                PersonId = actorId
            });
        }

        if (Image is not null)
        {
            var imageMemoryStream = await ToMemoryStreamAsync(Image.OpenReadStream(MaxFileSize));
            FilmModel.BoxCover = imageMemoryStream.ToArray();
            Image = null;
        }
        else
        {
            FilmModel.BoxCover = FilmDisplayModel.BoxCover;
        }
    }

    protected void CopyModelToDisplayModel()
    {
        FilmDisplayModel.Name = FilmModel.Name;
        FilmDisplayModel.Description = FilmModel.Description;
        FilmDisplayModel.GenreId = FilmModel.GenreId;
        FilmDisplayModel.StudioId = FilmModel.StudioId;
        FilmDisplayModel.DirectorId = FilmModel.DirectorId;
        FilmDisplayModel.BoxCover = FilmModel.BoxCover;
        foreach (var filmPerson in FilmModel.FilmPerson ?? [])
        {
            SelectedActors = SelectedActors.Append(filmPerson.PersonId);
        }
    }
}
