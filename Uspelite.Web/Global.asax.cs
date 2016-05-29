namespace Uspelite.Web
{
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Infrastructure.Mapping;
    using Quartz;
    using Scheduler;
    using Quartz.Impl;
    using Infrastructure;
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            DbConfig.Initialize();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var autoMapperConfig = new AutoMapperConfig();
            autoMapperConfig.Execute(Assembly.GetExecutingAssembly());

            ScheduledEventsConfig.Initialize();
            var publishScheduledArticleJobDetails = new PublishScheduledArticleJobDetails();
            var publishScheduledArticleJob = publishScheduledArticleJobDetails.GetJob();
            var publishScheduledArticleTrigger = publishScheduledArticleJobDetails.GetTrigger();

            ScheduledEventsConfig.ScheduleJob(publishScheduledArticleJob, publishScheduledArticleTrigger);
        }
    }
}
