namespace Uspelite.Web
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure.Interception;
    using Fissoft.EntityFramework.Fts;
    using Uspelite.Data;
    using Uspelite.Data.Migrations;

    public class DbConfig
    {
        public static void Initialize()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<UspeliteDbContext, Configuration>());
            UspeliteDbContext.Create().Database.Initialize(true);
            DbInterceptors.Init();
        }
    }
}