namespace WingStudio.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class mig28 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Favorites", new[] { "ToListBlog_Id" });
            DropIndex("dbo.BlogGroups", new[] { "ToListBlog_Id" });
            DropIndex("dbo.Recommendations", new[] { "ToListBlog_Id" });
            CreateIndex("dbo.Favorites", "tolistblog_Id");
            CreateIndex("dbo.BlogGroups", "tolistblog_Id");
            CreateIndex("dbo.Recommendations", "tolistblog_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Recommendations", new[] { "tolistblog_Id" });
            DropIndex("dbo.BlogGroups", new[] { "tolistblog_Id" });
            DropIndex("dbo.Favorites", new[] { "tolistblog_Id" });
            CreateIndex("dbo.Recommendations", "ToListBlog_Id");
            CreateIndex("dbo.BlogGroups", "ToListBlog_Id");
            CreateIndex("dbo.Favorites", "ToListBlog_Id");
        }
    }
}
