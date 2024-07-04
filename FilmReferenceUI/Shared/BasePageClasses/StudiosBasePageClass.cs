using FilmReferenceUI.Models;

namespace FilmReferenceUI.Shared.BasePageClasses;

public class StudiosBasePageClass : BasePageClass
{
    [Inject] protected IStudioHandler StudioHandler { get; set; } = null!;

    protected StudioModel StudioModel { get; set; } = new();

    protected StudioDisplayModel StudioDisplayModel { get; set; } = new();

    protected void CopyDisplayModelToModel()
    {
        StudioModel.Name = StudioDisplayModel.Name;
        StudioModel.Description = StudioDisplayModel.Description;
        StudioModel.Picture = StudioDisplayModel.Picture;
        StudioModel.Films = StudioDisplayModel.Films;
    }
}
