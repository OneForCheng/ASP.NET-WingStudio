using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WingStudio.Models
{
    /// <summary>
    /// 重置码
    /// </summary>
    public class ResetCode
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [Required]
        public Int32 Id { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        [Required]
        public String Account { get; set; }

        /// <summary>
        /// 验证码的值
        /// </summary> 
        [Required]
        public String Value { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}