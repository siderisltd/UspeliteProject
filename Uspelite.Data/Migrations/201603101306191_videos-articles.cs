namespace Uspelite.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class videosarticles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles_Videos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArticleId = c.Int(nullable: false),
                        VideoId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        ModifiedOn = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId)
                .ForeignKey("dbo.Videos", t => t.VideoId)
                .Index(t => t.ArticleId)
                .Index(t => t.VideoId)
                .Index(t => t.IsDeleted);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Articles_Videos", "VideoId", "dbo.Videos");
            DropForeignKey("dbo.Articles_Videos", "ArticleId", "dbo.Articles");
            DropIndex("dbo.Articles_Videos", new[] { "IsDeleted" });
            DropIndex("dbo.Articles_Videos", new[] { "VideoId" });
            DropIndex("dbo.Articles_Videos", new[] { "ArticleId" });
            DropTable("dbo.Articles_Videos");
        }
    }
}
