namespace Uspelite.Services.Data
{
    using Contracts;
    using Uspelite.Data.Models;
    using Uspelite.Data.Repositories;

    public class RatesService : IRatesService
    {
        private readonly IRepository<Rate> repo;

        public RatesService(IRepository<Rate> repo)
        {
            this.repo = repo;
        }
    }
}
