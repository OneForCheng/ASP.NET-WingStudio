using System.Data.Entity;

namespace WingStudio.Models
{
    public class WebAppContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public WebAppContext() : base("name=WebAppContext")
        {
        }

        /// <summary>
        /// 内部用户
        /// </summary>
        public  DbSet<User> Users { get; set; }

        /// <summary>
        /// 内部用户信息
        /// </summary>
        public DbSet<UserInfo> UserInfos { get; set; }

        /// <summary>
        /// 内部用户配置
        /// </summary>
        public DbSet<UserConfig> UserConfigs { get; set; }

        /// <summary>
        /// 超级用户
        /// </summary>
        public DbSet<SuperUser> SuperUsers { get; set; }

        /// <summary>
        /// 动态
        /// </summary>
        public DbSet<Dynamic> Dynamics { get; set; }

        /// <summary>
        /// 用户组
        /// </summary>
        public DbSet<UserGroup> UserGroups { get; set; }

        /// <summary>
        /// 资源组
        /// </summary>
        public DbSet<FileGroup> FileGroups { get; set; }

        /// <summary>
        /// 博客组
        /// </summary>
        public DbSet<BlogGroup> BlogGroups { get; set; }


        /// <summary>
        /// 报名组
        /// </summary>
        public DbSet<Application> Applications { get; set; }

        /// <summary>
        /// 内部信息
        /// </summary>
        public DbSet<Message> Messages { get; set; }


        /// <summary>
        /// 数据包
        /// </summary>
        public DbSet<Packet> Packets { get; set; }

        /// <summary>
        /// 公告
        /// </summary>
        public DbSet<Notice> Notices { get; set; }


        /// <summary>
        /// 报名者
        /// </summary>
        public DbSet<Participant> Participants { get; set; }


        /// <summary>
        /// 文件
        /// </summary>
        public DbSet<WebFile> WebFiles { get; set; }


        /// <summary>
        /// 文件夹
        /// </summary>
        public DbSet<WebFolder> WebFolders { get; set; }


        /// <summary>
        /// 博客
        /// </summary>
        public DbSet<Blog> Blogs { get; set; }

        /// <summary>
        /// 博客榜
        /// </summary>
        public DbSet<tolistblog> ToListBlogs { get; set; }

        /// <summary>
        /// 重置码
        /// </summary>
        public DbSet<ResetCode> ResetCodes { get; set; }

        /// <summary>
        /// 收藏夹
        /// </summary>
        public DbSet<Favorites> Favorites { get; set; }

        /// <summary>
        /// 推荐
        /// </summary>
        public DbSet<Recommendations> Recommendations { get; set; }
    }
}
