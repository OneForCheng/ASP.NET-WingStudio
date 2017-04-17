namespace WingStudio.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class mig27 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ToListBlogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreateTime = c.DateTime(nullable: false),
                        Content = c.String(nullable: false),
                        Theme = c.String(nullable: false),
                        Type = c.Int(nullable: false),
                        BlogEditor = c.Int(nullable: false),
                        Checked = c.Boolean(nullable: false),
                        CheckId = c.Int(nullable: false),
                        Tag = c.String(),
                        IsPublic = c.Boolean(nullable: false),
                        PublicTime = c.DateTime(),
                        LastModTime = c.DateTime(nullable: false),
                        LookCount = c.Int(nullable: false),
                        Owner_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
            AddColumn("dbo.Favorites", "ToListBlog_Id", c => c.Int());
            AddColumn("dbo.BlogGroups", "ToListBlog_Id", c => c.Int());
            AddColumn("dbo.Recommendations", "ToListBlog_Id", c => c.Int());
            CreateIndex("dbo.Favorites", "ToListBlog_Id");
            CreateIndex("dbo.BlogGroups", "ToListBlog_Id");
            CreateIndex("dbo.Recommendations", "ToListBlog_Id");
            AddForeignKey("dbo.Favorites", "ToListBlog_Id", "dbo.ToListBlogs", "Id");
            AddForeignKey("dbo.BlogGroups", "ToListBlog_Id", "dbo.ToListBlogs", "Id");
            AddForeignKey("dbo.Recommendations", "ToListBlog_Id", "dbo.ToListBlogs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recommendations", "ToListBlog_Id", "dbo.ToListBlogs");
            DropForeignKey("dbo.ToListBlogs", "Owner_Id", "dbo.Users");
            DropForeignKey("dbo.BlogGroups", "ToListBlog_Id", "dbo.ToListBlogs");
            DropForeignKey("dbo.Favorites", "ToListBlog_Id", "dbo.ToListBlogs");
            DropIndex("dbo.ToListBlogs", new[] { "Owner_Id" });
            DropIndex("dbo.Recommendations", new[] { "ToListBlog_Id" });
            DropIndex("dbo.BlogGroups", new[] { "ToListBlog_Id" });
            DropIndex("dbo.Favorites", new[] { "ToListBlog_Id" });
            DropColumn("dbo.Recommendations", "ToListBlog_Id");
            DropColumn("dbo.BlogGroups", "ToListBlog_Id");
            DropColumn("dbo.Favorites", "ToListBlog_Id");
            DropTable("dbo.ToListBlogs");
        }
    }
}
