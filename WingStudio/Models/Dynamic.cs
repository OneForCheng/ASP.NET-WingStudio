using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WingStudio.Models
{
    /// <summary>
    /// 动态
    /// </summary>
    public class Dynamic 
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
        /// 发布时间
        /// </summary>
        public DateTime? PublicTime { get; set; } = null;

        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        [Required]
        public DateTime LastModTime { get; set; } = DateTime.Now;


        /// <summary>
        /// 内容
        /// </summary>
        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public String Content { get; set; }

        /// <summary>
        /// 发布者
        /// </summary>
        public virtual User Publisher { get; set; }

        /// <summary>
        /// 是否公开
        /// </summary>
        public Boolean IsPublic { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        [Required]
        public String Theme { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [Required]
        public String Icon { get; set; } = "Dynamic.jpg";

        /// <summary>
        /// 浏览次数
        /// </summary>
        [Required]
        public Int32 LookCount { get; set; } = 0;

        /// <summary>
        /// 是否比较正式
        /// </summary>
        [Required]
        public Boolean IsFormal { get; set; }
    }
}