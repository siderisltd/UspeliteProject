namespace Uspelite.Web.Areas.Management.Controllers
{
    using System.Web.Mvc;
    using Data.Common.Roles;
    using Web.Controllers;
    
    [Authorize(Roles = AppRoles.MANAGER_ROLE)]
    public class ManagerController : BaseController
    {
    }
}