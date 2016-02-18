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

        protected IMapper Mapper
        {
            get
            {
                return AutoMapperConfig.Configuration.CreateMapper();
            }
        }
    }
}