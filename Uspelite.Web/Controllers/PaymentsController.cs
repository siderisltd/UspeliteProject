using System.Web.Mvc;

namespace Uspelite.Web.Controllers
{
    public class PaymentsController : BaseController
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}