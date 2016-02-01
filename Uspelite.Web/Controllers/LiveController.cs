using System;

namespace Uspelite.MVC.Controllers
{
    using System.Web.Mvc;

    public class LiveController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}