﻿namespace Uspelite.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Transactions;
    using System.Linq;
    using System.Web;
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

        public void SaveImage(IEnumerable<Image> images, ImageFormat imageFormat)
        {
            var date = DateTime.Now.ToString("MM.yyyy");
            using (TransactionScope transaction = new TransactionScope())
            {
                foreach (var image in images)
                {
                    this.repo.Add(image);
                    this.repo.SaveChanges();
                    var imagePath = date + "\\" + (image.Id % Constants.MAX_FILES_IN_DIRECTORY);
                    image.PathOriginalSize = imagePath + "\\ori_" + image.Title;
                    image.PathResizedImage = imagePath + "\\400_" + image.Title;


                    var directory = this.rootImagesFolder + imagePath;
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    var originalImageArray = this.StreamToByteArray(image.Stream);
                    var resizedImageArray400 = this.imageHelper.ScaleImage(new Bitmap(image.Stream), 400, imageFormat);

                    File.WriteAllBytes(this.rootImagesFolder + "\\" + image.PathOriginalSize, originalImageArray);
                    File.WriteAllBytes(this.rootImagesFolder + "\\" + image.PathResizedImage, resizedImageArray400);
                }

                transaction.Complete();
            }

            this.repo.SaveChanges();
        }

        public PictureDTO GetPicturePathsFromTitle(string title)
        {
            var image = this.repo
                .All()
                .Where(x => x.Title == title)
                .Select(x => new PictureDTO
                {
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

        private byte[] StreamToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}

