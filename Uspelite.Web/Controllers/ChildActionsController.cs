namespace Uspelite.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Infrastructure.Mapping.Contracts;
    using Models.ChildActions;
    using Models.Home;
    using Services.Data.Contracts;
    using Data.Models;

    [ChildActionOnly]
    public class ChildActionsController : Controller
    {
        private readonly IPostsService postsService;

        public ChildActionsController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        [OutputCache(Duration = 5 * 60, VaryByParam = "none")]
        public ActionResult GetSlider()
        {
            var sliderPosts = this.postsService.GetNewestPosts(6).To<PostViewModel>().ToList();

            var posts = this.postsService.GetTopCountPostsByRatingInEveryCategory(3).To<PostViewModel>().ToList();

            var model = new SliderViewModel()
            {
                Posts = sliderPosts
            };

            return this.PartialView("_Slider", model);
        }
    }
}