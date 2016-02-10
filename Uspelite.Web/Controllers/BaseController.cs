namespace Uspelite.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Data.Models;
    using Services.Contracts;

    public abstract class BaseController : Controller
    {
        private IUsersService usersService;
        protected User UserProfile { get; private set; }

        protected BaseController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            this.UserProfile = this.usersService.GetByUsername(this.User.Identity.Name);
            return base.BeginExecute(requestContext, callback, state);
        }
    }
}