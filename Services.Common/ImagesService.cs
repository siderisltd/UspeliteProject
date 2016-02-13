namespace Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts;
    using DTO;
    using Uspelite.Data.Models;
    using Uspelite.Data.Repositories;
    using Uspelite.Web.Infrastructure;
    using Uspelite.Web.Infrastructure.Contracts;

    public class ImagesService : FileInfoService, IImagesService
    {
        private readonly IImageProcessorService imageProcessor;
        private readonly IRepository<Image> images;

        public ImagesService(IObjectFactory objectFactory, IImageProcessorService imageProcessorService, IRepository<Image> imagesService)
            : base(objectFactory)
        {
            this.imageProcessor = imageProcessorService;
            this.images = imagesService;
        }

        public async Task<IEnumerable<ProcessedImage>> ProcessImages(IEnumerable<RawFile> rawImages)
        {
            var processedImages = await rawImages.ForEachAsync(async rawImage => 
            {
                var image = await this.SaveFileInfo<Image>(rawImage);

                var thumbnailContent = await this.imageProcessor.Resize(rawImage.Content, ProcessedImage.ThumbnailImageWidth);
                var highContent = await this.imageProcessor.Resize(rawImage.Content, ProcessedImage.HighResolutionWidth);

                return ProcessedImage.FromImage(image, thumbnailContent, highContent);
            });

            return processedImages;
        }

        public async Task<bool> ValidateImageUrls(ICollection<string> imageUrls)
        {
            var validImages = await this.ImagesQueryByUrls(imageUrls).CountAsync();
            return validImages == imageUrls.Count();
        }

        public async Task<IEnumerable<Image>> ImagesByUrls(ICollection<string> imageUrls)
        {
            return await this.ImagesQueryByUrls(imageUrls).ToListAsync();
        }

        private IQueryable<Image> ImagesQueryByUrls(ICollection<string> imageUrls)
        {
            return this.images
                .All()
                .Where(i => imageUrls.Contains(i.UrlPath));
        }
    }
}
