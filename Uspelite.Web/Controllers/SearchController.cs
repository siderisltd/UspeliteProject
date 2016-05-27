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

            var foundItemsDTO = this.articlesService.FullTextSearch(query, page, pageSize);

            var foundItems = foundItemsDTO
                .ResultsInTitles
                .Skip(itemsToSkip)
                .Take(pageSize)
                .Select(x => x.Item)
                .To<ArticleViewModel>()
                .ToList();

            var foundItemsCount = foundItems.Count;
            if (foundItemsCount < pageSize)
            {
                foundItems.AddRange(foundItemsDTO
                                        .ResultsInContents
                                        .OrderByDescending(x => x.Hits)
                                        .Skip(itemsToSkip)
                                        .Take(pageSize - foundItems.Count)
                                        .Select(x => x.Item)
                                        .To<ArticleViewModel>()
                                        .ToList());
            }


            var model = new HomeIndexViewModel
                {
                    SearchResults = foundItems,
                    NewestPostsAndCategories = null,
                    HighRatedPosts = null,
                    MostCommentedPosts = null,
                    HighRatedInCategory = null,
                    TopArticle = null,
                    IsSearchResult = true
                };

            model.Query = query;
            model.AllItemsCount = foundItemsDTO.AllSearchedResultsCount;
            model.TotalPages = (int) Math.Ceiling(model.AllItemsCount / (decimal) pageSize);
            model.PageSize = pageSize;
            model.CurrentPage = page;

            if (page <= 6)
            {
                model.DisplayPageFrom = 1;
                model.DisplayPageTo = 10 > model.TotalPages ? model.TotalPages : 10;
            }
            else if (page > 6)
            {
                var displayTo = page + 4;
                model.DisplayPageTo = model.TotalPages <= displayTo ? model.TotalPages : displayTo;
                model.DisplayPageFrom = model.DisplayPageTo - 9;
            }

            return this.View("~/Views/Home/Index.cshtml", model);
        }
    }
}