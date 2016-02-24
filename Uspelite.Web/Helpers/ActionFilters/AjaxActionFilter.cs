namespace Uspelite.Web.ActionFilters
{
    using System.Net;
    using System.Web.Mvc;

    public class AjaxActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!(filterContext.HttpContext.Request.IsAjaxRequest()))
            {
                filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.BadRequest,
                    "Only ajax requests are allowed");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}