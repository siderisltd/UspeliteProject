namespace Uspelite.Web.Areas.Management.Controllers
{
    using System.Web.Mvc;
    using Data.Common.Roles;
    using Web.Controllers;

    //TODO: Only for managers
    [Authorize(Roles = AppRoles.ULTIMATE_ROLE)]
    public class ManagerController : BaseController
    {
    }
}