namespace Uspelite.Web.Controllers
{
    using System.Web.Mvc;

    [Authorize]
    public class LiveController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}