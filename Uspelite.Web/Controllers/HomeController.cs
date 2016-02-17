namespace Uspelite.Web.Controllers
{
    using System.Web.Mvc;
    using Services.Data.Contracts;
    using System.Linq;
    using System.Web.Hosting;
    using Models.Home;
    using Infrastructure.Mapping.Contracts;

    public class HomeController : BaseController
    {
        private readonly IArticlesService postsService;

        public HomeController(IArticlesService postsService)
        {
            this.postsService = postsService;
        }

        public ActionResult Index()
        {
            //Should get even number of items
            var newestPosts = this.Cache.Get(
                "newestPosts",
                () => this.postsService.GetNewestPosts(6).To<ArticleViewModel>().ToList(), 10);

            //Should get even number of items
            var highRatedPosts = this.Cache.Get(
                "highRatedPosts",
                () => this.postsService.GetTopPostsByRating(6).To<ArticleViewModel>().ToList(), 10);

            var mostCommentedPosts = this.Cache.Get(
                "mostCommentedPosts",
                () => this.postsService.GetMostCommented(6).To<ArticleViewModel>().ToList(), 10);

            var highRatedInCategory = this.Cache.Get(
                 "highRatedPostsInCategory",
                 () => this.postsService.GetTopPostsByRating(6, "Test1").To<ArticleViewModel>().ToList(), 10);

            var topArticleByRating = this.Cache.Get(
                "topArticleByRating",
                () => this.postsService.GetTopPostsByRating(1).To<ArticleViewModel>().FirstOrDefault(), 10);

            var indexViewModel = new HomeIndexViewModel
            {
                HighRatedPosts = highRatedPosts,
                NewestPosts = newestPosts,
                MostCommentedPosts = mostCommentedPosts,
                HighRatedInCategory = highRatedInCategory,
                TopArticle = topArticleByRating
            };


            return this.View(indexViewModel);
        }

    }
}