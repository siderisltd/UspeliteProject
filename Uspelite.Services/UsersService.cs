namespace Uspelite.Services
{
    using System.Linq;
    using Contracts;
    using Data.Models;
    using Data.Repositories;

    public class UsersService : IUsersService
    {
        private IRepository<User> usersRepo;

        public UsersService(IRepository<User> usersRepo)
        {
            this.usersRepo = usersRepo;
        }

        public User GetById(string id)
        {
            var user = this.usersRepo.GetById(id);
            return user;
        }

        public User GetByUsername(string username)
        {
            var user = this.usersRepo
                           .All()
                           .FirstOrDefault(x => x.UserName == username);

            return user;
        }
    }
}
