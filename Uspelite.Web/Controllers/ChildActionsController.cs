namespace Uspelite.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Data.Models;
    using Data.Models.Enum;
    using Infrastructure.Mapping.Contracts;
    using Models.Articles;
    using Models.ChildActions;
    using Services.Data.Contracts;
    using Infrastructure.Enums;
    using Models.Categories;

    [ChildActionOnly]
    public class ChildActionsController : BaseController
    {
        private readonly IArticlesService articlesService;
        private readonly ICategoriesService categoriesService;

        public ChildActionsController(IArticlesService articlesService, ICategoriesService categoriesService)
        {
            this.articlesService = articlesService;
            this.categoriesService = categoriesService;
        }

        //TODO: Make generic cache that recaches the top from all categories when entity is inserted
        [OutputCache(Duration = 5, VaryByParam = "none")]
        public ActionResult GetSlider()
        {
            var sliderArticles = this.articlesService.All().Where(x => x.Place != ArticlePlaceType.Normal).ToList().AsQueryable();
            var getTopArticlesByRating = this.articlesService.GetTopPostsByRating(5).To<ArticleViewModel>().ToList();

            var navCenter = sliderArticles.Where(x => x.Place == ArticlePlaceType.NavCenter).To<ArticleViewModel>().FirstOrDefault();
            var navRightBottom = sliderArticles.Where(x => x.Place == ArticlePlaceType.NavRightBottom).To<ArticleViewModel>().FirstOrDefault();
            var navRightCenter = sliderArticles.Where(x => x.Place == ArticlePlaceType.NavRightCenter).To<ArticleViewModel>().FirstOrDefault();
            var navRightTop = sliderArticles.Where(x => x.Place == ArticlePlaceType.NavRightTop).To<ArticleViewModel>().FirstOrDefault();

            if(navCenter == null)
            {
                navCenter = getTopArticlesByRating[0];
            }

            if (navRightBottom == null)
            {
                navRightBottom = getTopArticlesByRating[1];
            }

            if (navRightCenter == null)
            {
                navRightCenter = getTopArticlesByRating[2];
            }

            if (navRightTop == null)
            {
                navRightTop = getTopArticlesByRating[3];
            }


            var model = new SliderViewModel
            {
                NavCenter = navCenter,
                NavRightBottom = navRightBottom,
                NavRightCenter = navRightCenter,
                NavRightTop = navRightTop
            };

            return this.PartialView("_Slider", model);
        }

        [OutputCache(Duration = 5, VaryByParam = "none")]
        public ActionResult GetSideBar()
        {
            var model = this.articlesService.GetTopArticles(ArticleTopFactor.Rating, 3).To<CategoryAndPostsViewModel>().ToList();
            return this.PartialView("_Sidebar", model);
        }

        [OutputCache(Duration = 5, VaryByParam = "none")]
        public ActionResult GetClientNavigation()
        {
            Dictionary<CategoryViewModel, Dictionary<CategoryViewModel, IEnumerable<ArticleViewModel>>> model =
    new Dictionary<CategoryViewModel, Dictionary<CategoryViewModel, IEnumerable<ArticleViewModel>>>();

            var parentCategories = this.categoriesService.GetParentCategories().Where(x => x.Children.Count > 0).ToList();

            foreach (var parentCat in parentCategories)
            {
                var childCategories = this.categoriesService.GetAll().Where(x => x.ParentId == parentCat.Id).AsEnumerable();

                if (childCategories != null)
                {
                    var allChildCategoriesAndTopArticles =
                            this.articlesService.GetTopArticles(ArticleTopFactor.Newest, 4, childCategories)
                                .To<CategoryAndPostsViewModel>()
                                .ToDictionary(x => x.Category, x => x.Posts);
                   
                    model.Add(this.Mapper.Map<Category, CategoryViewModel>(parentCat), allChildCategoriesAndTopArticles);

                }
            }

            //var allCategoriesAndTopArticles = this.articlesService.GetTopArticles(ArticleTopFactor.Newest, 4).To<CategoryAndPostsViewModel>().ToDictionary(x => x.CategoryName, x => x.Posts);  



            return this.PartialView("_ClientNavigation", model);
        }
    }
}