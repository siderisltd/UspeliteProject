namespace Uspelite.Data
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public class UspeliteDbContext : IdentityDbContext<User>, IUspeliteDbContext
    {
        public UspeliteDbContext()
            : base("UspeliteDbConnection", throwIfV1Schema: false)
        {
        }

        public IDbSet<Video> Videos { get; set; }

        public IDbSet<Rate> Rates { get; set; }

        public IDbSet<Picture> Pictures { get; set; }

        public IDbSet<Comment> Comments { get; set; }

        public IDbSet<Category> Categories { get; set; }

        public IDbSet<Post> Posts { get; set; }

        public static UspeliteDbContext Create()
        {
            return new UspeliteDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Picture>()
             .HasRequired(c => c.Author)
             .WithMany()
             .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
