namespace Uspelite.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Contracts;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Uspelite.Data.Common.Roles;
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

        public User Find(string authorId, string authorFirstName, string authorLastName)
        {
            return this.repo
                .All()
                .FirstOrDefault(x => x.Id == authorId && x.FirstName == authorFirstName && x.LastName == authorLastName);
        }

        public int SaveChanges()
        {
            return this.repo.SaveChanges();
        }

        public User Delete(User user)
        {
            user.IsDeleted = true;
            user.ModifiedOn = DateTime.Now;
            user.DeletedOn = DateTime.Now;
            this.repo.SaveChanges();
            return user;
        }

        public User GetByEmail(string email)
        {
            var resultUser = this.repo.All().FirstOrDefault(x => x.Email == email);
            return resultUser;
        }

        public ICollection<IdentityUserRole> GetAllRolesOfUser(string userId)
        {
            var user = this.repo.GetById(userId);
            var userRoles = user.Roles;
            return userRoles;
        }

        public User Create(User user, string pass, UserManager<User> userManager)
        {
            var asd = userManager.Create(user, pass);
            IdentityResult result = new IdentityResult();
            try
            {
                result = userManager.Create(user, pass);
            }
            catch (Exception ex)
            {     
            }

            //if (result.Succeeded)
            //{
            //    userManager.AddToRole(user.Id, AppRoles.CLIENT_ROLE);
            //}


            return user;
        }
    }
}
