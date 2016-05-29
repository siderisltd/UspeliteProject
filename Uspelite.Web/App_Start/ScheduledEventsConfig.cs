namespace Uspelite.Web
{
    using Quartz;
    using Quartz.Impl;
    using Uspelite.Web.Infrastructure;
    using Uspelite.Web.Scheduler;

    public class ScheduledEventsConfig
    {
        private static IScheduler scheduler;

        public static void Initialize()
        {
            var ninjectJobFactory = new NinjectJobFactory(ObjectFactory.GetKernal());
            scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.JobFactory = ninjectJobFactory;
            scheduler.Start();
        }

        public static void ScheduleJob(IJobDetail job, ITrigger trigger)
        {
            scheduler.ScheduleJob(job, trigger);
        }
    }
}