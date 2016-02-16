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
        private readonly IPostsService postService;

        public PostsController(IPostsService postService)
        {
            this.postService = postService;
        }

        public ActionResult GetByTitle(string title)
        {
            var model = this.postService.GetByTitle(title).To<PostViewModel>().FirstOrDefault();
            return this.View(model);
        }
    }
}