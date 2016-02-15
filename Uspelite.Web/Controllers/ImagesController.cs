namespace Uspelite.Web.Controllers
{
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.Web;
    using System.Web.Mvc;
    using Data.Models;
    using Models.Images;
    using Services.Data.Contracts;

    public class ImagesController : BaseController
    {
        private IImagesService imagesService;

        public ImagesController(IImagesService imagesService)
        {
            this.imagesService = imagesService;
        }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult SaveImage(IEnumerable<HttpPostedFileBase> model)
        { 
            var imagesToDatabase = new List<Image>();
            foreach (var mod  in model)
            {
                var image = new Image()
                            {
                                AltTag = "alt tag",
                                AuthorId = "5a46a4b4-ab5e-486d-909d-2ecc4b7b858d",
                                ImageType = Data.Models.Enum.ImageType.Png,
                                Title = mod.FileName,
                                Stream = mod.InputStream,
                            };

                imagesToDatabase.Add(image);

            }

            this.imagesService.SaveImage(imagesToDatabase, ImageFormat.Png);

            
            var image1 = this.imagesService.GetPicturePathsFromTitle("AGtdd_tulips.jpg");
            var image2 = this.imagesService.GetPicturePathsFromTitle("jWSIu_penguins---copy-(2).jpg");
            var image3 = this.imagesService.GetPicturePathsFromTitle("e_penguins.jpg");

            this.ViewBag.Original1 = image1.OriginalPath;
            this.ViewBag.Original2 = image2.OriginalPath;
            this.ViewBag.Original3 = image3.OriginalPath;
            this.ViewBag.Resized1 = image1.ResizedImage400;
            this.ViewBag.Resized2 = image2.ResizedImage400;
            this.ViewBag.Resized3 = image3.ResizedImage400;

            return this.PartialView("_SaveImage", imagesToDatabase);
        }
    }
}