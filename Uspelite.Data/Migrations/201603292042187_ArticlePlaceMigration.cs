namespace Uspelite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArticlePlaceMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Place", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Place");
        }
    }
}
