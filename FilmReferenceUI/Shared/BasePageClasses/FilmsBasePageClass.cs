
namespace FilmReferenceUI.Shared.BasePageClasses;

public abstract class FilmsBasePageClass : BasePageClass
{
    [Inject] protected IFilmHandler FilmHandler { get; set; } = null!;
    
    [Inject] protected IGenreHandler GenreHandler { get; set; } = null!;

    [Inject] protected IStudioHandler StudioHandler { get; set; } = null!;

    [Inject] protected IPersonHandler PersonHandler { get; set; } = null!;

    [Parameter] public int FilmId { get; set; }

    protected string FilmNotFoundMessage = "Film not found";

    protected string LoadingFilmMessage = "Loading film...";

    protected FilmModel FilmModel { get; set; } = new FilmModel
    {
        Genre = new GenreModel(),
        Director = new PersonModel(),
        Studio = new StudioModel()
    };

    protected FilmDisplayModel FilmDisplayModel { get; set; } = new();

    protected List<PersonModel> CastMemberModels { get; set; } = [];

    protected List<PersonModel> DirectorModels { get; set; } = [];

    protected List<GenreModel> GenreModels { get; set; } = [];

    protected List<StudioModel> StudioModels { get; set; } = [];

    protected List<GenreModelLightweight> GenreModelsLightweight { get; set; } = [];

    protected List<StudioModelLightweight> StudioModelsLightweight { get; set; } = [];

    public List<PersonModelLightweight> CastMemberModelsLightweight { get; set; } = [];

    protected List<PersonModelLightweight> DirectorModelsLightweight { get; set; } = [];

    protected async Task CopyDisplayModelToModelAsync()
    {
        FilmModel.Name = FilmDisplayModel.Name;
        FilmModel.Description = FilmDisplayModel.Description;
        FilmModel.GenreId = FilmDisplayModel.GenreId;
        FilmModel.StudioId = FilmDisplayModel.StudioId;
        FilmModel.DirectorId = FilmDisplayModel.DirectorId;
        FilmModel.BoxCover = FilmDisplayModel.BoxCover;
        FilmModel.FilmPerson = [];
        foreach (var selectedCastMemberId in FilmDisplayModel.SelectedCastMemberIds)
        {            
            FilmModel.FilmPerson.Add(new FilmPersonModel
            {
                FilmId = FilmModel.FilmId,
                PersonId = selectedCastMemberId
            });
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
            FilmDisplayModel.SelectedCastMemberIds = FilmDisplayModel.SelectedCastMemberIds.Append(filmPerson.PersonId);
        }
    }
}
