namespace Uspelite.Services.Common.Contracts
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    public interface IImageHelper
    {
        byte[] ScaleImage(Image image, int maxWidth, int maxHeight, ImageFormat format);

        byte[] ScaleImage(Image image, int longestSide, ImageFormat format);

        byte[] GetFromUrl(string url, ImageFormat format);

        byte[] GetFromUrl(string pictureLink);

        byte[] GetFromUrl(string url, int longestSide, ImageFormat format);

        byte[] StreamToByteArray(Stream input);

        byte[] ImageToByte(Image img, ImageFormat format);

        Image ByteToImage(byte[] array);
    }
}
