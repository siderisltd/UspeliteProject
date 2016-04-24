namespace Uspelite.Web.Controllers
{
    using System.Web.Mvc;
    using Data.Common.Roles;
    using Helpers.Attributes;

    [AuthorizeRoles(AppRoles.EDITOR_ROLE, AppRoles.MANAGER_ROLE, AppRoles.ADMIN_ROLE, AppRoles.ULTIMATE_ROLE)]
    public class StaffController : BaseController
    {
        public ActionResult Index()
        {
            return this.View();
        }
    }
}