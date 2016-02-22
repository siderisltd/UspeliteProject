namespace Uspelite.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Infrastructure.Mapping.Contracts;
    using Models.Articles;
    using Models.Home;
    using Services.Data.Contracts;
    using Data.Common.Roles;
    using Data.Models;
    using Data.Models.Enum;
    using Microsoft.AspNet.Identity;
    using Models.Categories;

    public class ArticlesController : BaseController
    {
        private readonly IArticlesService articlesService;
        private readonly ICategoriesService categoriesService;
        private readonly IImagesService imagesService;

        public ArticlesController(IArticlesService articlesService, ICategoriesService categoriesService, IImagesService imagesService)
        {
            this.articlesService = articlesService;
            this.categoriesService = categoriesService;
            this.imagesService = imagesService;
        }

        public ActionResult Show(string slug)
        {
            var model = this.articlesService.GetBySlug(slug).To<ConcreteArticleViewModel>().FirstOrDefault();
            return this.View(model);
        }

        [HttpGet]
        //[Authorize(Roles = AppRoles.CLIENT_ROLE)]
        public ActionResult Add()
        {
            var model = new ArticlesBindingModel();
            model.AllCategories = this.GetAllCategories();
            return this.View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = AppRoles.CLIENT_ROLE)]
        public ActionResult Add(ArticlesBindingModel model)
        {
            if (this.ModelState.IsValid)
            {
                if (!this.IsImage(model.TitleImage))
                {
                    this.ModelState.AddModelError("NotImageError", new ArgumentException("The provided file is not an image"));
                }
                string imageName = Path.GetFileNameWithoutExtension(model.TitleImage.FileName);

                var userId = this.User.Identity.GetUserId();

                var articleImage = new Image
                {
                    IsMain = true,
                    Title = imageName,
                    Slug = model.Slug,
                    Stream = model.TitleImage.InputStream,
                    AuthorId = userId
                };
                try
                {
                    var imageId = this.imagesService.SaveImage(articleImage, ImageFormat.Jpeg);
                   
                    var articleId = this.articlesService.Add(
                        model.Title,
                        model.Slug,
                        userId,
                        model.Content,
                        PostStatus.Published,
                        model.CategoryId,
                        articleImage);

                    this.TempData["Notification"] = "Статията беше добавена успешно! ID:" + articleId;
                }
                catch (DbUpdateException ex)
                {
                    this.TempData["Alert"] = ex.InnerException.Message;
                    model.AllCategories = this.GetAllCategories();
                    return this.View(model);
                }
                 
                return this.RedirectToAction("Index", "Home");
            }

            model.AllCategories = this.GetAllCategories();
            return this.View(model);
        }

        private bool IsImage(HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg", "bmp" };

            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        private List<SelectListItem> GetAllCategories()
        {
            return this.Cache.Get(
                "allCategories",
                () => this.categoriesService
                          .GetAll().To<CategoryViewModel>()
                          .Select(x => new SelectListItem
                          {
                              Text = x.Title,
                              Value = x.Id.ToString()
                          })
                          .ToList(), 2000);
        }

    }
}