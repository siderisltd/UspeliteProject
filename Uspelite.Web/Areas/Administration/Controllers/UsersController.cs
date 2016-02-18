namespace Uspelite.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;
    using Infrastructure.Mapping.Contracts;
    using System.Linq;
    using Infrastructure;
    using Kendo.Mvc.UI;
    using Kendo.Mvc.Extensions;
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
            if (this.ModelState.IsValid)
            {

            }

            //dobavqme go i go vrushtame kato json eto taka
            //mojem da vurnem i modelstate, ako ima greshki kendo da gi vizualizira
            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, UserViewModel model)
        {
            if (this.ModelState.IsValid)
            {

            }

            //sushtiq tertip pak update vame i go vrushtame

            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, UserViewModel model)
        {
            //delete the data na sushtiq tertip
            return this.Json(new[] { model }.ToDataSourceResult(request));
        }

    }
}