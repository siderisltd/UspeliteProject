namespace Uspelite.Web.Controllers
{
    using System;
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

        public ActionResult Articles(string query, int page = 1, int pageSize = 10)
        {
            var itemsToSkip = (page - 1) * pageSize;

            var result = this.articlesService
                .FullTextSearch(query)
                .Skip(itemsToSkip)
                .Take(pageSize)
                .To<ArticleViewModel>()
                .ToList();


            var resultsCount = (int) Math.Ceiling(result.Count/(decimal) pageSize);
            this.TempData["resultsCount"] = resultsCount;


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