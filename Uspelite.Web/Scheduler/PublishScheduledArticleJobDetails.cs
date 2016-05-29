namespace Uspelite.Web.Scheduler
{
    using Quartz;

    public class PublishScheduledArticleJobDetails
    {
        public IJobDetail GetJob()
        {
            return JobBuilder
                     .Create<PublishScheduledArticleJob>()
                     .WithIdentity("PublishScheduledArticleJob", "PublishScheduledArticleJobGroup")
                     .Build();
        }

        public ITrigger GetTrigger()
        {
            return TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(10)
                    .RepeatForever())
                .WithPriority(100)
                .Build();
        }
    }
}