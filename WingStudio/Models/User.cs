using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WingStudio.Models
{
    
    /// <summary>
    /// 用户
    /// </summary>
    public class User : BaseUser
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        [Required]
        public String Email { get; set; }

        /// <summary>
        /// 安全提问
        /// </summary>
        [Required]
        public virtual SecurityFlag SecQuestion { get; set; } = SecurityFlag.None;

        /// <summary>
        /// 安全提问答案
        /// </summary>
        public String SecAnswer { get; set; } = "";

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        public String Name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Required]
        public String Sex { get; set; }

        /// <summary>
        ///  年级
        /// </summary>
        [Required]
        public String Grade { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        [Required]
        public Boolean IsForbidden { get; set; } = false;

        /// <summary>
        /// 头像路径
        /// </summary>
        [Required]
        public String Avatar { get; set; } = "Default.jpg";

        /// <summary>
        /// 用户信息
        /// </summary>
        public virtual UserInfo UserInfo { get; set; }

        /// <summary>
        /// 用户配置
        /// </summary>
        public virtual UserConfig UserConfig { get; set; }

        /// <summary>
        /// 收藏夹
        /// </summary>
        public virtual Favorites Favorites { get; set; }

        /// <summary>
        /// 推荐
        /// </summary>
        public virtual Recommendations Recommendations { get; set; }

        /// <summary>
        /// 用户组
        /// </summary>
        public virtual ICollection<UserGroup> Groups { get; set; }

        /// <summary>
        /// 博客
        /// </summary>
        public virtual ICollection<Blog> Blogs { get; set; }

        /// <summary>
        /// 文件
        /// </summary>
        public virtual ICollection<WebFile> WebFiles { get; set; }

        /// <summary>
        /// 文件夹
        /// </summary>
        public virtual ICollection<WebFolder> WebFolders { get; set; }
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [Required]
        public Int32 Id { get; set; }

        /// <summary>
        /// 公开个人信息
        /// </summary>
        [Required]
        public Boolean PublicInfo { get; set; } = false;

        /// <summary>
        /// 公开联系方式
        /// </summary>
        [Required]
        public Boolean PublicContact { get; set; } = false;

        /// <summary>
        /// 班级
        /// </summary>
        public String Class { get; set; } = "";

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; } = null;

        /// <summary>
        /// 当前住址
        /// </summary>
        public String CurrentAddr { get; set; } = "";

        /// <summary>
        /// 家庭住址
        /// </summary>
        public String NativeAddr { get; set; } = "";

        /// <summary>
        /// 爱好
        /// </summary>
        public String Hobby { get; set; } = "";

        /// <summary>
        /// 个人签名
        /// </summary>
        public String Sign { get; set; } = "";

        /// <summary>
        /// 个人说明
        /// </summary>
        [DataType(DataType.MultilineText)]
        public String Introduction { get; set; } = "";

        /// <summary>
        /// QQ号
        /// </summary>
        public String QQ { get; set; } = "";

        /// <summary>
        /// 微信
        /// </summary>
        public String WeiChat { get; set; } = "";

        /// <summary>
        /// 博客
        /// </summary>
        public String Blog { get; set; } = "";

        /// <summary>
        /// 电话
        /// </summary>
        public String Phone { get; set; } = "";
    }

    /// <summary>
    /// 用户配置
    /// </summary>
    public class UserConfig
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [Required]
        public Int32 Id { get; set; }

        /// <summary>
        /// 是否公布博客
        /// </summary>
        [Required]
        public Boolean PublicBlog { get; set; } = true;

        /// <summary>
        /// 博客编辑器
        /// </summary>
        [Required]
        public BlogEditor BlogEditor { get; set; } = BlogEditor.Baidu;
    }

}