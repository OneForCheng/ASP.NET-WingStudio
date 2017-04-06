using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WingStudio.Models
{

    /// <summary>
    /// 博客
    /// </summary>
    public class Blog
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [Required]
        public Int32 Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 内容
        /// </summary>
        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public String Content { get; set; }

        /// <summary>
        /// 所有者
        /// </summary>
        public virtual User Owner { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public String Theme { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Required]
        public BlogType Type { get; set; }

        /// <summary>
        /// 博客编辑器
        /// </summary>
        [Required]
        public BlogEditor BlogEditor { get; set; }

        /// <summary>
        /// 是否已经审核
        /// </summary>
        [Required]
        public Boolean Checked { get; set; } = false;

        /// <summary>
        /// 审核人Id
        /// </summary>
        [Required]
        public Int32 CheckId { get; set; } = -1;

        /// <summary>
        /// 标签
        /// </summary>
        public String Tag { get; set; } = "";

        [Required]
        /// <summary>
        /// 是否发布
        /// </summary>
        public Boolean IsPublic { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? PublicTime { get; set; } = null;

        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        [Required]
        public DateTime LastModTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 浏览次数
        /// </summary>
        [Required]
        public Int32 LookCount { get; set; } = 0;

        /// <summary>
        /// 组
        /// </summary>
        public virtual ICollection<BlogGroup> Groups { get; set; }

        /// <summary>
        /// 收藏夹
        /// </summary>
        public virtual ICollection<Favorites> Favorites { get; set; }

        /// <summary>
        /// 推荐
        /// </summary>
        public virtual ICollection<Recommendations> Recommendations { get; set; }
    }

    /// <summary>
    /// 入榜博客
    /// </summary>
    public class tolistblog
    {

        /// <summary>
        /// 关键字
        /// </summary>
        [Required]
        public Int32 Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 内容
        /// </summary>
        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public String Content { get; set; }

        /// <summary>
        /// 所有者
        /// </summary>
        public virtual User Owner { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public String Theme { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Required]
        public BlogType Type { get; set; }

        /// <summary>
        /// 博客编辑器
        /// </summary>
        [Required]
        public BlogEditor BlogEditor { get; set; }

        /// <summary>
        /// 是否已经审核
        /// </summary>
        [Required]
        public Boolean Checked { get; set; } = false;

        /// <summary>
        /// 审核人Id
        /// </summary>
        [Required]
        public Int32 CheckId { get; set; } = -1;

        /// <summary>
        /// 标签
        /// </summary>
        public String Tag { get; set; } = "";

        [Required]
        /// <summary>
        /// 是否发布
        /// </summary>
        public Boolean IsPublic { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime? PublicTime { get; set; } = null;

        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        [Required]
        public DateTime LastModTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 浏览次数
        /// </summary>
        [Required]
        public Int32 LookCount { get; set; } = 0;

        /// <summary>
        /// 组
        /// </summary>
        public virtual ICollection<BlogGroup> Groups { get; set; }

        /// <summary>
        /// 收藏夹
        /// </summary>
        public virtual ICollection<Favorites> Favorites { get; set; }

        /// <summary>
        /// 推荐
        /// </summary>
        public virtual ICollection<Recommendations> Recommendations { get; set; }
    }
}