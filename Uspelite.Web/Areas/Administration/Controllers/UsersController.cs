namespace Uspelite.Web.Areas.Administration.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Data.Models;
    using Infrastructure.Mapping.Contracts;
    using Kendo.Mvc.UI;
    using Kendo.Mvc.Extensions;
    using Microsoft.AspNet.Identity.Owin;
    using Models.Users;
    using Services.Data.Contracts;


    public class UsersController : AdministratorController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var usersToReturn = this.usersService.All().To<UserViewModel>().ToDataSourceResult(request);

            return this.Json(usersToReturn);
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, UserViewModel model)
        {
            var names = model.FullName.Split(new char[] {' ', '-'}, StringSplitOptions.RemoveEmptyEntries);
            if(names.Length < 2)
            {
                this.ModelState.AddModelError("Invalid name", new ArgumentException());
            }
            if (this.ModelState.IsValid)
            {
                var userToAdd = new User()
                {
                    Email = model.Email,
                    FirstName = names[0],
                    LastName = names[1],
                    UserName = model.Username
                };
                var userManager = this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = this.usersService.Create(userToAdd, model.Password, userManager);

                return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
            }

            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, UserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var userToDelete = this.usersService.GetById(model.Id);
                var deletedUser = this.usersService.Delete(userToDelete);
            }
            
            return this.Json(new[] { model }.ToDataSourceResult(request));
        }

    }
}