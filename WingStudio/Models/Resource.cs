using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WingStudio.Models
{

    /// <summary>
    /// 资源
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [Required]
        public Int32 Id { get; set; }

        /// <summary>
        /// 所有者
        /// </summary>
        public virtual User Owner { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public String Name { get; set; }

    }

    /// <summary>
    /// 文档
    /// </summary>
    public class WebFile : Resource, ICloneable
    {
        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        [Required]
        public DateTime LastModTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 文件路径
        /// </summary>
        [Required]
        public String FilePath { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        [Required]
        public Int32 FileSize { get; set; }

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
        /// 类型
        /// </summary>
        [Required]
        public FileType Type { get; set; }

        /// <summary>
        /// 父目录
        /// </summary>
        public virtual WebFolder ParentFolder { get; set; }

        /// <summary>
        /// 下载次数
        /// </summary>
        [Required]
        public Int32 LoadCount { get; set; } = 0;

        /// <summary>
        /// 浏览次数
        /// </summary>
        [Required]
        public Int32 LookCount { get; set; } = 0;

        /// <summary>
        /// 描述
        /// </summary>
        public String Description { get; set; } = "";

        /// <summary>
        /// 组
        /// </summary>
        public virtual ICollection<FileGroup> Groups { get; set; }

        /// <summary>
        /// 拷贝
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var file = new WebFile();
            file.Owner = this.Owner;
            file.Name = this.Name;
            file.FilePath = this.FilePath;
            file.FileSize = this.FileSize;
            file.Type = this.Type;
            file.Description = this.Description;
            return file;
        }
    }

    /// <summary>
    /// 文件夹
    /// </summary>
    public class WebFolder : Resource, ICloneable
    {
        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        [Required]
        public DateTime LastModTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 类型
        /// </summary>
        [Required]
        public FileType Type { get; set; }

        /// <summary>
        /// 父目录
        /// </summary>
        public virtual WebFolder ParentFolder { get; set; }

        /// <summary>
        /// 子文件夹
        /// </summary>
        public virtual ICollection<WebFolder> SubFolders { get; set; } 

        /// <summary>
        /// 子文件
        /// </summary>
        public virtual ICollection<WebFile> SubFiles { get; set; } 

        /// <summary>
        /// 拷贝
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var folder = new WebFolder();
            CopyFolder(this, folder);
            return folder;
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="targetFolder"></param>
        private void CopyFolder(WebFolder sourceFolder, WebFolder targetFolder)
        {
            targetFolder.Owner = sourceFolder.Owner;
            targetFolder.Name = sourceFolder.Name;
            targetFolder.Type = sourceFolder.Type;
            targetFolder.SubFolders = new HashSet<WebFolder>();
            targetFolder.SubFiles = new HashSet<WebFile>();
            foreach (var item in sourceFolder.SubFiles)
            {
                var file = (WebFile)item.Clone();
                targetFolder.SubFiles.Add(file);
            }
            foreach(var item in sourceFolder.SubFolders)
            {
                var folder = new WebFolder();
                CopyFolder(item, folder);
                targetFolder.SubFolders.Add(folder);
            }
        }

    }

}