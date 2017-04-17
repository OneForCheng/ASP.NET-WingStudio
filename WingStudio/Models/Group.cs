using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WingStudio.Models
{
    
    /// <summary>
    /// 管理员组
    /// </summary>
    public class UserGroup : Group
    {
        /// <summary>
        /// 主题
        /// </summary>
        [Required]
        public String Theme { get; set; }

        /// <summary>
        /// 内部用户
        /// </summary>
        public virtual ICollection<User> Users { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        [Required]
        public int Authority { get; set; }
    }

    /// <summary>
    /// 资源组
    /// </summary>
    public class FileGroup : Group
    {
        /// <summary>
        /// 主题
        /// </summary>
        [Required]
        public String Theme { get; set; }

        /// <summary>
        /// 资源组图标
        /// </summary>
        [Required]
        public String Icon { get; set; } = "FileGroup.jpg";

        /// <summary>
        /// 可访问性
        /// </summary>
        [Required]
        public Accessible Accessible { get; set; } = Accessible.Inner;

        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        public String Description { get; set; }

        /// <summary>
        /// 文件集合
        /// </summary>
        public virtual ICollection<WebFile> WebFiles { get; set; }
    }

    /// <summary>
    /// 博客组
    /// </summary>
    public class BlogGroup : Group
    {
        /// <summary>
        /// 主题
        /// </summary>
        [Required]
        public String Theme { get; set; }

        /// <summary>
        /// 博客组图标
        /// </summary>
        [Required]
        public String Icon { get; set; } = "BlogGroup.jpg";

        /// <summary>
        /// 可访问性
        /// </summary>
        [Required]
        public Accessible Accessible { get; set; } = Accessible.Inner;

        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        public String Description { get; set; } 

        /// <summary>
        /// 博客集合
        /// </summary>
        public virtual ICollection<Blog> Blogs { get; set; }
    }

    /// <summary>
    /// 收藏夹
    /// </summary>
    public class Favorites : Group
    {
        /// <summary>
        /// 博客
        /// </summary>
        public virtual ICollection<Blog> Blogs { get; set; }
    }

    /// <summary>
    /// 推荐
    /// </summary>
    public class Recommendations : Group
    {
        /// <summary>
        /// 博客
        /// </summary>
        public virtual ICollection<Blog> Blogs { get; set; }
    }
}