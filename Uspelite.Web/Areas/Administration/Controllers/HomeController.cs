namespace Uspelite.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;
    using Kendo.Mvc.UI;

    public class HomeController : AdministratorController
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}