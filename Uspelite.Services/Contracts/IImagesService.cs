namespace Uspelite.Services.Data.Contracts
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using DTO;
    using Image = Uspelite.Data.Models.Image;

    public interface IImagesService
    {
        int SaveImage(Image model, ImageFormat imageFormat, bool addBrand = false);

        PagedImageDTO AllPaged(int page, int pageSize, int? categoryId = null);

        Image SaveImageFromWeb(string url, string title, ImageFormat imageFormat, string authorId);

        byte[] CropImage(Stream inputStream, Rectangle rectangle);

        void RemoveAllRelatedToUser(string id);

        void RemoveAllRelatedToArticle(int id);

        byte[] ToByteArray(Stream inputStream);
    }
}
