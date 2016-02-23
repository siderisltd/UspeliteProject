namespace Uspelite.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    public class HomeController : AdministratorController
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}