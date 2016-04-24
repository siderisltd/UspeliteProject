namespace Uspelite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryParentMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "ParentId", c => c.Int());
            CreateIndex("dbo.Categories", "ParentId");
            AddForeignKey("dbo.Categories", "ParentId", "dbo.Categories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categories", "ParentId", "dbo.Categories");
            DropIndex("dbo.Categories", new[] { "ParentId" });
            DropColumn("dbo.Categories", "ParentId");
        }
    }
}
