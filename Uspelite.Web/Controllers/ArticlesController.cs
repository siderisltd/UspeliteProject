namespace Uspelite.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using ActionFilters;
    using Data.Common.Roles;
    using Infrastructure.Mapping.Contracts;
    using Models.Articles;
    using Services.Data.Contracts;
    using Data.Models.Enum;
    using Helpers.Attributes;
    using Microsoft.AspNet.Identity;
    using Models.Categories;
    using Image = Data.Models.Image;

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
            slug = this.Server.UrlEncode(slug);
            var model = this.articlesService.GetBySlug(slug).To<ConcreteArticleViewModel>().FirstOrDefault();
            if (model == null)
            {
                return this.HttpNotFound();
            }

            if (this.User.Identity.IsAuthenticated)
            {
                if (this.User.IsInRole(AppRoles.ADMIN_ROLE) || this.User.IsInRole(AppRoles.MANAGER_ROLE) || (this.User.IsInRole(AppRoles.EDITOR_ROLE) && this.User.Identity.GetUserId() == model.Author.Id))
                {
                    model.ShowEdit = true;
                }
            }
           
            return this.View(model);
        }

        [HttpGet]
        [AjaxActionFilter]
        public ActionResult BuildLinkView()
        {
            return this.PartialView("_BuildLinkView");
        }

        [HttpGet]
        [AjaxActionFilter]
        public ActionResult GetArticlesFilteredByTitle(string searchQuery)
        {
            List<SelectListItem> articles = this.articlesService
                .GetAllFilteredByTitle(searchQuery)
                .To<BuildLinkArticleModel>()
                .Select(x => new SelectListItem
                {
                    Text = x.Title,
                    Value = x.Slug
                })
                .ToList();

            var model = new ArticlesSearchByTitleViewModel { Articles = articles };

            return this.PartialView("_FilteredArticlesByTitleDropdown", model);
        }

        [HttpGet]
        public ActionResult Add(string Id)
        {
            var model = new ArticlesBindingModel();
            if (!string.IsNullOrEmpty(Id))
            {
                var foundArticle = this.articlesService.GetById(int.Parse(Id));
                if(foundArticle == null)
                {
                    return this.HttpNotFound("Article with that Id was not found");
                }
                model = this.Mapper.Map<ArticlesBindingModel>(foundArticle);

            }

            model.AllCategories = this.GetAllCategories();
            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles(AppRoles.EDITOR_ROLE, AppRoles.MANAGER_ROLE, AppRoles.ADMIN_ROLE, AppRoles.ULTIMATE_ROLE)]
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

                var x1 = (int)Math.Ceiling(double.Parse(model.X1));
                var x2 = (int)Math.Ceiling(double.Parse(model.X2));

                var y1 = (int)Math.Ceiling(double.Parse(model.Y1));
                var y2 = (int)Math.Ceiling(double.Parse(model.Y2));

                var w = (int)Math.Ceiling(double.Parse(model.W));
                var h = (int)Math.Ceiling(double.Parse(model.H));

                var rect = new Rectangle(x1, y1, w, h);
                var imageAsByteArray = this.imagesService.CropImage(model.TitleImage.InputStream, rect);



                var articleImage = new Image
                {
                    IsMain = true,
                    Title = imageName,
                    Slug = model.Slug,
                    AsByteArray = imageAsByteArray,
                    AuthorId = userId
                };
                try
                {
                    int imageId;
                    if (model.ImportBranding)
                    {
                        imageId = this.imagesService.SaveImage(articleImage, ImageFormat.Jpeg, true);
                    }
                    else
                    {
                        imageId = this.imagesService.SaveImage(articleImage, ImageFormat.Jpeg);
                    }


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