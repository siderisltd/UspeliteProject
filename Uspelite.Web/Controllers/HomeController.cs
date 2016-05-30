namespace Uspelite.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Services.Data.Contracts;
    using System.Linq;
    using Models.Home;
    using Infrastructure.Mapping.Contracts;
    using Models.Articles;
    using System.Collections.Generic;
    using System.Web.Security;
    using Areas.Administration.Models.Users;
    using Data.Common.Roles;
    using Models.Authors;
    using Data.Models.Enum;
    public class HomeController : BaseController
    {
        private readonly IArticlesService articlesService;
        private readonly IUsersService usersService;
        private readonly ICategoriesService categoriesService;

        public HomeController(IArticlesService articlesService, IUsersService usersService, ICategoriesService categoriesService)
        {
            this.articlesService = articlesService;
            this.usersService = usersService;
            this.categoriesService = categoriesService;
        }

        [HttpGet]
        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            var itemsToSkip = (page - 1) * pageSize;
            var articlesCount = 5;

            var newestArticlesInEachCategory = this.Cache.Get(
                "newestArticlesInEachCategory",
                () => this.articlesService
                          .GetTopArticles(Infrastructure.Enums.ArticleTopFactor.Newest, articlesCount)
                          .To<CategoriesAndArticlesViewModel>()      
                          .OrderByDescending(x => x.Category.HomePriority)
                          .Skip(itemsToSkip)
                          .Take(pageSize)
                          .ToList(), 10);


            //Should get even number of items
            var highRatedPosts = this.Cache.Get(
                "highRatedPosts",
                () => this.articlesService.GetTopPostsByRating(9).To<ArticleViewModel>().ToList(), 10);

            var mostCommentedPosts = this.Cache.Get(
                "mostCommentedPosts",
                () => this.articlesService.GetMostCommented(9).To<ArticleViewModel>().ToList(), 10);


            var model = new HomeIndexViewModel
            {
                HighRatedPosts = highRatedPosts,
                NewestPostsAndCategories = newestArticlesInEachCategory,
                MostCommentedPosts = mostCommentedPosts
            };
 
            model.AllItemsCount = this.Cache.Get("allArticlesCount", () => (this.categoriesService.GetAll().Count(x => x.Articles.Any() && x.Articles.Any(y => y.Status == PostStatus.Published)) * articlesCount), 10);
            model.TotalPages = (int)Math.Ceiling(model.AllItemsCount / ((decimal)pageSize * articlesCount));
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

            return this.View(model);
        }

        [HttpGet]
        public ActionResult About()
        {

            var usersWithAllowedRoles = this.Cache.Get("usersWithAllowedRoles", () =>
            this.usersService
                          .GetUsersByRoleNames(AppRoles.ADMIN_ROLE, AppRoles.MANAGER_ROLE)
                          .OrderBy(x => x.CreatedOn)
                          .To<AuthorsUserViewModel>()
                          .ToList(), 10 * 5);


            var model = new AboutViewModel
            {
                Users = usersWithAllowedRoles
            };

            return this.View(model);
        }
    }
}