namespace Services.Common.Contracts
{
    using System.Collections.Generic;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using DTO;
    using Uspelite.Data.Models;
    using Uspelite.Services.Data.DTO;

    public interface IImagesService
    {
        Task<IEnumerable<ProcessedImage>> ProcessImages(IEnumerable<RawFile> rawImages);

        Task<bool> ValidateImageUrls(ICollection<string> imageUrls);

        Task<IEnumerable<Image>> ImagesByUrls(ICollection<string> imageUrls);
    }
}
