namespace Uspelite.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Data.Models.Enum;
    using Infrastructure.Mapping.Contracts;
    using Models.Articles;
    using Models.ChildActions;
    using Services.Data.Contracts;

    [ChildActionOnly]
    public class ChildActionsController : Controller
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

        [OutputCache(Duration = 5 * 60, VaryByParam = "none")]
        public ActionResult GetSideBar()
        {
            var model = this.articlesService.GetTopCountPostsByRatingInEveryCategory(4).To<CategoryAndPostsViewModel>().ToList();
            return this.PartialView("_Sidebar", model);
        }

        public ActionResult GetClientNavigation()
        {
            var topSevenCategories = this.categoriesService.GetAll().OrderBy(x => x.Ratings.Any() ? x.Ratings.Sum(z => z.Value) : 0).Take(7).AsEnumerable();
            var model = this.articlesService.GetTopCountPostsByRatingInEveryCategory(4, topSevenCategories).To<CategoryAndPostsViewModel>().ToList();
            return this.PartialView("_ClientNavigation", model);
        }
    }
}