using Microsoft.Security.Application;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WingStudio.Models;

namespace WingStudio
{
    /// <summary>
    /// Xss过滤
    /// </summary>
    public class AntiXssAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //对于XSS攻击，只需要对string类型进行验证就可以了
            var str = value as string;
            if (!string.IsNullOrWhiteSpace(str) &&
                validationContext.ObjectInstance != null && !
                string.IsNullOrWhiteSpace(validationContext.MemberName))
            {
                str = Sanitizer.GetSafeHtmlFragment(str);
                PropertyInfo pi = validationContext.ObjectType.GetProperty(validationContext.MemberName,
                    BindingFlags.Public | BindingFlags.Instance);
                pi.SetValue(validationContext.ObjectInstance, str);
            }
            //由于这个类的目的并不是为了验证，所以返回验证成功
            return ValidationResult.Success;
        }
    }

    /// <summary>
    /// 安全检查类
    /// </summary>
    public static class WebSecurity
    {
        /// <summary>
        /// 检查登录账号是否合法
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static bool IsValidAccount(string account)
        {
            if (!string.IsNullOrWhiteSpace(account) && Regex.IsMatch(account, @"^[a-zA-Z0-9]{4,20}$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查密码是否合法
        /// </summary>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static bool IsValidPassword(string pwd)
        {
            if (!string.IsNullOrWhiteSpace(pwd) && Regex.IsMatch(pwd, @"^[a-zA-Z0-9]{6,20}$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查邮箱是否合法
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string email)
        {
            if(!string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4})$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查姓名是否合法
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsValidName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name) && name.Trim().Length < 21)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查要保存的用户是否合法
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsValidUser(User user)
        {
            var maxGrade = DateTime.Now.Year%100;
            if(user != null && user.Name.Trim().Length > 0 && user.Name.Trim().Length < 21 && Regex.IsMatch(user.Account, @"^[a-zA-Z0-9]{4,20}$") && Regex.IsMatch(user.Password, @"^[a-zA-Z0-9]{6,20}$") && Convert.ToInt32(user.Grade) <= maxGrade &&  Regex.IsMatch(user.Grade, "^[1-9][0-9]$") && Regex.IsMatch(user.Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4})$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查安全问题设置是否合法
        /// </summary>
        /// <param name="qusetion"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public static bool IsValidSecQusetion(int qusetion, string answer)
        {
            if (qusetion >= 0 &&  qusetion < 7 && answer != null && answer.Trim().Length < 51)
            {
                if(qusetion != 0 && answer.Trim().Length == 0)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查用户组是否合法
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static bool IsValidUserGroup(UserGroup group)
        {
            if(group != null && group.Theme.Trim().Length > 0 && group.Theme.Trim().Length < 21 && group.Authority >= 0 && group.Authority < 64)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 检查用户是否有这项权限
        /// </summary>
        /// <param name="user"></param>
        /// <param name="authority"></param>
        /// <returns></returns>
        public static bool HasAuthority(this User user, AuthorityFlag authority)
        {
            int auth = (int)authority;
            if (user.Groups.Count(m => (m.Authority & auth) != 0) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        /// <summary>
        /// 检查要保存的公告信息是否合法
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public static bool IsValidNotice(Notice notice)
        {
            if(notice != null && notice.Theme.Trim().Length > 0 && notice.Theme.Trim().Length < 41 && notice.Content.Trim().Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查要保存的动态信息是否合法
        /// </summary>
        /// <param name="dynamic"></param>
        /// <returns></returns>
        public static bool IsValidDynamic(Dynamic dynamic)
        {
            if(dynamic != null && dynamic.Theme.Trim().Length > 0 && dynamic.Theme.Trim().Length < 41 && dynamic.Content.Trim().Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查要保存的博客组信息是否合法
        /// </summary>
        /// <param name="blogGroup"></param>
        /// <returns></returns>
        public static bool IsValidBlogGroup(BlogGroup blogGroup)
        {
            if(blogGroup != null && !string.IsNullOrWhiteSpace(blogGroup.Theme) && !string.IsNullOrWhiteSpace(blogGroup.Description) && blogGroup.Theme.Trim().Length < 21 && blogGroup.Description.Trim().Length < 41)
            {
                var i = (int)blogGroup.Accessible;
                if (i >= 0 && i < 4)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查要保存的博客组信息是否合法
        /// </summary>
        /// <param name="resGroup"></param>
        /// <returns></returns>
        public static bool IsValidFileGroup(FileGroup fileGroup)
        {
            if(fileGroup != null && !string.IsNullOrWhiteSpace(fileGroup.Theme) && !string.IsNullOrWhiteSpace(fileGroup.Description) && fileGroup.Theme.Trim().Length < 21 && fileGroup.Description.Trim().Length < 41)
            {
                var i = (int)fileGroup.Accessible;
                if(i >= 0 && i < 4)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查要保存的报名信息是否合法
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static bool IsValidApplication(Application app)
        {
            if (app != null && app.Theme.Trim().Length > 0 && app.Theme.Trim().Length < 21 && app.Content.Trim().Length > 0 && DateTime.Compare(app.StartTime, app.EndTime) <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查要保存的信息是否合法
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool IsValidMessage(Message msg)
        {
            if (msg != null && !string.IsNullOrWhiteSpace(msg.Theme) && msg.Content != null && msg.Theme.Trim().Length < 41 && msg.Content.Trim().Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查用户住址是否合法
        /// </summary>
        /// <param name="path">网站根目录</param>
        /// <param name="address">用户住址</param>
        /// <returns></returns>
        static public bool IsValidUserAddress(string path, string address)
        {
            if (Regex.IsMatch(address, @"^[\u4e00-\u9fa5]{2,4}-[\u4e00-\u9fa5]{2,8}-[\u4e00-\u9fa5]{2,10}$"))
            {
                var strs = address.Split('-');
                StreamReader sr = new StreamReader(Path.Combine(path, @"WingStudio\DataFile\Areas.txt"));
                string jsonStr1 = sr.ReadLine();
                string jsonStr2 = sr.ReadLine();
                sr.Close();
                var serializer = new JavaScriptSerializer();
                var provinces = serializer.Deserialize<List<Province>>(jsonStr1);
                var areas = serializer.Deserialize<List<Area>>(jsonStr2);
                var province = provinces.SingleOrDefault(m => m.Name == strs[0]);
                if (provinces != null)
                {
                    var city = province.City.SingleOrDefault(m => m.Name == strs[1]);
                    if(city == null)
                    {
                        return false;
                    }
                    var area = areas.SingleOrDefault(m => m.Name == strs[2] && m.Pid == city.Id);
                    if (area != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断上传文件是否超过限制
        /// </summary>
        /// <param name="fileSzie"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public static bool UploadOverLimit(int fileSzie, List<WebFile> files)
        {
            int sum = 0;
            if (files.Count() > 0)
            {
                sum = files.Sum(m => m.FileSize);
            }
            if (sum + fileSzie > 1073741824)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查上传文件的后缀名是否合法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        public static bool IsValidFileExt(FileType type, string fileExt)
        {
            if (type == FileType.Other || type == FileType.Application)
            {
                return true;
            }
            else if(type == FileType.Document)
            {
                switch(fileExt)
                {
                    case ".doc":
                    case ".docx":
                    case ".xls":
                    case ".xlsx":
                    case ".ppt":
                    case ".pptx":
                    case ".pdf":
                    case ".txt":
                        return true;
                    default:
                        return false;
                }
            }
            else if (type == FileType.Music && fileExt == ".mp3")
            {
                return true;
            }
            else if (type == FileType.Picture && (fileExt == ".png" || fileExt == ".jpg" || fileExt == ".gif"))
            {
                return true;
            }
            else if (type == FileType.Video && (fileExt == ".mp4" || fileExt == ".flv"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查要保存的博客信息是否合法
        /// </summary>
        /// <param name="blog"></param>
        /// <returns></returns>
        public static bool IsValidBlog(Blog blog)
        {
            var type = (int)blog.Type;
            if (blog != null && blog.Theme.Trim().Length > 0 && blog.Theme.Trim().Length < 41 && blog.Content.Trim().Length > 0 && type >= 0 && type < 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查登录用户信息是否合法
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool IsValidLoginUser(LoginUser user)
        {
            if (user != null && user.Account != null && user.Password != null  && Regex.IsMatch(user.Account, @"^[a-zA-Z0-9]{4,20}$") && Regex.IsMatch(user.Password, @"^[a-zA-Z0-9]{6,20}$") && user.SecQuestion >= SecurityFlag.None && user.SecQuestion <= SecurityFlag.Sixth)
            {
                if(user.SecQuestion == SecurityFlag.None)
                {
                    return true;
                }
                else
                {
                    if(user.SecAnswer != null && user.SecAnswer.Trim().Length > 0 && user.SecAnswer.Trim().Length < 51)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查重置密码信息是否合法
        /// </summary>
        /// <param name="account"></param>
        /// <param name="code"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool IsValidResetPassword(string account, string code, string password)
        {
            if (account != null && code != null && password != null && Regex.IsMatch(account, @"^[a-zA-Z0-9]{4,20}$") && Regex.IsMatch(code, @"^[0-9a-zA-Z]{32}$") && Regex.IsMatch(password, @"^[a-zA-Z0-9]{6,20}$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查要保存的报名者信息是否合法
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        public static bool IsValidParticipant(Participant part, bool isFormal)
        {
            if (part != null && !string.IsNullOrWhiteSpace(part.Name) && !string.IsNullOrWhiteSpace(part.StudentNo) && !string.IsNullOrWhiteSpace(part.StudentClass) && !string.IsNullOrWhiteSpace(part.Email) && part.Name.Trim().Length < 21 && part.StudentClass.Trim().Length < 31 && Regex.IsMatch(part.StudentNo, "^20[0-9]{6,7}$") && Regex.IsMatch(part.Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4})$"))
            {
                if(isFormal)
                {
                    if(string.IsNullOrWhiteSpace(part.Phone) || string.IsNullOrWhiteSpace(part.Sex) || !Regex.IsMatch(part.Phone, @"^1[3|4|5|7|8]\d{9}$"))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查邮件信息
        /// </summary>
        /// <param name="emailInfo"></param>
        /// <returns></returns>
        public static bool IsValidEmailInfo(EmailInfo emailInfo)
        {
            if (emailInfo != null && !string.IsNullOrWhiteSpace(emailInfo.Theme) && !string.IsNullOrWhiteSpace(emailInfo.Content) && emailInfo.Theme.Trim().Length < 51 && emailInfo.ToEmail != null && Regex.IsMatch(emailInfo.ToEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4})$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}