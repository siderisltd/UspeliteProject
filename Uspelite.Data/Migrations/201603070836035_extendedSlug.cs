namespace Uspelite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class extendedSlug : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Articles", new[] { "Slug" });
            DropIndex("dbo.Images", new[] { "Slug" });
            AlterColumn("dbo.Articles", "Slug", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.Images", "Slug", c => c.String(nullable: false, maxLength: 300));
            CreateIndex("dbo.Articles", "Slug", unique: true);
            CreateIndex("dbo.Images", "Slug", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Images", new[] { "Slug" });
            DropIndex("dbo.Articles", new[] { "Slug" });
            AlterColumn("dbo.Images", "Slug", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Articles", "Slug", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Images", "Slug", unique: true);
            CreateIndex("dbo.Articles", "Slug", unique: true);
        }
    }
}
