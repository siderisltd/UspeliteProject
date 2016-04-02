namespace Uspelite.Web.Areas.Management.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Data.Models;
    using Data.Models.Enum;
    using Infrastructure.Mapping.Contracts;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Models;
    using Services.Data.Contracts;
    using Web.Models.Categories;

    public class ManageArticlesController : ManagerController
    {
        private readonly IArticlesService articlesService;

        private readonly ICategoriesService categoriesService;

        public ManageArticlesController(IArticlesService articlesService, ICategoriesService categoriesService)
        {
            this.articlesService = articlesService;
            this.categoriesService = categoriesService;
        }

        public ActionResult Index()
        {
            this.ViewData["categories"] = this.categoriesService.GetAll().To<CategoryViewModel>();
            this.ViewData["poststatuses"] = this.articlesService.GetPossibleArticleStatuses();
            this.ViewData["postplaces"] = this.articlesService.GetPossibleArticlePlaces();
            return this.View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var usersToReturn = this.articlesService.All().To<ManageArticleViewModel>().ToDataSourceResult(request);
            return this.Json(usersToReturn);
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, ManageArticleViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var entity = this.articlesService.GetById(model.Id);
                var category = this.categoriesService.GetById(model.Category.Id);
                if (entity == null)
                {
                    this.ModelState.AddModelError("invalidEntity", "Invalid entity id");
                }
                if (category == null)
                {
                    this.ModelState.AddModelError("invalidCategory", "Invalid category id");
                }
                else if(entity != null)
                {
                    entity.Title = model.Title;
                    entity.Category = category;

                    if(model.ArticleStatus != null)
                    {
                        var postStatusAsEnum = (PostStatus) model.ArticleStatus.Id;
                        entity.Status = postStatusAsEnum;
                        model.Status = postStatusAsEnum;
                    }
                    if(model.ArticlePlaceType != null)
                    {
                        var placeTypeAsBool = (ArticlePlaceType) model.ArticlePlaceType.Id;
                        var previousArticleWithSamePlaceType = this.articlesService.All().FirstOrDefault(x => x.Place == placeTypeAsBool);
                        if(previousArticleWithSamePlaceType != null)
                        {
                            previousArticleWithSamePlaceType.Place = ArticlePlaceType.Normal;
                        }
                        entity.Place = placeTypeAsBool;
                        model.Place = placeTypeAsBool;
                    }

                    this.articlesService.SaveChanges();
                }
            }
            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, ManageArticleViewModel model)
        {
            this.articlesService.DeleteById(model.Id);
            this.articlesService.SaveChanges();
            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }
    }
}