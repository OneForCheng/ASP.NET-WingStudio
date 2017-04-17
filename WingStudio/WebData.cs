using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Web.Mvc;
using WingStudio.Models;

namespace WingStudio
{
    /// <summary>
    /// 登录用户
    /// </summary>
    public class LoginUser
    {
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
        /// 安全提问标识
        /// </summary>
        [Required]
        public SecurityFlag SecQuestion { get; set; }

        /// <summary>
        /// 安全提问答案
        /// </summary>
        public String SecAnswer { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public Boolean IsAdmin { get; set; }
    }

    /// <summary>
    /// 邮件信息
    /// </summary>
    public class EmailInfo
    {
        /// <summary>
        /// 目标邮箱
        /// </summary>
        [Required]
        public String ToEmail { get; set; }

        /// <summary>
        /// 邮件主题
        /// </summary>
        [Required]
        public String Theme { get; set; }

        /// <summary>
        /// 邮件内容
        /// </summary>
        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public String Content { get; set; }
    }

    /// <summary>
    /// 待剪切的图片
    /// </summary>
    public class CropPicture
    {  
        /// <summary>
       /// your image path (the one we recieved after successfull upload)
       /// </summary>
        public string imgUrl { get; set; }

        /// <summary>
        /// your image original width (the one we recieved after upload)
        /// </summary>
        public double imgInitW { get; set; }

        /// <summary>
        /// your image original height (the one we recieved after upload)
        /// </summary>
        public double imgInitH { get; set; }

        /// <summary>
        ///  your new scaled image width
        /// </summary>
        public double imgW { get; set; }
        /// <summary>
        /// your new scaled image height
        /// </summary>
        public double imgH { get; set; }
        /// <summary>
        /// top left corner of the cropped image in relation to scaled image
        /// </summary>
        public double imgX1 { get; set; }
        /// <summary>
        /// top left corner of the cropped image in relation to scaled image
        /// </summary>
        public double imgY1 { get; set; }

        /// <summary>
        /// cropped image width
        /// </summary>
        public double cropW { get; set; }
        /// <summary>
        /// cropped image height
        /// </summary>
        public double cropH { get; set; }

        //public double rotation { get; set; }
    }

    /// <summary>
    /// 邮箱配置
    /// </summary>
    public class EmailConfigurationProvider : ConfigurationSection
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("account", IsRequired = true)]
        public string Account
        {
            get { return this["account"].ToString(); }
            set { this["account"] = value; }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return this["password"].ToString(); }
            set { this["password"] = value; }
        }

        [ConfigurationProperty("server", IsRequired = true)]
        public string Server
        {
            get { return this["server"].ToString(); }
            set { this["server"] = value; }
        }

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return Int32.Parse(this["port"].ToString()); }
            set { this["port"] = value; }
        }

        [ConfigurationProperty("isSSL", IsRequired = true)]
        public bool IsSSL
        {
            get { return Boolean.Parse(this["isSSL"].ToString()); }
            set { this["isSSL"] = value; }
        }
    }

    /// <summary>
    /// 省市信息
    /// </summary>
    public class Province
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<City> City { get; set; }
    }

    /// <summary>
    /// 城市信息
    /// </summary>
    public class City
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// 地区信息
    /// </summary>
    public class Area
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Pid { get; set; }
    }

    public class DayBlog
    {
        public int Count { get; set; }
    }
}