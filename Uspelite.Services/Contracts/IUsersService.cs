namespace Uspelite.Services.Data.Contracts
{
    using System.Linq;
    using Uspelite.Data.Models;

    public interface IUsersService
    {
        User GetById(string id);

        User GetByUsername(string username);

        IQueryable<User> All();
    }
}
