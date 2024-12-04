namespace FilmReferenceUI.Shared.Components;

public partial class CreateUpdateFilmComponent
{
    [Inject] protected IGenreHandler GenreHandler { get; set; } = null!;

    [Inject] protected IStudioHandler StudioHandler { get; set; } = null!;

    [Inject] protected IPersonHandler PersonHandler { get; set; } = null!;

    [Parameter] public FilmDisplayModel FilmDisplayModel { get; set; } = new();

    private List<GenreModel> GenreModels { get; set; } = [];

    private List<StudioModel> StudioModels { get; set; } = [];

    private List<PersonModel> PersonModels { get; set; } = [];

    private List<PersonModel> ActorModels { get; set; } = [];

    private List<PersonModel> DirectorModels { get; set; } = [];

    protected override async Task OnParametersSetAsync()
    {
        GenreModels = await GenreHandler.GetGenresAsync();
        GenreModels.Insert(0, new GenreModel
        {
            GenreId = SharedValues.SharedValues.PleaseSelectValue,
            Name = SharedValues.SharedValues.PleaseSelectText,
        });
        StudioModels = await StudioHandler.GetStudiosAsync();
        StudioModels.Insert(0, new StudioModel
        {
            StudioId = SharedValues.SharedValues.PleaseSelectValue,
            Name = SharedValues.SharedValues.PleaseSelectText,
        });
        PersonModels = await PersonHandler.GetPeopleAsync();
        ActorModels = PersonModels
            .Where(p => p.IsActor)
            .ToList();
        DirectorModels = PersonModels
            .Where(p => p.IsDirector)
            .ToList();
    }
}
