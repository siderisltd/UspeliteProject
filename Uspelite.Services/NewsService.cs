namespace Uspelite.Services.Data
{
    using Uspelite.Data.Models;
    using Uspelite.Data.Repositories;

    public class NewsService
    {
        private readonly IRepository<News> repo;

        public NewsService(IRepository<News> repo)
        {
            this.repo = repo;
        }
    }
}
