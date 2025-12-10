namespace FilmReferenceUI.Shared.Helpers;

public static class ImageHelper
{
    public static string ImageSource(byte[] imageContent)
    {
        return string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(imageContent));
    }

    public static async Task<MemoryStream> ToMemoryStream(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        return memoryStream;
    }
}
