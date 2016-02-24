namespace Uspelite.Web.Controllers
{
    using System.Web.Mvc;
    using Data.Models;
    using Models.Authors;
    using Services.Data.Contracts;

    public class AuthorsController : BaseController
    {
        private readonly IUsersService usersService;

        public AuthorsController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public ActionResult Info(string authorId, string authorName)
        {
            var authorNames = authorName.Split(new char[] {' ', '-'});
            
            if(authorNames.Length < 2)
            {
                return this.HttpNotFound();
            }
            var user = this.usersService.Find(authorId, authorNames[0], authorNames[1]);
            if(user == null)
            {
                return this.HttpNotFound();
            }
            var model = this.Mapper.Map<User, AuthorsUserViewModel>(user);

            return this.View(model);
        }
    }
}