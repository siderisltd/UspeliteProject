namespace Uspelite.Services.Data.Contracts
{
    using System.Linq;
    using Uspelite.Data.Models;

    public interface IRatesService
    {
        IQueryable<Rate> All();

        int SaveChanges();

        int Add(Rate rateToAdd);
    }
}
