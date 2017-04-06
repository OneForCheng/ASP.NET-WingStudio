using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WingStudio.Models
{

    /// <summary>
    /// 消息
    /// </summary>
    public class BaseMessage
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
    }

    /// <summary>
    /// 基本用户
    /// </summary>
    public class BaseUser
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [Required]
        public Int32 Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public String Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public String Password { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 组
    /// </summary>
    public class Group
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
    }
}