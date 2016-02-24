﻿namespace Uspelite.Web.Controllers
{
    using System.Web.Mvc;
    using Services.Data.Contracts;
    using System.Linq;
    using Models.Home;
    using Infrastructure.Mapping.Contracts;
    using Models.Articles;

    public class HomeController : BaseController
    {
        private readonly IArticlesService articlesService;

        public HomeController(IArticlesService articlesService)
        {
            this.articlesService = articlesService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            //Should get even number of items
            var newestPosts = this.Cache.Get(
                "newestPosts",
                () => this.articlesService.GetNewestPosts(16).To<ArticleViewModel>().ToList(), 10);

            //Should get even number of items
            var highRatedPosts = this.Cache.Get(
                "highRatedPosts",
                () => this.articlesService.GetTopPostsByRating(6).To<ArticleViewModel>().ToList(), 10);

            var mostCommentedPosts = this.Cache.Get(
                "mostCommentedPosts",
                () => this.articlesService.GetMostCommented(6).To<ArticleViewModel>().ToList(), 10);

            var highRatedInCategory = this.Cache.Get(
                 "highRatedPostsInCategory",
                 () => this.articlesService.GetTopPostsByRating(6, "Test1").To<ArticleViewModel>().ToList(), 10);

            var topArticleByRating = this.Cache.Get(
                "topArticleByRating",
                () => this.articlesService.GetTopPostsByRating(1).To<ArticleViewModel>().FirstOrDefault(), 10);

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