namespace Uspelite.Web.Infrastructure
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Common;

    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(filterContext.ActionParameters.Any(x => x.Value == null))
            {
                filterContext.Controller.ViewData.ModelState.AddModelError(string.Empty, Constants.RequestCannotBeEmpty);
            }

            if (!filterContext.Controller.ViewData.ModelState.IsValid)
            {
                var error = filterContext.Controller.ViewData
                    .ModelState
                    .Values
                    .SelectMany(v => v.Errors.Select(er => er.ErrorMessage))
                    .First();

                //actionContext.Response = actionContext.Request.CreateResponse(new ResultObject(false, error));
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
