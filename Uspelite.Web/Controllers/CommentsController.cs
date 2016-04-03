namespace Uspelite.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
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
        
        [Authorize]
        [HttpPost]
        [AjaxActionFilter]
        [ValidateAntiForgeryToken]
        public ActionResult AddToArticle(CommentsBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Post comment modelstate is invalid");
            }

            var userId = this.User.Identity.GetUserId();
            var user = this.usersService.GetById(userId);

            if(user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "You are not logged in MF");
            }

            var foundArticle = this.articlesService.All().FirstOrDefault(x => x.Id == model.toId);
            if (foundArticle == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Could not find article with Id: " + model.toId);
            }
            var commentToAdd = new Comment { Author = user, Content = model.Content };
            var comment = this.articlesService.AddCommentTo(foundArticle, commentToAdd);

            var returnObject = this.Mapper.Map<Comment, CommentViewModel>(comment);

            return this.PartialView("_CommentPartial", returnObject);
        }

    }
}