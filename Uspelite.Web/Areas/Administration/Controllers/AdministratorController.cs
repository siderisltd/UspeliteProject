namespace Uspelite.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;
    using Data.Common.Roles;
    using Web.Controllers;

    [Authorize(Roles = AppRoles.ADMIN_ROLE)] 
    public class AdministratorController : BaseController
    {
    }
}