namespace Uspelite.Web.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Infrastructure.Mapping.Contracts;
    using Models.Articles;
    using Services.Data.Contracts;
    using System.Linq;
    using System.Web.Routing;
    using Models.Home;

    public class SearchController : BaseController
    {
        private readonly IArticlesService articlesService;

        public SearchController(IArticlesService articlesService)
        {
            this.articlesService = articlesService;
        }

        public ActionResult Articles(string query)
        {
            var result = this.articlesService.FullTextSearch(query).To<ArticleViewModel>().ToList();

            var indexViewModel = new HomeIndexViewModel
            {
                NewestPosts = result,
                HighRatedPosts = null,
                MostCommentedPosts = null,
                HighRatedInCategory = null,
                TopArticle = null
            };

            return this.View("~/Views/Home/Index.cshtml", indexViewModel);
        }
    }
}