namespace Uspelite.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Data.Models;
    using Ninject;
    using Services.Data.Contracts;

    public abstract class BaseController : Controller
    {
        [Inject]
        private IUsersService usersService { get; set; }

        protected User UserProfile { get; private set; }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            this.UserProfile = this.usersService.GetByUsername(this.User.Identity.Name);
            return base.BeginExecute(requestContext, callback, state);
        }
    }
}