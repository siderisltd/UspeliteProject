namespace Uspelite.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Uspelite.Data.Models;

    public interface IUsersService
    {
        User GetById(string id);

        User GetByUsername(string username);

        IQueryable<User> All();

        User Find(string authorId, string authorFirstName, string authorLastName);

        int SaveChanges();

        User Delete(User user);

        User GetByEmail(string email);

        ICollection<IdentityUserRole> GetAllRolesOfUser(string userId);

        User Create(User user, string pass, UserManager<User> userManager);
    }
}
