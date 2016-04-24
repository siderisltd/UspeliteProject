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
    using Uspelite.Data.Models;
    using Uspelite.Data.Repositories;
    using Image = Uspelite.Data.Models.Image;

    public class ImagesService : IImagesService
    {
        private readonly IRepository<Image> repo;
        private readonly IImageHelper imageHelper;
        private System.Drawing.Image BrandImage
        {
            get
            {
                ObjectCache cache = MemoryCache.Default;
                System.Drawing.Image brandImage = cache["brandImage"] as System.Drawing.Image;
                if (brandImage == null)
                {
                    var brandImg = System.Drawing.Image.FromFile(Constants.APP_ROOT_PATH + "Content\\Uploads\\Brand\\brand.png");
                    cache["brandImage"] = brandImg;
                    brandImage = brandImg;
                }
                return brandImage;
            }
        }

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
            var imagePath = date + "\\" + (image.Id % Constants.FOLDERS_SEPARATION_COUNT);
            image.PathOriginalSize = imagePath + "\\ori_" + image.Slug + ".jpg";
            image.PathResizedImage = imagePath + "\\400_" + image.Slug + ".jpg";


            var directory = this.rootImagesFolder + imagePath;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            //byte[] originalImageArray = this.imageHelper.GetFromUrlAndBrandImage(url, imageFormat, this.BrandImage);
            byte[] originalImageArray = this.imageHelper.GetFromUrl(url, imageFormat);
            var resizedImageArray400 = this.imageHelper.ScaleImage(this.imageHelper.ByteToImage(originalImageArray), 400, imageFormat);

            File.WriteAllBytes(this.rootImagesFolder + "\\" + image.PathOriginalSize, originalImageArray);
            File.WriteAllBytes(this.rootImagesFolder + "\\" + image.PathResizedImage, resizedImageArray400);

            this.repo.SaveChanges();

            return image;
        }

        public byte[] CropImage(Stream inputStream, Rectangle rectangle)
        {
            var img = System.Drawing.Image.FromStream(inputStream);


            //var quality = 100;
            //var info = ImageCodecInfo.GetImageEncoders();
            //var parameters = new EncoderParameters(1);
            //parameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);

            byte[] byteArrImg;
            using (var bitmapImg = new Bitmap(img))
            {
                Bitmap cropedImg = bitmapImg.Clone(rectangle, bitmapImg.PixelFormat);
                ImageConverter converter = new ImageConverter();
                byteArrImg = (byte[])converter.ConvertTo(cropedImg, typeof(byte[]));
            }

            return byteArrImg;
        }

        public void RemoveAllRelatedToArticle(int id)
        {
            var images = this.repo.All().Where(x => x.ArticleId == id);

            foreach (var img in images)
            {
                this.repo.Delete(img.Id);
            }

            this.repo.SaveChanges();
        }

        public void RemoveAllRelatedToUser(User user)
        {
            foreach (var image in user.ProfileImages)
            {
                image.IsDeleted = true;
                image.IsMainProfilePicture = false;
                image.UserProfilePictureId = null;
            }

            user.ProfileImages = new HashSet<Image>();

            this.repo.SaveChanges();
        }

        public byte[] ToByteArray(Stream inputStream)
        {
           return this.imageHelper.StreamToByteArray(inputStream);
        }


        public int SaveImage(Image image, ImageFormat imageFormat, bool addBrand = false)
        {
            var date = DateTime.Now.ToString("MM.yyyy");

            var imagePath = date + "\\" + (image.Id % Constants.FOLDERS_SEPARATION_COUNT);
            image.PathOriginalSize = imagePath + "\\ori_" + image.Slug + ".jpg";
            image.PathResizedImage = imagePath + "\\400_" + image.Slug + ".jpg";


            var directory = this.rootImagesFolder + imagePath;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            byte[] originalImageArray;

            if (image.AsByteArray.Length < 1)
            {
                originalImageArray = this.imageHelper.StreamToByteArray(image.Stream);
            }
            else
            {
                originalImageArray = image.AsByteArray;
            }


            if (addBrand)
            {
                originalImageArray = this.imageHelper.AddBranding(originalImageArray, this.BrandImage, imageFormat);
            }

            var resizedImageArray400 = this.imageHelper.ScaleImage(this.imageHelper.ByteToImage(originalImageArray), 400, imageFormat);

            File.WriteAllBytes(this.rootImagesFolder + "\\" + image.PathOriginalSize, originalImageArray);
            File.WriteAllBytes(this.rootImagesFolder + "\\" + image.PathResizedImage, resizedImageArray400);

            var result = this.repo.SaveChanges();

            return image.Id;
        }

        public PagedImageDTO AllPaged(int page, int pageSize, int? categoryId = null)
        {
            var dto = new PagedImageDTO();
            var allImagesCount = this.repo.All().Count();
            dto.AllItemsCount = allImagesCount;
            dto.CurrentPage = page;
            dto.TotalPages = (int)Math.Ceiling(allImagesCount / (decimal)pageSize);

            var itemsToSkip = (page - 1) * pageSize;

            var query = this.repo.All();

            if (categoryId != null)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            var images = query
                .OrderByDescending(x => x.CreatedOn)
                .Skip(itemsToSkip)
                .Take(pageSize)
                .AsQueryable();

            dto.Images = images;

            return dto;
        }

    }
}

