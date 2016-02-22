namespace Uspelite.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.Web;
    using System.Web.Mvc;
    using Data.Models;

    using Services.Data.Contracts;
    using Services.Data.DTO;

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

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult SaveImage(IEnumerable<HttpPostedFileBase> model)
        //{
        //    var imagesToDatabase = new List<Image>();
        //    foreach (var mod in model)
        //    {
        //        var image = new Image()
        //        {
        //            AltTag = "alt tag",
        //            AuthorId = "5a46a4b4-ab5e-486d-909d-2ecc4b7b858d",
        //            ImageType = Data.Models.Enum.ImageType.Png,
        //            Title = mod.FileName,
        //            Stream = mod.InputStream,
        //        };

        //        imagesToDatabase.Add(image);

        //    }

        //    this.imagesService.SaveImage(imagesToDatabase, ImageFormat.Png);


        //    PictureDTO viewModel = this.imagesService.GetPicturePathsFromSlug("QOjZ_desert.jpg");


        //    return this.PartialView("_SaveImage", viewModel);
        //}
    }
}