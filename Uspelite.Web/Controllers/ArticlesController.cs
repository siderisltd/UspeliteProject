namespace Uspelite.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web;
    using System.Web.Mvc;
    using ActionFilters;
    using Data.Common.Roles;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;
    using Models.Articles;
    using Services.Data.Contracts;
    using Data.Models.Enum;
    using Helpers.Attributes;
    using Microsoft.AspNet.Identity;
    using Models.Categories;
    using Services.Web.Contracts;
    using Image = Data.Models.Image;

    public class ArticlesController : BaseController
    {
        private readonly IArticlesService articlesService;
        private readonly ICategoriesService categoriesService;
        private readonly IImagesService imagesService;
        private readonly ISlugService slugService;
        private readonly IShliokavitsaConvertService _shliokavitsaConvertService;

        public ArticlesController(IArticlesService articlesService, ICategoriesService categoriesService, IImagesService imagesService, ISlugService slugService, IShliokavitsaConvertService _shliokavitsaConvertService)
        {
            this.articlesService = articlesService;
            this.categoriesService = categoriesService;
            this.imagesService = imagesService;
            this.slugService = slugService;
            this._shliokavitsaConvertService = _shliokavitsaConvertService;
        }

        public ActionResult Show(string slug)
        {
            if(this.Server != null)
            {
                slug = this.Server.UrlEncode(slug);
            }
            var model = this.articlesService.GetBySlug(slug).To<ConcreteArticleViewModel>().FirstOrDefault();
            if (model == null)
            {
                return this.HttpNotFound();
            }

            this.ShouldShowEditButton(model);

            int  modelRelatedArticlesNumber = 6;
            string searchWord = this.GetSearchWord(model.Title);

            model.RelatedArticles = new List<ArticleViewModel>();

            var relatedArticlesDto = this.articlesService.FullTextSearch(searchWord, pageSize: modelRelatedArticlesNumber);

            List<ArticleViewModel> relatedArticles = new List<ArticleViewModel>();
            if (!string.IsNullOrEmpty(searchWord))
            {
                relatedArticles.AddRange(relatedArticlesDto.
                    ResultsInTitles
                    .Select(x => x.Item)
                    .Where(x => x.Title != model.Title)
                    .To<ArticleViewModel>()
                    .ToList());
            }
           
            var actualRelatedArticlesCount = relatedArticles.Count;

            if (actualRelatedArticlesCount < modelRelatedArticlesNumber)
            {
                var articlesToTake = modelRelatedArticlesNumber - actualRelatedArticlesCount;
                relatedArticles.AddRange(relatedArticlesDto
                    .ResultsInContents
                    .Take(articlesToTake)
                    .Select(x => x.Item)
                    .Where(x => x.Title != model.Title)
                    .To<ArticleViewModel>()
                    .ToList());
            }
            actualRelatedArticlesCount = relatedArticles.Count;
            if (actualRelatedArticlesCount < modelRelatedArticlesNumber)
            {
                var currentModelCategory = model.Category;
                var articlesToTake = modelRelatedArticlesNumber - actualRelatedArticlesCount + 1;
                var newestArticlesInCategory  = this.articlesService
                    .GetNewestPosts(articlesToTake, currentModelCategory.Title)
                    .Where(x => x.Title != model.Title)
                    .To<ArticleViewModel>()
                    .ToList();

                foreach (var article in newestArticlesInCategory)
                {
                    relatedArticles.Add(article);
                }
            }
            actualRelatedArticlesCount = relatedArticles.Count;
            if (actualRelatedArticlesCount < modelRelatedArticlesNumber)
            {
                var currentModelCategory = model.Category;
                var articlesToTake = modelRelatedArticlesNumber - actualRelatedArticlesCount + 1; // because we don't show the same article in the related ones
                var newestArticles = this.articlesService
                    .GetNewestPosts(articlesToTake)
                    .Where(x => x.Title != model.Title)
                    .To<ArticleViewModel>()
                    .ToList();

                foreach (var article in newestArticles)
                {
                    relatedArticles.Add(article);
                }
            }

            model.RelatedArticles = relatedArticles;

            return this.View(model);
        }

        private string GetSearchWord(string title)
        {
            var minWordLength = 3;
            var allWords = title.Split(new char[] { ' ', '.', '/' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var words = allWords.Where(x => x.Length > minWordLength).ToList();
            var result = words.Count > 0 ? words[0] : string.Empty;

            return result;
        }

        private void ShouldShowEditButton(ConcreteArticleViewModel model)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                if (this.User.IsInRole(AppRoles.ADMIN_ROLE) || this.User.IsInRole(AppRoles.MANAGER_ROLE) ||
                    (this.User.IsInRole(AppRoles.EDITOR_ROLE) && this.User.Identity.GetUserId() == model.Author.Id))
                {
                    model.ShowEdit = true;
                }
            }
        }

        [HttpGet]
        [AjaxActionFilter]
        public ActionResult BuildLinkView()
        {
            return this.PartialView("_BuildLinkView");
        }

        [HttpGet]
        [AjaxActionFilter]
        public ActionResult InsertVideo()
        {
            return this.PartialView("_InsertVideo");
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
        [AuthorizeRoles(AppRoles.EDITOR_ROLE, AppRoles.MANAGER_ROLE, AppRoles.ADMIN_ROLE, AppRoles.ULTIMATE_ROLE)]
        public ActionResult Add(string Id)
        {
            var model = new ArticlesBindingModel();
            if (!string.IsNullOrEmpty(Id))
            {
                var foundArticle = this.articlesService.GetById(int.Parse(Id));

                if (foundArticle == null)
                {
                    return this.HttpNotFound("Article with that Id was not found");
                }

                if(!this.User.IsInRole(AppRoles.MANAGER_ROLE) && !this.User.IsInRole(AppRoles.ADMIN_ROLE) && !this.User.IsInRole(AppRoles.ULTIMATE_ROLE))
                {
                    //if it is inside the only possible role is [EDITOR.ROLE] and it must be the owner of the article
                    var userId = this.User.Identity.GetUserId();
                    if(userId != foundArticle.AuthorId)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Naughty user..");
                    }
                }

         
                model = this.Mapper.Map<ArticlesBindingModel>(foundArticle);
                model.MainImageInBase64String = @"data:image/png;base64," + Convert.ToBase64String(System.IO.File.ReadAllBytes(Uspelite.Services.Common.Constants.ROOT_IMAGES_FOLDER + foundArticle.MainImage.PathOriginalSize));
                this.TempData["editmode"] = true;
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
                if (model.Id != 0)
                {
                    var foundArticle = this.articlesService.GetById(model.Id);

                    if (foundArticle == null)
                    {
                        return this.HttpNotFound("Article with that Id was not found");
                    }

                    if (!this.User.IsInRole(AppRoles.MANAGER_ROLE) && !this.User.IsInRole(AppRoles.ADMIN_ROLE) && !this.User.IsInRole(AppRoles.ULTIMATE_ROLE))
                    {
                        //if it is inside the only possible role is [EDITOR.ROLE] and it must be the owner of the article
                        var userId = this.User.Identity.GetUserId();
                        if (userId != foundArticle.AuthorId)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Naughty user..");
                        }
                    }
                }

                try
                {
                    var articleImage = new Image();
                    var userId = this.User.Identity.GetUserId();

                    if (model.TitleImage != null)
                    {
                        if (!this.IsImage(model.TitleImage))
                        {
                            this.ModelState.AddModelError("NotImageError", new ArgumentException("The provided file is not an image"));
                        }

                        string imageName = Path.GetFileNameWithoutExtension(model.TitleImage.FileName);

                        var x1 = (int)Math.Floor(double.Parse(model.X1));
                        var y1 = (int)Math.Floor(double.Parse(model.Y1));
                        var w = (int)Math.Floor(double.Parse(model.W));
                        var h = (int)Math.Floor(double.Parse(model.H));
                        var rect = new Rectangle(x1, y1, w, h);
                        var imageAsByteArray = this.imagesService.CropImage(model.TitleImage.InputStream, rect);


                        //TODO: Remode when importing the new data
                        if (model.Slug == null)
                        {
                            model.Slug = this._shliokavitsaConvertService.Convert(model.Title);
                        }

                        articleImage = new Image
                        {
                            IsMain = true,
                            Title = imageName,
                            Slug = model.Slug,
                            AsByteArray = imageAsByteArray,
                            AuthorId = userId
                        };

                        int imageId;
                        if (model.ImportBranding)
                        {
                            imageId = this.imagesService.SaveImage(articleImage, ImageFormat.Jpeg, true);
                        }
                        else
                        {
                            imageId = this.imagesService.SaveImage(articleImage, ImageFormat.Jpeg);
                        }
                    }
                    if (model.Id != 0)
                    {
                        var articleId = this.articlesService.Update(model.Id,
                                                                     model.Title,
                                                                     userId,
                                                                     model.Content,
                                                                     model.Status,
                                                                     model.CategoryId,
                                                                     articleImage.Slug == null ? null : articleImage);

                        if (Request.IsAjaxRequest())
                        {
                            return this.Json(new { Added = true, Id = articleId, Slug = model.Slug });
                        }
                        this.TempData["Notification"] = "Статията беше редактирана успешно! ID:" + articleId;
                    }
                    else
                    {
                        var articleId = this.articlesService.Add(
                                           model.Title,
                                           model.Slug,
                                           userId,
                                           model.Content,
                                           model.Status,
                                           model.CategoryId,
                                           articleImage);

                        if (Request.IsAjaxRequest())
                        {
                            return this.Json(new { Added = true, Id = articleId, Slug = model.Slug });
                        }

                        this.TempData["Notification"] = "Статията беше добавена успешно! ID:" + articleId;
                    }

                }
                catch (DbUpdateException ex)
                {
                    this.TempData["Alert"] = ex.InnerException.Message;
                    model.AllCategories = this.GetAllCategories();
                    return this.View(model);
                }

                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                List<string> errors = new List<string>();
                var modelErrors = this.ModelState.Where(x => x.Value.Errors.Any()).Select(x => x.Value);

                foreach (var error in modelErrors)
                {
                    foreach (var er in error.Errors)
                    {
                        errors.Add(er.ErrorMessage);
                    }
                }

                this.TempData["Alert"] = errors;

                if (Request.IsAjaxRequest())
                {
                    return this.Json(new { alert = this.TempData["Alert"]});
                }
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