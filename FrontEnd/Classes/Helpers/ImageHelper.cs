using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace FilmReference.FrontEnd.Classes.Helpers
{
    public class ImageHelper
    {
        public string ImageSource(byte[] imageContent, bool useDefaultImage = true)
        {
            return imageContent != null
                ? string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(imageContent))
                : useDefaultImage
                    ? "/images/NoCurrentPicture.jpg"
                    : "";
        }

        private List<string> AllowedImageTypes()
        {
            return new List<string>
            {
                "jpeg",
                "png",
                "gif"
            };
        }

        public bool FileTypeOk(IFormFile file, out string errorMessage)
        {
            var fileTypeOk = true;
            errorMessage = "";
            var fileType = file.ContentType.Substring(file.ContentType.IndexOf("/") + 1);
            if (!AllowedImageTypes().Contains(fileType))
            {
                fileTypeOk = false;
                errorMessage = "Must be of type .jpg, .gif or .png";
            }
            return fileTypeOk;
        }
    }
}
