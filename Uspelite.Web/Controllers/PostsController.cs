namespace Uspelite.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using Infrastructure.Mapping.Contracts;
    using Models.Home;
    using Services.Data.Contracts;

    //TODO: Rename to ArticlesController and every post stuffs also
    public class PostsController : BaseController
    {
        private readonly IArticlesService postService;

        public PostsController(IArticlesService postService)
        {
            this.postService = postService;
        }

        public ActionResult GetByTitle(string title)
        {
            var model = this.postService.GetByTitle(title).To<ArticleViewModel>().FirstOrDefault();
            return this.View(model);
        }
    }
}