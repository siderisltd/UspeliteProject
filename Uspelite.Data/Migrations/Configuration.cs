namespace Uspelite.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Roles;

    public sealed class Configuration : DbMigrationsConfiguration<Uspelite.Data.UspeliteDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(UspeliteDbContext context)
        {
            this.AddOrUpdateRoles(context);
        }

        private void AddOrUpdateRoles(UspeliteDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            
            foreach (var roleName in AppRoles.AllRoles)
            {
                if (!roleManager.RoleExists(roleName))
                {
                    context.Roles.Add(new IdentityRole(roleName));
                }
            }
        }
    }
}

