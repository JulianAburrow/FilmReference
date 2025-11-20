using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace FilmReferenceUI.Shared.BasePageClasses;

public class StudiosBasePageClass : BasePageClass
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

        if (Image != null)
        {
            var imageMemoryStream = await ToMemoryStreamAsync(Image.OpenReadStream(MaxFileSize));
            StudioModel.Logo = imageMemoryStream.ToArray();
            Image = null;
        }
        else
        {
            StudioModel.Logo = StudioDisplayModel.Logo;
        }
    }

    protected void CopyModelToDisplayModel()
    {
        StudioDisplayModel.Name = StudioModel.Name;
        StudioDisplayModel.Description = StudioModel.Description;
        StudioDisplayModel.Logo = StudioModel.Logo;
        StudioDisplayModel.Films = StudioModel.Films;
    }
}
