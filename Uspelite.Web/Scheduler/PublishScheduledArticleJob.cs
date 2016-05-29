namespace Uspelite.Web.Scheduler
{
    using Quartz;
    using Services.Data.Contracts;
    using System.Diagnostics;

    public class PublishScheduledArticleJob : IJob
    {
        private readonly IArticlesService articlesService;

        public PublishScheduledArticleJob(IArticlesService articlesService)
        {
            this.articlesService = articlesService;
        }

        public void Execute(IJobExecutionContext context)
        {
            this.articlesService.PublishScheduledArticles();
        }
    }
}