namespace Uspelite.Services.Data
{
    using System;
    using System.Runtime.Caching;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Transactions;
    using System.Linq;
    using Common;
    using Common.Contracts;
    using Contracts;
    using DTO;
    using Uspelite.Data.Repositories;
    using Image = Uspelite.Data.Models.Image;

    public class ImagesService : IImagesService
    {
        private readonly IRepository<Image> repo;
        private readonly IImageHelper imageHelper;

        private readonly string rootImagesFolder = Constants.ROOT_IMAGES_FOLDER;

        public ImagesService(IRepository<Image> repo, IImageHelper imageHelper)
        {
            this.repo = repo;
            this.imageHelper = imageHelper;
        }

        public Image SaveImageFromWeb(string url, string title, ImageFormat imageFormat, string authorId)
        {
            var date = DateTime.Now.ToString("MM.yyyy");
            var image = new Image();
            image.Title = title;
            image.AuthorId = authorId;
            this.repo.Add(image);
            this.repo.SaveChanges();
            var imagePath = date + "\\" + (image.Id % Constants.MAX_FILES_IN_DIRECTORY);
            image.PathOriginalSize = imagePath + "\\ori_" + image.Slug + ".jpg";
            image.PathResizedImage = imagePath + "\\400_" + image.Slug + ".jpg";


            var directory = this.rootImagesFolder + imagePath;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            //TODO: Remove
            ObjectCache cache = MemoryCache.Default;
            System.Drawing.Image brandImage = cache["brandImage"] as System.Drawing.Image;
            if (brandImage == null)
            {
                var brandImg = System.Drawing.Image.FromFile(Constants.APP_ROOT_PATH + "Content\\Uploads\\Brand\\brand.png");
                cache["brandImage"] = brandImg;
                brandImage = brandImg;
            }

            byte[] originalImageArray = this.imageHelper.GetFromUrlAndBrandImage(url, imageFormat, brandImage);
            var resizedImageArray400 = this.imageHelper.ScaleImage(this.imageHelper.ByteToImage(originalImageArray), 400, imageFormat);

            File.WriteAllBytes(this.rootImagesFolder + "\\" + image.PathOriginalSize, originalImageArray);
            File.WriteAllBytes(this.rootImagesFolder + "\\" + image.PathResizedImage, resizedImageArray400);

            this.repo.SaveChanges();

            return image;
        }

        public int SaveImage(Image image, ImageFormat imageFormat)
        {
            var date = DateTime.Now.ToString("MM.yyyy");

            var imagePath = date + "\\" + (image.Id % Constants.MAX_FILES_IN_DIRECTORY);
            image.PathOriginalSize = imagePath + "\\ori_" + image.Slug + ".jpg";
            image.PathResizedImage = imagePath + "\\400_" + image.Slug + ".jpg";


            var directory = this.rootImagesFolder + imagePath;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var originalImageArray = this.imageHelper.StreamToByteArray(image.Stream);
            var resizedImageArray400 = this.imageHelper.ScaleImage(new Bitmap(image.Stream), 400, imageFormat);

            File.WriteAllBytes(this.rootImagesFolder + "\\" + image.PathOriginalSize, originalImageArray);
            File.WriteAllBytes(this.rootImagesFolder + "\\" + image.PathResizedImage, resizedImageArray400);

            this.repo.SaveChanges();

            return image.Id;
        }

        public PictureDTO GetPicturePathsFromSlug(string slug)
        {
            var image = this.repo
                .All()
                .Where(x => x.Slug == slug)
                .Select(x => new PictureDTO
                {
                    Id = x.Id,
                    OriginalPath = Constants.IMAGES_PREFIX_FROM_ROOT + x.PathOriginalSize,
                    ResizedImage400 = Constants.IMAGES_PREFIX_FROM_ROOT + x.PathResizedImage
                })
                .FirstOrDefault();

            if (image != null)
            {
                return image;
            }

            throw new ArgumentNullException("Image is not found in the database");
        }


    }
}

