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

        public ActionResult Update([DataSourceRequest]DataSourceRequest request, ManageArticleViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var entity = this.articlesService.GetById(model.Id);
                entity.Title = model.Title;
                this.articlesService.SaveChanges();
            }

            //sushtiq tertip pak update vame i go vrushtame

            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, ManageArticleViewModel model)
        {
            this.articlesService.DeleteById(model.Id);
            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }
    }
}