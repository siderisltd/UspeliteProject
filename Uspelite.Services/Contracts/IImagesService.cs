namespace Uspelite.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using DTO;
    using Uspelite.Data.Models;

    public interface IImagesService
    {
       void SaveImage(IEnumerable<Image> models, ImageFormat imageFormat);

        PictureDTO GetPicturePathsFromTitle(string title);
    }
}
