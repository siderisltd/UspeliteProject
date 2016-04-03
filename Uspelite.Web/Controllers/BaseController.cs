namespace Uspelite.Web.Controllers
{
    using System.Web.Mvc;
    using AutoMapper;
    using Infrastructure.Mapping;
    using Ninject;
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