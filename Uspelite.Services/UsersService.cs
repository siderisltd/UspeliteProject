namespace Uspelite.Services.Data
{
    using System.Linq;
    using Contracts;
    using Uspelite.Data.Models;
    using Uspelite.Data.Repositories;

    public class UsersService : IUsersService
    {
        private readonly IRepository<User> repo;

        public UsersService(IRepository<User> repo)
        {
            this.repo = repo;
        }

        public User GetById(string id)
        {
            var user = this.repo.GetById(id);
            return user;
        }

        public User GetByUsername(string username)
        {
            var user = this.repo
                           .All()
                           .FirstOrDefault(x => x.UserName == username);

            return user;
        }

        public IQueryable<User> All()
        {
            return this.repo.All();
        }
    }
}
