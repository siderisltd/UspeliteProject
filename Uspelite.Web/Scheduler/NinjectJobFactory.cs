namespace Uspelite.Web.Scheduler
{
    using Ninject;
    using Ninject.Syntax;
    using Quartz;
    using Quartz.Spi;

    public class NinjectJobFactory : IJobFactory
    {
        private readonly IResolutionRoot resolutionRoot;

        public NinjectJobFactory(IResolutionRoot resolutionRoot)
        {
            this.resolutionRoot = resolutionRoot;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return (IJob)this.resolutionRoot.Get(bundle.JobDetail.JobType);
        }

        public void ReturnJob(IJob job)
        {
            this.resolutionRoot.Release(job);
        }
    }
}