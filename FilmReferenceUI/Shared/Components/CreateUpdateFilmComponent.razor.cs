namespace FilmReferenceUI.Shared.Components;

public partial class CreateUpdateFilmComponent
{
    [Parameter] public FilmDisplayModel FilmDisplayModel { get; set; } = new();

    [Parameter] public List<PersonModelLightweight> ActorModels { get; set; } = [];

    [Parameter] public List<PersonModelLightweight> DirectorModels { get; set; } = [];

    [Parameter] public List<GenreModelLightweight> GenreModels { get; set; } = [];
    
    [Parameter] public List<StudioModelLightweight> StudioModels { get; set; } = [];

    private Dictionary<int, string> _actorLookup = [];

    protected override void OnParametersSet()
    {
        _actorLookup = ActorModels?
            .Where(a => a is not null)
            .ToDictionary(a => a.PersonId, a => $"{a.FirstName} {a.LastName}")
            ?? [];
    }

    protected async Task LocalUploadImage(IBrowserFile file)
    {
        await GlobalUploadImage(file);
        FilmDisplayModel.BoxCover = ImageForDisplay;
    }

    protected void LocalRemoveImage()
    {
        GlobalRemoveImage();
        FilmDisplayModel.BoxCover = ImageForDisplay;
    }

    private string GetMultiSelectionText(IReadOnlyList<string> selectedValues)
    {
        if (selectedValues is null || selectedValues.Count == 0)
        {
            return "No cast members have been selected";
        }

        var names = selectedValues
            .Select(s => int.TryParse(s, out var id)
                ? ActorModels.FirstOrDefault(a => a.PersonId == id)
                : null)
            .Where(p => p is not null)
            .Select(p => $"{p!.FirstName} {p.LastName}".Trim())
            .Where(n => !string.IsNullOrEmpty(n))
            .ToList();

        return names.Count == 0 ? "No cast members have been selected" : string.Join(", ", names);
    }
}