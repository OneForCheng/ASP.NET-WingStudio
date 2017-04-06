using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WingStudio.Models
{
    
    /// <summary>
    /// 内部信息
    /// </summary>
    public class Message : BaseMessage
    {
        /// <summary>
        /// 所有者Id
        /// </summary>
        [Required]
        public Int32 OwnId { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        [Required]
        public String Theme { get; set; }

        /// <summary>
        /// 是否已经删除
        /// </summary>
        [Required]
        public Boolean Deleted { get; set; } = false;

        /// <summary>
        /// 数据包
        /// </summary>
        public virtual ICollection<Packet> Packets { get; set; }
    }

    /// <summary>
    /// 数据包
    /// </summary>
    public class Packet
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [Required]
        public Int32 Id { get; set; }

        /// <summary>
        /// 目标Id
        /// </summary>
        [Required]
        public Int32 TargetId { get; set; }

        /// <summary>
        /// 是否已经读取
        /// </summary>
        [Required]
        public Boolean Read { get; set; } = false;

        /// <summary>
        /// 是否已经删除
        /// </summary>
        [Required]
        public Boolean Deleted { get; set; } = false;

        /// <summary>
        /// 信息源
        /// </summary>
        [Required]
        public virtual Message Message { get; set; }
    }
}