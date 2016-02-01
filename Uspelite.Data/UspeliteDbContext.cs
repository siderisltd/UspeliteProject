namespace Uspelite.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public class UspeliteDbContext : IdentityDbContext<User>, IUspeliteDbContext
    {
        public UspeliteDbContext()
            : base("UspeliteDbConnection", throwIfV1Schema: false)
        {
        }

        public static UspeliteDbContext Create()
        {
            return new UspeliteDbContext();
        }
    }
}
