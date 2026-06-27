namespace FilmReferenceUI.Shared.BasePageClasses;

public abstract class StudiosBasePageClass : BasePageClass
{
    [Inject] protected IStudioHandler StudioHandler { get; set; } = null!;

    [Parameter] public int StudioId { get; set; }

    protected StudioModel StudioModel { get; set; } = new();

    protected StudioDisplayModel StudioDisplayModel { get; set; } = new();

    protected async Task CopyDisplayModelToModel()
    {
        StudioModel.Name = StudioDisplayModel.Name;
        StudioModel.Description = StudioDisplayModel.Description;
        StudioModel.Logo = StudioDisplayModel.Logo;
        StudioModel.Films = StudioDisplayModel.Films;
    }

    protected void CopyModelToDisplayModel()
    {
        StudioDisplayModel.Name = StudioModel.Name;
        StudioDisplayModel.Description = StudioModel.Description;
        StudioDisplayModel.Logo = StudioModel.Logo;
        StudioDisplayModel.Films = StudioModel.Films;
    }
}
