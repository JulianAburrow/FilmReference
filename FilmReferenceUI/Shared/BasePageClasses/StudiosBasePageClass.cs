namespace FilmReferenceUI.Shared.BasePageClasses;

public class StudiosBasePageClass : BasePageClass
{
    [Inject] protected IStudioHandler StudioHandler { get; set; } = null!;

    [Parameter] public int StudioId { get; set; }

    protected StudioModel StudioModel { get; set; } = new();

    protected StudioDisplayModel StudioDisplayModel { get; set; } = new();

    protected void CopyDisplayModelToModel()
    {
        StudioModel.Name = StudioDisplayModel.Name;
        StudioModel.Description = StudioDisplayModel.Description;
        StudioModel.PictureName = StudioDisplayModel.PictureName;
        StudioModel.Films = StudioDisplayModel.Films;
    }

    protected void CopyModelToDisplayModel()
    {
        StudioDisplayModel.Name = StudioModel.Name;
        StudioDisplayModel.Description = StudioModel.Description;
        StudioDisplayModel.PictureName = StudioModel.PictureName;
        StudioDisplayModel.Films = StudioModel.Films;
    }
}
