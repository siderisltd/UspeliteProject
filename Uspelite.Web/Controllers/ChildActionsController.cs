namespace Uspelite.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Infrastructure.Mapping.Contracts;
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
        [OutputCache(Duration = 5 * 60, VaryByParam = "none")]
        public ActionResult GetSlider()
        {
            var model = this.articlesService.GetTopCountPostsByRatingInEveryCategory(4).To<CategoryAndPostsViewModel>().ToList();
            return this.PartialView("_Slider", model);
        }

        [OutputCache(Duration = 5 * 60, VaryByParam = "none")]
        public ActionResult GetSideBar()
        {
            var model = this.articlesService.GetTopCountPostsByRatingInEveryCategory(4).To<CategoryAndPostsViewModel>().ToList();
            return this.PartialView("_Sidebar", model);
        }

        [OutputCache(Duration = 5 * 60, VaryByParam = "none")]
        public ActionResult GetClientNavigation()
        {
            var topSevenCategories = this.categoriesService.GetAll().OrderBy(x => x.Ratings.Any() ? x.Ratings.Sum(z => z.Value) : 0).Take(7).AsEnumerable();
            var model = this.articlesService.GetTopCountPostsByRatingInEveryCategory(4, topSevenCategories).To<CategoryAndPostsViewModel>().ToList();
            return this.PartialView("_ClientNavigation", model);
        }
    }
}