namespace Uspelite.Web
{
    using System.Data.Entity;
    using Uspelite.Data;
    using Uspelite.Data.Migrations;

    public class DbConfig
    {
        public static void Initialize()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<UspeliteDbContext, Configuration>());

            UspeliteDbContext.Create().Database.Initialize(true);
        }
    }
}