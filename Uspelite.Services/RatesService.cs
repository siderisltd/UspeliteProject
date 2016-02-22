namespace Uspelite.Services.Data
{
    using System.Linq;
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

        public IQueryable<Rate> All()
        {
            return this.repo.All();
        }

        public int SaveChanges()
        {
            return this.repo.SaveChanges();
        }

        public int Add(Rate rateToAdd)
        {
            this.repo.Add(rateToAdd);
            this.repo.SaveChanges();
            return rateToAdd.Id;
        }
    }
}
