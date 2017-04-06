using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WingStudio.Models
{
    /// <summary>
    /// 权限标识
    /// </summary>
    public enum AuthorityFlag
    {
        /// <summary>
        /// 无任何权限
        /// </summary>
        None = 0,
        /// <summary>
        /// 管理公告
        /// </summary>
        ManageNotice = 1,
        /// <summary>
        /// 管理动态
        /// </summary>
        ManageDynamic = 2,
        /// <summary>
        /// 管理博客
        /// </summary>
        ManageBlog = 4,
        /// <summary>
        /// 管理文件
        /// </summary>
        ManageFile = 8,
        /// <summary>
        /// 管理报名
        /// </summary>
        ManageApplication = 16,
        /// <summary>
        /// 管理信息
        /// </summary>
        ManageMessage = 32,
        /// <summary>
        /// 全部权限
        /// </summary>
        All = 63,
    }

    /// <summary>
    /// 安全标识
    /// </summary>
    public enum SecurityFlag
    {
        None = 0,
        First = 1,
        Second = 2,
        Third = 3,
        Forth = 4,
        Fifth = 5,
        Sixth = 6,
    }

    /// <summary>
    /// 文件类型
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// 图片
        /// </summary>
        Picture = 0,
        /// <summary>
        /// 视频
        /// </summary>
        Video = 1,      
        /// <summary>
        /// 音频
        /// </summary>
        Music = 2,      
        /// <summary>
        /// 应用
        /// </summary>
        Application = 3,
        /// <summary>
        /// 文档
        /// </summary>
        Document = 4,   
        /// <summary>
        /// 其它
        /// </summary>
        Other = 5,
    }

    /// <summary>
    /// 博客类型
    /// </summary>
    public enum BlogType
    {
        /// <summary>
        /// 随笔
        /// </summary>
        Jotting = 0,
        /// <summary>
        /// 文章
        /// </summary>
        Article = 1,
        /// <summary>
        /// 日记
        /// </summary>
        Diary = 2,
    }

    /// <summary>
    /// 博客编辑器
    /// </summary>
    public enum BlogEditor
    {
        /// <summary>
        /// 百度富文本
        /// </summary>
        Baidu = 0,
        /// <summary>
        /// Markdown
        /// </summary>
        Markdown = 1,
    }

    /// <summary>
    /// 资源可访问性
    /// </summary>
    public enum Accessible
    {
        /// <summary>
        /// 不可达
        /// </summary>
        None = 0,
        /// <summary>
        /// 内部
        /// </summary>
        Inner = 1,
        /// <summary>
        /// 外部
        /// </summary>
        Outer = 2,
        /// <summary>
        /// 全部
        /// </summary>
        All = 3,    
    }

}