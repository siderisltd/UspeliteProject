namespace Uspelite.Web.Controllers
{
    using System.Web.Mvc;

    [Authorize]
    public class LiveController : Controller
    {

        //TODO: Add elmah
        public ActionResult Index()
        {
            return this.View();
        }
    }
}