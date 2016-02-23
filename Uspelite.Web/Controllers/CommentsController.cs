namespace Uspelite.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using ActionFilters;
    using Data.Models;
    using Microsoft.AspNet.Identity;
    using Models.Comments;
    using Services.Data.Contracts;

    public class CommentsController : BaseController
    {
        private readonly ICommentsService commentsService;
        private readonly IArticlesService articlesService;
        private readonly IUsersService usersService;

        public CommentsController(ICommentsService commentsService, IArticlesService articlesService, IUsersService usersService)
        {
            this.commentsService = commentsService;
            this.articlesService = articlesService;
            this.usersService = usersService;
        }
        
        [HttpPost]
        [AjaxActionFilter]
        public ActionResult AddToArticle(CommentsBindingModel model)
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                //todo:proper return
                return null;
            }
            if (!this.ModelState.IsValid)
            {
                //Invalid?
            }

            var userId = this.User.Identity.GetUserId();
            var user = this.usersService.GetById(userId);
            string returnMessage = string.Empty;

            var foundArticle = this.articlesService.All().FirstOrDefault(x => x.Id == model.toId);
            if (foundArticle == null)
            {
                throw new ArgumentNullException("Could not find article with Id: " + model.toId);
            }
            var commentToAdd = new Comment { Author = user, Content = model.Content };
            var comment = this.articlesService.AddCommentTo(foundArticle, commentToAdd);

            var returnObject = this.Mapper.Map<Comment, CommentViewModel>(comment);

            return this.PartialView("_CommentPartial", returnObject);
        }

    }
}