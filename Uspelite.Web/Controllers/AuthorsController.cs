namespace Uspelite.Web.Controllers
{
    using System.Web.Mvc;
    using System.Linq;
    using Data.Models;
    using Models.Authors;
    using Infrastructure.Mapping.Contracts;
    using Services.Data.Contracts;
    using System;
    public class AuthorsController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly IArticlesService articlesService;

        public AuthorsController(IUsersService usersService, IArticlesService articlesService)
        {
            this.usersService = usersService;
            this.articlesService = articlesService;
        }

        public ActionResult Info(string authorId, string authorName, int page = 1, int pageSize = 9)
        {
            var authorNames = authorName.Split(new char[] {' ', '-'});
            
            if(authorNames.Length < 2)
            {
                return this.HttpNotFound("Author should have 2 names");
            }
            var user = this.usersService.Find(authorId, authorNames[0], authorNames[1]);
            if(user == null)
            {
                return this.HttpNotFound("User not found");
            }
            var model = this.Mapper.Map<User, AuthorsUserViewModel>(user);

            var itemsToSkip = (page - 1) * pageSize;

            model.UserArticles = this.articlesService
                .All()
                .Where(x => x.AuthorId == model.Id)
                .OrderByDescending(x => x.CreatedOn)
                .To<AuthorArticleViewModel>()
                .Skip(itemsToSkip)
                .Take(pageSize)
                .ToList();

            var userArticlesCount = user.Articles.Count;
            model.AllItemsCount = userArticlesCount;
            model.TotalPages = (int)Math.Ceiling(userArticlesCount / (decimal)pageSize);
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
    }
}