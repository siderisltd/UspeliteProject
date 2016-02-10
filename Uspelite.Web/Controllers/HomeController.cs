namespace Uspelite.Web.Controllers
{
    using System.Web.Mvc;
    using Services.Data.Contracts;

    public class HomeController : BaseController
    {
        private readonly IPostsService postsService;

        public HomeController(IPostsService postsService)
        {
            this.postsService = postsService;
        }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult About()
        {
            return this.View();
        }
    }
}