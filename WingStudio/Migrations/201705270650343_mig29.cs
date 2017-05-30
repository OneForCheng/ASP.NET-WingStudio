namespace WingStudio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig29 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Favorites", "tolistblog_Id", "dbo.tolistblogs");
            DropForeignKey("dbo.BlogGroups", "tolistblog_Id", "dbo.tolistblogs");
            DropForeignKey("dbo.tolistblogs", "Owner_Id", "dbo.Users");
            DropForeignKey("dbo.Recommendations", "tolistblog_Id", "dbo.tolistblogs");
            DropIndex("dbo.Favorites", new[] { "tolistblog_Id" });
            DropIndex("dbo.BlogGroups", new[] { "tolistblog_Id" });
            DropIndex("dbo.Recommendations", new[] { "tolistblog_Id" });
            DropIndex("dbo.tolistblogs", new[] { "Owner_Id" });
            DropColumn("dbo.Favorites", "tolistblog_Id");
            DropColumn("dbo.BlogGroups", "tolistblog_Id");
            DropColumn("dbo.Recommendations", "tolistblog_Id");
            DropTable("dbo.tolistblogs");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.tolistblogs",
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Recommendations", "tolistblog_Id", c => c.Int());
            AddColumn("dbo.BlogGroups", "tolistblog_Id", c => c.Int());
            AddColumn("dbo.Favorites", "tolistblog_Id", c => c.Int());
            CreateIndex("dbo.tolistblogs", "Owner_Id");
            CreateIndex("dbo.Recommendations", "tolistblog_Id");
            CreateIndex("dbo.BlogGroups", "tolistblog_Id");
            CreateIndex("dbo.Favorites", "tolistblog_Id");
            AddForeignKey("dbo.Recommendations", "tolistblog_Id", "dbo.tolistblogs", "Id");
            AddForeignKey("dbo.tolistblogs", "Owner_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.BlogGroups", "tolistblog_Id", "dbo.tolistblogs", "Id");
            AddForeignKey("dbo.Favorites", "tolistblog_Id", "dbo.tolistblogs", "Id");
        }
    }
}
