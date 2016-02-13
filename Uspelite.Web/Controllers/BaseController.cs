namespace Uspelite.Web.Controllers
{
    using System;
    using System.Security.Principal;
    using System.Web.Mvc;
    using System.Web.Routing;
    using AutoMapper;
    using Data.Models;
    using Infrastructure.Mapping;
    using Ninject;
    using Services.Data.Contracts;
    using Services.Web.Contracts;

    public abstract class BaseController : Controller
    {
        [Inject]
        public IHttpCacheService Cache { get; set; }

        [Inject]
        public IUsersService UsersService { get; set; }

        protected User UserProfile { get; private set; }

        private IPrincipal user;

        protected IMapper Mapper
        {
            get
            {
                return AutoMapperConfig.Configuration.CreateMapper();
            }
        }

        protected override void Initialize(RequestContext requestContext)
        {
            this.user = requestContext.HttpContext.User;
          
            base.Initialize(requestContext);
        }

        protected override void EndExecute(IAsyncResult asyncResult)
        {
            if (this.user != null)
            {
                this.UserProfile = this.UsersService.GetByUsername(this.user.Identity.Name);
            }

            base.EndExecute(asyncResult);
        }
    }
}