using System;
using System.ComponentModel.DataAnnotations;

namespace WingStudio.Models
{

    /// <summary>
    /// 超级用户
    /// </summary>
    public class SuperUser : BaseUser
    {
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

    }
}