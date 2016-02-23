namespace Uspelite.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using DTO;
    using Uspelite.Data.Models;

    public interface IImagesService
    {
        int SaveImage(Image model, ImageFormat imageFormat, bool addBrand = false);

        PictureDTO GetPicturePathsFromSlug(string title);

        Image SaveImageFromWeb(string url, string title, ImageFormat imageFormat, string authorId);
    }
}
