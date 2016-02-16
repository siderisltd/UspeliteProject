namespace Uspelite.Web.Controllers
{

    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Infrastructure.Mapping.Contracts;
    using Models.ChildActions;
    using Services.Data.Contracts;

    [ChildActionOnly]
    public class ChildActionsController : Controller
    {
        private readonly IPostsService postsService;

        public ChildActionsController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        //TODO: Make generic cache that recaches the top from all categories
        [OutputCache(Duration = 5 * 60, VaryByParam = "none")]
        public ActionResult GetSlider()
        {
            var model = this.postsService.GetTopCountPostsByRatingInEveryCategory(4).To<CategoryAndPostsViewModel>().ToList();
            return this.PartialView("_Slider", model);
        }

        [OutputCache(Duration = 5 * 60, VaryByParam = "none")]
        public ActionResult GetSideBar()
        {
            var model = this.postsService.GetTopCountPostsByRatingInEveryCategory(4).To<CategoryAndPostsViewModel>().ToList();
            return this.PartialView("_Sidebar", model);
        }
    }
}