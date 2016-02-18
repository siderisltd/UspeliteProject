namespace Uspelite.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;
    using Data.Common.Roles;
    using Web.Controllers;

    //TODO: Change it to administrator role
    [Authorize(Roles = AppRoles.ULTIMATE_ROLE)] 
    public class AdministratorController : BaseController
    {
    }
}