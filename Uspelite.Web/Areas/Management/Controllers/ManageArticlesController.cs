namespace Uspelite.Web.Areas.Management.Controllers
{
    using System.Web.Mvc;
    using Infrastructure.Mapping.Contracts;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Models;
    using Services.Data.Contracts;

    public class ManageArticlesController : ManagerController
    {
        private readonly IArticlesService articlesService;

        public ManageArticlesController(IArticlesService articlesService)
        {
            this.articlesService = articlesService;
        }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var usersToReturn = this.articlesService.All().To<ManageArticleViewModel>().ToDataSourceResult(request);

            return this.Json(usersToReturn);
        }

        public ActionResult Create([DataSourceRequest]DataSourceRequest request, ManageArticleViewModel model)
        {
            if (this.ModelState.IsValid)
            {

            }

            //dobavqme go i go vrushtame kato json eto taka
            //mojem da vurnem i modelstate, ako ima greshki kendo da gi vizualizira
            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, ManageArticleViewModel model)
        {
            if (this.ModelState.IsValid)
            {

            }

            //sushtiq tertip pak update vame i go vrushtame

            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, ManageArticleViewModel model)
        {
            //delete the data na sushtiq tertip
            return this.Json(new[] { model }.ToDataSourceResult(request));
        }
    }
}