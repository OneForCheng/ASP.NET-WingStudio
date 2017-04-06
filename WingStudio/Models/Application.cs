using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WingStudio.Models
{

    /// <summary>
    /// 报名组
    /// </summary>
    public class Application 
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
        /// 发布者
        /// </summary>
        public virtual User Publisher { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        [Required]
        public String Theme { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 是否属于正式报名
        /// </summary>
        [Required]
        public Boolean IsFormal { get; set; }

        /// <summary>
        /// 报名者
        /// </summary>
        public virtual ICollection<Participant> Participants { get; set; }
    }

    /// <summary>
    /// 报名者
    /// </summary>
    public class Participant
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [Required]
        public Int32 Id { get; set; }

        /// <summary>
        /// 所属报名
        /// </summary>
        public virtual Application Application { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        public String Name { get; set; }

        /// <summary>
        /// 学号
        /// </summary>
        [Required]
        public String StudentNo { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public String Sex { get; set; } = "";

        /// <summary>
        /// 班级
        /// </summary>
        [Required]
        public String StudentClass { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required]
        public String Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>

        public String Phone { get; set; } = "";

    }
}