namespace Crowlers
{
    using Uspelite.Services.Data;
    using Uspelite.Services.Data.Contracts;

    public class DarikNewsCrawler
    {
        private readonly IArticlesService articlesService;

        public DarikNewsCrawler(IArticlesService articlesService)
        {
            this.articlesService = articlesService;
        }
    }
}
