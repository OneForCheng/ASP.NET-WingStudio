using ForCheng.Security.AES;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WingStudio.Models;
using System.Threading.Tasks;

namespace WingStudio.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : BaseController
    {
     
        #region 其它
        /// <summary>
        /// 个人主页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(int? page)
        {
            ViewBag.Logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            ViewBag.UserCount = entity.Users.Count();
            var timeLimit = DateTime.Now.AddMonths(-1);

            var blogs = entity.Blogs.Where(m => m.IsPublic);
            //var files = entity.WebFiles.Where(m => m.Checked && m.Groups.Count(n => (n.Accessible & Accessible.Inner) != 0) > 0);
            //ViewBag.NewBlogs = blogs.OrderByDescending(m => m.PublicTime).Take(5);
            ViewBag.HotBlogs = blogs.Where(m => m.CreateTime > timeLimit).OrderByDescending(m => m.LookCount).Take(5);
            //ViewBag.NewFiles = files.OrderByDescending(m => m.Id).Take(5);
            //ViewBag.HotFiles = files.OrderByDescending(m => m.LoadCount).ThenBy(m => m.LookCount).Take(5);

            var pageNumber = page ?? 1;
            ViewBag.PageNumber = pageNumber;
            var onePageOfProducts = entity.Users.OrderByDescending(m => m.Id).ToPagedList(pageNumber, 6);
            return View(onePageOfProducts);
        }

        #endregion

        #region 个人信息

        /// <summary>
        /// 获取当前登录用户的个人信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetLoginUserInfo()
        {
            var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            var userInfo = logined.UserInfo;
            var birthday = "";
            if (userInfo.Birthday != null)
            {
                birthday = userInfo.Birthday.Value.ToLongDateString();
            }
            var infos = new
            {
                logined.Name,
                userInfo.Class,
                Birthday = birthday,
                userInfo.CurrentAddr,
                userInfo.NativeAddr,
                userInfo.Hobby,
                userInfo.Sign,
                userInfo.Introduction,
                userInfo.QQ,
                userInfo.WeiChat,
                userInfo.Blog,
                userInfo.Phone
            };
            return Json((new JavaScriptSerializer()).Serialize(infos));
        }

        #region 账户设置
        /// <summary>
        /// 账户设置
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountSetting()
        {
            var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            return View(logined);
        }

        /// <summary>
        /// 修改用户名
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeAccount(string account)
        {
            if (WebSecurity.IsValidAccount(account))
            {
                if (entity.Users.Count(m => m.Account == account) > 0 || entity.SuperUsers.Count(m => m.Account == account) > 0)
                {
                    return Json(new { title = "修改失败", message = "该用户账号已存在!" });
                }
                else
                {
                    var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
                    logined.Account = account;
                    entity.SaveChanges();
                    return Json(new { title = "修改成功", message = "成功修改用户账号!" });
                }
            }
            else
            {
                return Json(new { title = "修改失败", message = "用户账号不合法!" });
            }

        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangePassword(string oldPassword, string newPassword)
        {
            if (WebSecurity.IsValidPassword(oldPassword) && WebSecurity.IsValidPassword(newPassword))
            {
                var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
                if (AES.Encrypt(oldPassword) == logined.Password)
                {
                    logined.Password = AES.Encrypt(newPassword);
                    entity.SaveChanges();
                    return Json(new { title = "修改成功", message = "成功修改密码!" });
                }
                else
                {
                    return Json(new { title = "修改失败", message = "旧密码不正确!" });
                }
            }
            else
            {
                return Json(new { title = "修改失败", message = "提交信息不合法!" });
            }
        }

        /// <summary>
        /// 修改安全提问
        /// </summary>
        /// <param name="qusetion"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeSecQusetion(int qusetion, string answer)
        {
            if (WebSecurity.IsValidSecQusetion(qusetion, answer))
            {
                var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
                var flag = (SecurityFlag)qusetion;
                logined.SecQuestion = flag;
                if (flag != SecurityFlag.None)
                {
                    logined.SecAnswer = AES.Encrypt(answer.Trim());
                }
                entity.SaveChanges();
                return Json(new { title = "修改成功", message = "成功修改安全提问!" });
            }
            else
            {
                return Json(new { title = "修改失败", message = "提交信息不合法!" });
            }
        }

        /// <summary>
        /// 修改邮箱
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ActionResult ChangeEmail(string email)
        {
            if (WebSecurity.IsValidEmail(email))
            {
                var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
                logined.Email = email;
                entity.SaveChanges();
                return Json(new { title = "修改成功", message = "成功修改邮箱!" });
            }
            else
            {
                return Json(new { title = "修改失败", message = "邮箱不合法!" });
            }

        }

        /// <summary>
        /// 修改姓名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ActionResult ChangeName(string name)
        {
            if (WebSecurity.IsValidName(name))
            {
                var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
                logined.Name = name.Trim();
                entity.SaveChanges();
                return Json(new { title = "修改成功", message = "成功修改姓名!" });
            }
            else
            {
                return Json(new { title = "修改失败", message = "姓名不合法!" });
            }

        }

        #endregion

        #region 更新头像

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadAvatar()
        {
            if (Request.Files.Count == 0)
            {
                return Json(new {status = "error", message = "上传图片不能为空!" });
            }
            HttpPostedFileBase picture = Request.Files[0];
            if (picture.ContentLength > 5242880)
            {
                return Json(new { status = "error", message = "上传图片大小超出指定范围(5M)!" });
            }
            string fileName = picture.FileName.Split('\\').Last();
            if(!fileName.Contains("."))
            {
                return Json(new { status = "error", message = "上传图片格式不正确(.jpg/.png/.gif)!" });
            }
            string fileExt = fileName.Substring(fileName.LastIndexOf('.')).ToLower();
            if (fileExt == ".jpg" || fileExt == ".png" || fileExt == ".gif")
            {
                string path = Server.MapPath("~/WingStudio/Avatar");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                fileName = DateTime.Now.ToFileTime().ToString() + Guid.NewGuid().ToString("N") + fileExt;
                var picPath = Path.Combine(path, fileName);
                picture.SaveAs(picPath);//从客户端保存文件到本地

                var image = new WebImage(picPath);
                double height = image.Height;
                double width = image.Width;
                return Json(new {
                    status = "success",
                    url = $"/WingStudio/Avatar/{fileName}",
                    width = width,
                    height = height
                });
            }
            else
            {
                return Json(new { status = "error", message = "上传图片格式不正确(.jpg/.png/.gif)!" });
            }
        }

        /// <summary>
        /// 剪切头像
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CropAvatar(CropPicture picture)
        {
            try
            {
                string picPath = Server.MapPath(picture.imgUrl);
                if (System.IO.File.Exists(picPath))
                {
                    var image = new WebImage(picPath);
                    image.Resize((int)picture.imgW, (int)picture.imgH);

                    image.Crop((int)picture.imgY1, (int)picture.imgX1, (int)(picture.imgH - picture.cropH - picture.imgY1), (int)(picture.imgW - picture.cropW - picture.imgX1));
                    image.Save(picPath);

                    var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
                    logined.Avatar = picPath.Split('\\').Last();
                    entity.SaveChanges();
                    return Json(new { status = "success", url = $"/WingStudio/Avatar/{logined.Avatar}" });
                }
                else
                {
                    return Json(new { status = "error", message = "剪切失败，找不到目标图片!" });
                }
            }
            catch (Exception e)
            {
                return Json(new { status = "error", message = "操作异常，图片剪切失败!" });
            }
           
        }
        #endregion

        #region 个人信息
        /// <summary>
        /// 更新个人信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InfoSetting()
        {
            var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            return View(logined.UserInfo);
        }

        /// <summary>
        /// 更新个人信息
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateInfo(UserInfo userInfo)
        {
            var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            //更新是否公开信息
            logined.UserInfo.PublicInfo = userInfo.PublicInfo;

            //更新班级
            if (!string.IsNullOrWhiteSpace(userInfo.Class))
            {
                if (userInfo.Class.Trim().Length <= 20)
                {
                    logined.UserInfo.Class = userInfo.Class.Trim();
                }
                else
                {
                    return Content(WebHelper.SweetAlert("更新失败", "班级不合法!", "window.history.go(-1);"));
                }
            }
            else
            {
                logined.UserInfo.Class = "";
            }

            //更新生日
            if (userInfo.Birthday != null)
            {
                try
                {

                    if (DateTime.Compare(DateTime.Now, (DateTime)userInfo.Birthday) >= 0)
                    {
                        logined.UserInfo.Birthday = userInfo.Birthday;
                    }
                    else
                    {
                        return Content(WebHelper.SweetAlert("更新失败", "出生日期不合法!", "window.history.go(-1);"));
                    }
                }
                catch
                {
                    return Content(WebHelper.SweetAlert("更新失败", "出生日期不合法!", "window.history.go(-1);"));
                }
            }
            else
            {
                logined.UserInfo.Birthday = null;
            }

            //更新当前住址
            if (!string.IsNullOrWhiteSpace(userInfo.CurrentAddr))
            {
                if (userInfo.CurrentAddr != logined.UserInfo.CurrentAddr)
                {
                    if (WebSecurity.IsValidUserAddress(Server.MapPath("~/"), userInfo.CurrentAddr))
                    {
                        logined.UserInfo.CurrentAddr = userInfo.CurrentAddr;
                    }
                    else
                    {
                        return Content(WebHelper.SweetAlert("更新失败", "当前住址不合法!", "window.history.go(-1);"));
                    }
                }
            }
            else
            {
                logined.UserInfo.CurrentAddr = "";
            }

            //更新家庭住址
            if (!string.IsNullOrWhiteSpace(userInfo.NativeAddr))
            {
                if (userInfo.NativeAddr != logined.UserInfo.NativeAddr)
                {
                    if (WebSecurity.IsValidUserAddress(Server.MapPath("~/"), userInfo.NativeAddr))
                    {
                        logined.UserInfo.NativeAddr = userInfo.NativeAddr;
                    }
                    else
                    {
                        return Content(WebHelper.SweetAlert("更新失败", "家庭住址不合法!", "window.history.go(-1);"));
                    }
                }
            }
            else
            {
                logined.UserInfo.NativeAddr = "";
            }

            //更新爱好
            if (!string.IsNullOrWhiteSpace(userInfo.Hobby))
            {
                if (userInfo.Hobby.Trim().Length <= 50)
                {
                    logined.UserInfo.Hobby = userInfo.Hobby.Trim();
                }
                else
                {
                    return Content(WebHelper.SweetAlert("更新失败", "爱好不合法!", "window.history.go(-1);"));
                }
            }
            else
            {
                logined.UserInfo.Hobby = "";
            }

            //更新个人签名
            if (!string.IsNullOrWhiteSpace(userInfo.Sign))
            {
                if (userInfo.Sign.Trim().Length <= 50)
                {
                    logined.UserInfo.Sign = userInfo.Sign.Trim();
                }
                else
                {
                    return Content(WebHelper.SweetAlert("更新失败", "个人标签不合法!", "window.history.go(-1);"));
                }
            }
            else
            {
                logined.UserInfo.Sign = "";
            }

            //更新个人简介
            if (!string.IsNullOrWhiteSpace(userInfo.Introduction))
            {
                if (userInfo.Introduction.Trim().Length <= 100)
                {
                    logined.UserInfo.Introduction = userInfo.Introduction.Trim();
                }
                else
                {
                    return Content(WebHelper.SweetAlert("更新失败", "个人简介不合法!", "window.history.go(-1);"));
                }
            }
            else
            {
                logined.UserInfo.Introduction = "";
            }

            entity.SaveChanges();
            return Content(WebHelper.SweetAlert("更新成功", "成功更新个人信息!", "location.href='/User/Index'"));
        }

        /// <summary>
        /// 获取指定用户的个人信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserInfo(int id)
        {
            var user = entity.Users.Find(id);
            if(user != null && user.UserInfo.PublicInfo)
            {
                
                var userInfo = user.UserInfo;
                var birthday = "";
                if(userInfo.Birthday != null)
                {
                    birthday = userInfo.Birthday.Value.ToLongDateString();
                }
                var infos = new
                {
                    user.Name,
                    userInfo.Class,
                    Birthday = birthday,
                    userInfo.CurrentAddr,
                    userInfo.NativeAddr,
                    userInfo.Hobby,
                    userInfo.Sign,
                    userInfo.Introduction
                };
                return Json((new JavaScriptSerializer()).Serialize(infos));
            }
            else
            {
                return Json(new {  });
            }
        }
        #endregion

        #region 联系方式
        /// <summary>
        /// 更新联系方式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ContactSetting()
        {
            var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            return View(logined.UserInfo);
        }

        /// <summary>
        /// 更新联系方式
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateContact(UserInfo contact)
        {
            var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));

            logined.UserInfo.PublicContact = contact.PublicContact;

            //更新QQ账号
            if (!string.IsNullOrWhiteSpace(contact.QQ))
            {
                if (Regex.IsMatch(contact.QQ, @"^[0-9]{5,11}$"))
                {
                    logined.UserInfo.QQ = contact.QQ;
                }
                else
                {
                    return Content(WebHelper.SweetAlert("更新失败", "QQ不合法!", "window.history.go(-1);"));
                }
            }
            else
            {
                logined.UserInfo.QQ = "";
            }

            //更新微信账号
            if (!string.IsNullOrWhiteSpace(contact.WeiChat))
            {
                if (Regex.IsMatch(contact.WeiChat, @"^[a-zA-Z]([0-9a-zA-Z]|_|-){5,19}$"))
                {
                    logined.UserInfo.WeiChat = contact.WeiChat;
                }
                else
                {
                    return Content(WebHelper.SweetAlert("更新失败", "微信不合法!", "window.history.go(-1);"));
                }
            }
            else
            {
                logined.UserInfo.WeiChat = "";
            }

            //更新博客连接
            if (!string.IsNullOrWhiteSpace(contact.Blog))
            {
                if (Regex.IsMatch(contact.Blog, @"^((http|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?$"))
                {
                    logined.UserInfo.Blog = contact.Blog;
                }
                else
                {
                    return Content(WebHelper.SweetAlert("更新失败", "博客不合法!", "window.history.go(-1);"));
                }
            }
            else
            {
                logined.UserInfo.Blog = "";
            }

            //更新电话
            if (!string.IsNullOrWhiteSpace(contact.Phone))
            {
                if (Regex.IsMatch(contact.Phone, @"^1[3|4|5|7|8]\d{9}$"))
                {
                    logined.UserInfo.Phone = contact.Phone;
                }
                else
                {
                    return Content(WebHelper.SweetAlert("更新失败", "电话不合法!", "window.history.go(-1);"));
                }
            }
            else
            {
                logined.UserInfo.Phone = "";
            }

            entity.SaveChanges();
            return Content(WebHelper.SweetAlert("更新成功", "成功更新联系方式!", "location.href='/User/Index'"));
        }

        /// <summary>
        /// 获取指定用户的联系信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserContact(int id)
        {
            var user = entity.Users.Find(id);
            if (user != null && user.UserInfo.PublicContact)
            {
                var userInfo = user.UserInfo;
                var infos = new
                {
                    user.Name,
                    userInfo.QQ,
                    userInfo.WeiChat,
                    userInfo.Blog,
                    userInfo.Phone
                };
                return Json((new JavaScriptSerializer()).Serialize(infos));
            }
            else
            {
                return Json(new { });
            }
        }
        #endregion

        #endregion

        #region 个人设置

        /// <summary>
        /// 用户设置
        /// </summary>
        /// <returns></returns>
        public ActionResult UserConfig()
        {
            return View();
        }

        #region 基本设置
        /// <summary>
        /// 更新基本设置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateBaseConfig(UserConfig config)
        {
            return View();
        }

        #endregion

        #region 其它设置
        /// <summary>
        /// 更新其他设置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateOtherConfig(UserConfig config)
        {
            return View();
        }

        #endregion

        #endregion

        #region 博客
        /// <summary>
        /// 显示指定博客
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowBlog(int id)
        {
            var blog = entity.Blogs.Find(id);
            var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            if (blog == null)
            {
                return View("Error");
            }
            else if (blog.Owner.Id == logined.Id)
            {
                //blog.LookCount++;
                //entity.SaveChanges();
                var blogs = logined.Blogs.Where(m => m.Type == blog.Type);
                ViewBag.LastBlog = blogs.Where(m => m.Id > id).OrderBy(m => m.Id).FirstOrDefault();
                ViewBag.NextBlog = blogs.Where(m => m.Id < id).OrderByDescending(m => m.Id).FirstOrDefault();
                ViewBag.Logined = logined;
                return View("ShowBlog", blog);
            }
            else if (blog.IsPublic)
            {
                blog.LookCount++;
                entity.SaveChanges();
                var blogs = entity.Blogs.Where(m => m.Owner.Id == blog.Owner.Id && m.IsPublic);
                ViewBag.LastBlog = blogs.Where(m => m.Id > id).OrderBy(m => m.Id).FirstOrDefault();
                ViewBag.NextBlog = blogs.Where(m => m.Id < id).OrderByDescending(m => m.Id).FirstOrDefault();
                ViewBag.Logined = logined;
                return View("ShowBlog", blog);
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 显示指定用户的已发布的博客
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult PublicUserBlogs(int? id, int? page)
        {
            var loginId = Convert.ToInt32(User.Identity.Name);
            var targetId = id ?? loginId;
            var user = entity.Users.Find(targetId);
            if (user != null)
            {
                if (targetId == loginId)
                {
                    ViewBag.Logined = user;
                }
                else
                {
                    ViewBag.Logined = entity.Users.Find(loginId);
                }
                ViewBag.UserId = targetId;
                ViewBag.Title = user.Account + " - 个人博客";
                var blogs = user.Blogs.Where(m => m.IsPublic).OrderByDescending(m => m.Id);
                var pageNumber = page ?? 1;
                var onePageOfProducts = blogs.ToPagedList(pageNumber, 10);
                return View("Blogs", onePageOfProducts);
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 搜索公告博客
        /// </summary>
        /// <param name="searchContent"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult SearchPublicBlog(string searchContent, int? page)
        {
            ViewBag.Logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            ViewBag.Title = "搜索博客 - 所有博客";
            if (string.IsNullOrWhiteSpace(searchContent))
            {
                return View("Blogs", new List<Blog>().ToPagedList(1, 10));
            }
            else
            {
               
                searchContent = searchContent.Trim();
                var blogs = entity.Blogs.Where(m => m.IsPublic).Where(m => m.Theme.Contains(searchContent)).OrderByDescending(m => m.Id);
                var pageNumber = page ?? 1;

                var onePageOfProducts = blogs.ToPagedList(pageNumber, 10);
                ViewBag.SearchContent = searchContent;
                return View("Blogs", onePageOfProducts);
            }
        }

        #region 博客专栏
        /// <summary>
        /// 博客专栏
        /// </summary>
        /// <returns></returns>
        public ActionResult BlogColumn(int? page)
        {
            var groups = entity.BlogGroups.Where(m => (m.Accessible & Accessible.Inner) != 0).OrderByDescending(m => m.Id);
            var pageNumber = page ?? 1;
            var onePageOfProducts = groups.ToPagedList(pageNumber, 12);
            return View(onePageOfProducts);
        }

        /// <summary>
        /// 专栏博客
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ColumnBlogs(int id, int? page)
        {
            var blogGroup = entity.BlogGroups.Find(id);
            if (blogGroup != null && (blogGroup.Accessible & Accessible.Inner) != 0)
            {
                ViewBag.GroupId = id;
                ViewBag.Title = blogGroup.Theme + " - 博客专栏";
                ViewBag.Logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));

                var pageNumber = page ?? 1;
                var onePageOfProducts = blogGroup.Blogs.OrderByDescending(m => m.Id).ToPagedList(pageNumber, 10);
                return View("Blogs", onePageOfProducts);
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 最新博客专栏
        /// </summary>
        /// <returns></returns>
        public ActionResult ColumnNewBlogs()
        {
            ViewBag.Title = "最新博客 - 博客专栏";
            ViewBag.Logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            var blogs = entity.Blogs.Where(m => m.IsPublic).OrderByDescending(m => m.PublicTime).ToPagedList(1, 10);
            return View("Blogs", blogs);
        }
        
        /// <summary>
        /// 最热博客专栏
        /// </summary>
        /// <returns></returns>
        public ActionResult ColumnHotBlogs()
        {
            var timeLimit = DateTime.Now.AddMonths(-1);
            ViewBag.Title = "最热博客 - 博客专栏";
            ViewBag.Logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            var blogs = entity.Blogs.Where(m => m.IsPublic && (m.PublicTime > timeLimit)).OrderByDescending(m => m.LookCount).ThenBy(m => m.Id).ToPagedList(1, 10);
            return View("Blogs", blogs);
        }

        /// <summary>
        /// 强推博客专栏
        /// </summary>
        /// <returns></returns>
        public ActionResult ColumnPraiseBlogs()
        {
            ViewBag.Title = "强推博客 - 博客专栏";
            ViewBag.Logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            var blogs = entity.Blogs.Where(m => m.IsPublic).OrderByDescending(m => m.Recommendations.Count).ThenBy(m => m.LookCount).ToPagedList(1, 10);
            return View("Blogs", blogs);
        }

        #endregion

        #region 博客管理

        #region 个人博客管理
        /// <summary>
        /// 管理博客(随笔)
        /// </summary>
        /// <param name="page"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult ManageBlog(int? page, string type)
        {
            var blogType = BlogType.Jotting;
            if (type == "article")
            {
                blogType = BlogType.Article;
            }
            else if (type == "diary")
            {
                blogType = BlogType.Diary;
            }
            ViewBag.BlogType = blogType;

            var id = Convert.ToInt32(User.Identity.Name);
            var logined = entity.Users.Find(id);
            ViewBag.PublicBlog = logined.UserConfig.PublicBlog;
            ViewBag.Account = logined.Account;

            var blogs = entity.Blogs.Where(m => m.Owner.Id == id && m.Type == blogType).OrderByDescending(m => m.Id);
            var pageNumber = page ?? 1;
            var onePageOfProducts = blogs.ToPagedList(pageNumber, 10);
            return View("ManageBlog", onePageOfProducts);
        }

        /// <summary>
        /// 博客设置
        /// </summary>
        /// <returns></returns>
        public ActionResult BlogConfig()
        {
            var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            ViewBag.Account = logined.Account;
            return View(logined.UserConfig);
        }

        /// <summary>
        /// 保存博客配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveBlogConfig(UserConfig config)
        {
            var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            var userConfig = logined.UserConfig;
            if(userConfig.PublicBlog != config.PublicBlog)
            {
                userConfig.PublicBlog = config.PublicBlog;
            }
            if(config.BlogEditor == BlogEditor.Baidu)
            {
                userConfig.BlogEditor = BlogEditor.Baidu;
            }
            else
            {
                userConfig.BlogEditor = BlogEditor.Markdown;
            }
            entity.SaveChanges();
            return Content(WebHelper.SweetAlert("保存成功", "成功保存博客设置!"));
        }

        /// <summary>
        /// 重置博客设置
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetBlogConfig()
        {
            var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            logined.UserConfig.PublicBlog = false;
            logined.UserConfig.BlogEditor = BlogEditor.Baidu;
            entity.SaveChanges();
            return Content(WebHelper.SweetAlert("重置成功", "成功重置博客设置为默认!", "location.href='/User/BlogConfig'"));
        }

        /// <summary>
        /// 未发布的博客
        /// </summary>
        /// <param name="page"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult PrivateBlogs(int? page, string type)
        {
            var blogType = BlogType.Jotting;
            if (type == "article")
            {
                blogType = BlogType.Article;
            }
            ViewBag.BlogType = blogType;

            var id = Convert.ToInt32(User.Identity.Name);
            var blogs = entity.Blogs.Where(m => m.Owner.Id == id && !m.IsPublic && m.Type == blogType).OrderByDescending(m => m.Id);
            var pageNumber = page ?? 1;
            var onePageOfProducts = blogs.ToPagedList(pageNumber, 10);
            return View("ManageBlog", onePageOfProducts);
        }

        /// <summary>
        /// 登录用户博客
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LoginUserBlogs(int? page, string type)
        {
            var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            ViewBag.Logined = logined;
            var pageNumber = page ?? 1;
            ViewBag.Type = "jotting";
            ViewBag.Title = "我的随笔";
            var blogType = BlogType.Jotting;
            if (type == "article")
            {
                ViewBag.Type = "article";
                ViewBag.Title = "我的文章";
                blogType = BlogType.Article;
            }
            else if (type == "diary")
            {
                ViewBag.Type = "diary";
                ViewBag.Title = "我的日记";
                blogType = BlogType.Diary;
            }
            else if (type == "collect")
            {
                ViewBag.Type = "collect";
                ViewBag.Title = "我的收藏";
                return View("Blogs", logined.Favorites.Blogs.ToPagedList(pageNumber, 10));
            }
            
            var blogs = logined.Blogs.Where(m => m.Type == blogType).OrderByDescending(m => m.Id);
            var onePageOfProducts = blogs.ToPagedList(pageNumber, 10);
            return View("Blogs", onePageOfProducts);
        }

        /// <summary>
        /// 保存博客成功
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DoneSaveBlog(int id)
        {
            var blog = entity.Blogs.Find(id);
            if (blog != null && blog.Owner.Id == Convert.ToInt32(User.Identity.Name))
            {
                ViewBag.PublicBlog = blog.Owner.UserConfig.PublicBlog;
                ViewBag.Account = blog.Owner.Account;
                return View(blog);
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 上传博客图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadBlogPicture()
        {
            if (Request.Files.Count == 0)
            {
                return Json(new { success = 0, message = "上传图片不能为空!" });
            }
            HttpPostedFileBase picture = Request.Files[0];
            if (picture.ContentLength > 5242880)
            {
                return Json(new { success = 0, message = "上传图片大小超出指定范围(5M)!" });
            }
            string fileName = picture.FileName.Split('\\').Last();
            if (!fileName.Contains("."))
            {
                return Json(new { success = 0, message = "上传图片格式不正确(.jpg/.png/.gif/.jpeg)!" });
            }
            string fileExt = fileName.Substring(fileName.LastIndexOf('.')).ToLower();
            if (fileExt == ".jpg" || fileExt == ".png" || fileExt == ".gif" || fileExt == ".jpeg")
            {
                string path = Server.MapPath("~/WingStudio/MarkdownPicture");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                
                fileName = DateTime.Now.ToFileTime().ToString() + Guid.NewGuid().ToString("N") + fileExt;
                var picPath = Path.Combine(path, fileName);
                picture.SaveAs(picPath);//从客户端保存文件到本地
                var image = new WebImage(picPath);
                var width = image.Width;
                var height = image.Height;
                if (width > height && width > 800)
                {
                    height = 800 * (height / width);
                    if(height > 0)
                    {
                        image.Resize(800, height);//调整图片大小
                        image.Save(picPath);
                    }
                }
                else if (height > 800)
                {
                    width = 800 * (width / height);
                    if(width > 0)
                    {
                        image.Resize(width, 800);//调整图片大小
                        image.Save(picPath);
                    }
                }
                return Json(new { success = 1, message = "成功上传图片!", url = $"/WingStudio/MarkdownPicture/{fileName}" });
            }
            else
            {
                return Json(new { success = 0, message = "上传图片格式不正确(.jpg/.png/.gif/.jpeg)!" });
            }
        }

        /// <summary>
        /// 添加博客
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddBlog(string type)
        {
            ViewBag.BlogEditor = entity.Users.Find(Convert.ToInt32(User.Identity.Name)).UserConfig.BlogEditor;
            ViewBag.IsModified = false;
            var blogType = BlogType.Jotting;
            if (type == "article")
            {
                blogType = BlogType.Article;
            }
            else if (type == "diary")
            {
                blogType = BlogType.Diary;
            }
            ViewBag.BlogType = blogType;
            return View("SaveBlog");
        }

        /// <summary>
        /// 添加博客
        /// </summary>
        /// <param name="blog"></param>
        /// <returns></returns>
        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult AddBlog(Blog blog)
        {
            if (WebSecurity.IsValidBlog(blog))
            {
                var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
                var newBlog = new Blog();
                newBlog.Owner = logined;
                newBlog.Theme = blog.Theme.Trim();
                newBlog.Content = blog.Content;
                newBlog.Type = blog.Type;
                newBlog.BlogEditor = logined.UserConfig.BlogEditor;
                if (blog.Type == BlogType.Diary)
                {
                    newBlog.IsPublic = false;
                }
                else
                {
                    newBlog.IsPublic = blog.IsPublic;
                    if (newBlog.IsPublic)
                    {
                        newBlog.PublicTime = DateTime.Now;
                    }
                }

                if (!string.IsNullOrWhiteSpace(blog.Tag))
                {
                    var str = "";
                    var subStrs = blog.Tag.ToLower().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    var count = subStrs.Count() < 10 ? subStrs.Count() : 10;
                    for (int i = 0; i < count - 1; i++)
                    {
                        if (subStrs.ElementAt(i).Length < 51)
                        {
                            str += subStrs.ElementAt(i) + ",";
                        }
                        else
                        {
                            str += subStrs.ElementAt(i).Substring(0, 50) + ",";
                        }
                    }
                    if (subStrs.ElementAt(count - 1).Length < 51)
                    {
                        str += subStrs.ElementAt(count - 1);
                    }
                    else
                    {
                        str += subStrs.ElementAt(count - 1).Substring(0, 50);
                    }
                    newBlog.Tag = str;
                }
                entity.Blogs.Add(newBlog);
                entity.SaveChanges();
                return RedirectToAction("DoneSaveBlog", new { id = newBlog.Id });
            }
            else
            {
                return Content(WebHelper.SweetAlert("保存失败", "提交的信息不合法!"));
            }
        }

        /// <summary>
        /// 直接添加博客到榜中
        /// </summary>
        /// <param name="blog"></param>
        /// <returns></returns>
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddBlogToTopList(Blog blog)
        {
            if (WebSecurity.IsValidBlog(blog))
            {
                var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
                string tag = "";
                if (!string.IsNullOrWhiteSpace(blog.Tag))
                {
                    var subStrs = blog.Tag.ToLower().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    var count = subStrs.Count() < 10 ? subStrs.Count() : 10;
                    for (int i = 0; i < count - 1; i++)
                    {
                        if (subStrs.ElementAt(i).Length < 51)
                        {
                            tag += subStrs.ElementAt(i) + ",";
                        }
                        else
                        {
                            tag += subStrs.ElementAt(i).Substring(0, 50) + ",";
                        }
                    }
                    if (subStrs.ElementAt(count - 1).Length < 51)
                    {
                        tag += subStrs.ElementAt(count - 1);
                    }
                    else
                    {
                        tag += subStrs.ElementAt(count - 1).Substring(0, 50);
                    }
                }
                Dictionary<string, string> requstData = new Dictionary<string, string> {
                    {"Tag", tag },
                    {"Owner_Id", logined.Id.ToString() },
                    {"Content", blog.Content },
                    {"Theme",  blog.Theme.Trim() },
                    {"Type", ((int)BlogType.Jotting).ToString() },
                    {"BlogEditor", ((int)logined.UserConfig.BlogEditor).ToString() },
                    {"Account", logined.Account },
                    {"Password", logined.Password },
                };
                string result = await WebHelper.HttpRequest("http://119.29.209.102:8080/appserver/addToBlogTop", requstData);
                if (result == "1")
                {
                    return Content(WebHelper.SweetAlert("入榜成功", "博客成功入榜，可在手机App上查看，待到放榜之时就是它回归之日!", "location.href='/User/ManageBlog'"));
                }
                else
                {
                    return Content(WebHelper.SweetAlert("入榜失败", $"可能由于网络原因或访问受限，请求发送失败，请刷新重试!"));
                }
                
            }
            else
            {
                return Content(WebHelper.SweetAlert("入榜失败", "提交的信息不合法!"));
            }
        }

        /// <summary>
        /// 修改博客
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ModBlog(int id)
        {
            var blog = entity.Blogs.Find(id);
            if (blog != null && blog.Owner.Id == Convert.ToInt32(User.Identity.Name))
            {
                ViewBag.BlogEditor = blog.BlogEditor;
                ViewBag.IsModified = true;
                ViewBag.BlogType = blog.Type;
                return View("SaveBlog", blog);
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 修改博客
        /// </summary>
        /// <param name="id"></param>
        /// <param name="blog"></param>
        /// <returns></returns>
        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult ModBlog(int id, Blog blog)
        {
            

            var targetBlog = entity.Blogs.Find(id);
            var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            if (targetBlog != null && targetBlog.Owner.Id == logined.Id && WebSecurity.IsValidBlog(blog))
            {
                targetBlog.Owner = logined;
                targetBlog.LastModTime = DateTime.Now;
                targetBlog.Theme = blog.Theme.Trim();
                targetBlog.Content = blog.Content;
                if (targetBlog.Type != BlogType.Diary)
                {
                    if (!targetBlog.IsPublic && blog.IsPublic)
                    {
                        targetBlog.IsPublic = true;
                        targetBlog.PublicTime = DateTime.Now;
                    }
                }
                if (!string.IsNullOrWhiteSpace(blog.Tag))
                {
                    var str = "";
                    var subStrs = blog.Tag.ToLower().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    var count = subStrs.Count() < 10 ? subStrs.Count() : 10;
                    for (int i = 0; i < count - 1; i++)
                    {
                        if (subStrs.ElementAt(i).Length < 51)
                        {
                            str += subStrs.ElementAt(i) + ",";
                        }
                        else
                        {
                            str += subStrs.ElementAt(i).Substring(0, 50) + ",";
                        }
                    }
                    if (subStrs.ElementAt(count - 1).Length < 51)
                    {
                        str += subStrs.ElementAt(count - 1);
                    }
                    else
                    {
                        str += subStrs.ElementAt(count - 1).Substring(0, 50);
                    }
                    targetBlog.Tag = str;
                }

                entity.SaveChanges();
                return RedirectToAction("DoneSaveBlog", new { id });
            }
            else
            {
                return Content(WebHelper.SweetAlert("修改失败", "提交的信息不合法!"));
            }
        }

        /// <summary>
        /// 修改博客草稿到榜中
        /// </summary>
        /// <param name="id"></param>
        /// <param name="blog"></param>
        /// <returns></returns>
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ModBlogToTopList(int id, Blog blog)
        {
            var targetBlog = entity.Blogs.Find(id);
            var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            if (targetBlog != null && targetBlog.Owner.Id == logined.Id && targetBlog.Type == BlogType.Jotting && !targetBlog.IsPublic && WebSecurity.IsValidBlog(blog))
            {
                var tag = "";
                if (!string.IsNullOrWhiteSpace(blog.Tag))
                {
                    var subStrs = blog.Tag.ToLower().Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).Distinct();
                    var count = subStrs.Count() < 10 ? subStrs.Count() : 10;
                    for (int i = 0; i < count - 1; i++)
                    {
                        if (subStrs.ElementAt(i).Length < 51)
                        {
                            tag += subStrs.ElementAt(i) + ",";
                        }
                        else
                        {
                            tag += subStrs.ElementAt(i).Substring(0, 50) + ",";
                        }
                    }
                    if (subStrs.ElementAt(count - 1).Length < 51)
                    {
                        tag += subStrs.ElementAt(count - 1);
                    }
                    else
                    {
                        tag += subStrs.ElementAt(count - 1).Substring(0, 50);
                    }
                }
                entity.Blogs.Remove(targetBlog);
                entity.SaveChanges();

                Dictionary<string, string> requstData = new Dictionary<string, string> {
                    {"Tag", tag },
                    {"Owner_Id", logined.Id.ToString() },
                    {"Content", blog.Content },
                    {"Theme",  blog.Theme.Trim() },
                    {"Type", ((int)BlogType.Jotting).ToString() },
                    {"BlogEditor", ((int)logined.UserConfig.BlogEditor).ToString() },
                    {"Account", logined.Account },
                    {"Password", logined.Password },
                };
                string result = await WebHelper.HttpRequest("http://119.29.209.102:8080/appserver/addToBlogTop", requstData);
                if (result == "1")
                {
                    return Content(WebHelper.SweetAlert("入榜成功", "博客成功入榜，可在手机App上查看，待到放榜之时就是它回归之日!", "location.href='/User/ManageBlog'"));
                }
                else
                {
                    return Content(WebHelper.SweetAlert("入榜失败", $"可能由于网络原因或访问受限，请求发送失败，请刷新重试!"));
                }
            }
            else
            {
                return Content(WebHelper.SweetAlert("入榜失败", "提交的信息不合法!"));
            }
        }

        /// <summary>
        /// 删除博客
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DelBlog(int id)
        {
            var blog = entity.Blogs.Find(id);
            if (blog != null && blog.Owner.Id == Convert.ToInt32(User.Identity.Name))
            {
                var typeText = "";
                if (blog.Type == BlogType.Jotting)
                {
                    typeText = "随笔";
                }
                else if (blog.Type == BlogType.Article)
                {
                    typeText = "文章";
                }
                else
                {
                    typeText = "日记";
                }
                entity.Blogs.Remove(blog);
                entity.SaveChanges();
                return Content(WebHelper.SweetAlert("删除成功", $"成功删除这篇{typeText}!"));
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 搜索公告博客
        /// </summary>
        /// <param name="searchContent"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult SearchLoginUserBlog(string searchContent, int? page)
        {
            var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            ViewBag.Logined = logined;
            ViewBag.Title = "搜索 - 个人博客";
            if (string.IsNullOrWhiteSpace(searchContent))
            {
                return View("Blogs", new List<Blog>().ToPagedList(1, 10));
            }
            else
            {
                searchContent = searchContent.Trim();
                var blogs = logined.Blogs.Where(m => m.Theme.Contains(searchContent)).OrderByDescending(m => m.Id);
                var pageNumber = page ?? 1;

                var onePageOfProducts = blogs.ToPagedList(pageNumber, 10);
                ViewBag.SearchContent = searchContent;
                return View("Blogs", onePageOfProducts);
            }
        }

        /// <summary>
        /// 按标签搜索博客
        /// </summary>
        /// <param name="searchContent"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult SearchTagBlog(string searchContent, int? page)
        {
            var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            ViewBag.Logined = logined;
            ViewBag.Title = "标签搜索 - 个人博客";
            if (string.IsNullOrWhiteSpace(searchContent))
            {
                return View("Blogs", new List<Blog>().ToPagedList(1, 10));
            }
            else
            {
                searchContent = searchContent.Trim();
                var blogs = logined.Blogs.Where(m => m.Tag.Contains(searchContent)).OrderByDescending(m => m.Id);
                var pageNumber = page ?? 1;

                var onePageOfProducts = blogs.ToPagedList(pageNumber, 10);
                ViewBag.SearchContent = searchContent;
                return View("Blogs", onePageOfProducts);
            }
        }

        #endregion

        #region 收藏博客管理

        /// <summary>
        /// 收藏夹
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Favorites(int? page)
        {
            var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            ViewBag.PublicBlog = logined.UserConfig.PublicBlog;
            ViewBag.Account = logined.Account;

            var blogs = logined.Favorites.Blogs;
            var pageNumber = page ?? 1;
            ViewBag.PageNumber = pageNumber;
            var onePageOfProducts = blogs.ToPagedList(pageNumber, 10);
            return View("ManageBlog", onePageOfProducts);
        }

        /// <summary>
        /// 收藏博客
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddBlogToFavorites(int id)
        {
            var blog = entity.Blogs.Find(id);
            if (blog != null && blog.IsPublic)
            {
                var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
                if(logined.Favorites.Blogs.Count(m => m.Id == id) > 0)
                {
                    return Content(WebHelper.SweetAlert("温馨提示", "你已经已收藏了该博客!"));
                }
                else
                {
                    logined.Favorites.Blogs.Add(blog);
                    entity.SaveChanges();
                    return Content(WebHelper.SweetAlert("收藏成功", "成功收藏该博客!"));
                }
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 删除收藏的博客
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DelBlogToFavorites(int id)
        {
            var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            var blog = logined.Favorites.Blogs.SingleOrDefault(m => m.Id == id);
            if (blog != null)
            {
                logined.Favorites.Blogs.Remove(blog);
                entity.SaveChanges();
                return Content(WebHelper.SweetAlert("删除成功", "成功从收藏夹中删除博客!"));
            }
            else
            {
                return View("Error");
            }
        }

        #endregion

        #region 推荐博客管理
        /// <summary>
        /// 推荐博客
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddBlogToRecommendations(int id)
        {
            var blog = entity.Blogs.Find(id);
            if (blog != null && blog.IsPublic)
            {
                var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
                if(blog.Owner.Id != logined.Id)
                {
                    if(logined.Recommendations.Blogs.Count(m => m.Id == id) >  0)
                    {
                        return Content(WebHelper.SweetAlert("温馨提示", "你已经推荐了该博客!"));
                    }
                    else
                    {
                        logined.Recommendations.Blogs.Add(blog);
                        entity.SaveChanges();
                        return Content(WebHelper.SweetAlert("推荐成功", "成功推荐该博客!"));
                    }
                }
                else
                {
                    return Content(WebHelper.SweetAlert("推荐失败", "不能推荐自己的内容!"));
                }
            }
            else
            {
                return View("Error");
            }
        }

        #endregion

        #endregion

        #endregion

        #region 资源

        #region 资源专栏
        /// <summary>
        /// 资源专栏
        /// </summary>
        /// <returns></returns>
        public ActionResult ResourceColumn(int? page)
        {
            var groups = entity.FileGroups.Where(m => (m.Accessible & Accessible.Inner) != 0);
            var pageNumber = page ?? 1;
            var onePageOfProducts = groups.ToPagedList(pageNumber, 10);
            return View(onePageOfProducts);
        }

        /// <summary>
        /// 显示资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ColumnResources(int id, int? page)
        {
            var fileGroup = entity.FileGroups.Find(id);
            if (fileGroup != null && (fileGroup.Accessible & Accessible.Inner) != 0)
            {
                return View(fileGroup);
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 阅读文档
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LookColumnDocument(int id)
        {
            var file = entity.WebFiles.Find(id);
            if (file != null && file.Type == FileType.Document && file.Groups.Count(m => (m.Accessible & Accessible.Inner) != 0) > 0)
            {
                var path = Server.MapPath("~/WingStudio/Resource/") + file.FilePath;
                if (System.IO.File.Exists(path))
                {

                    file.LookCount++;
                    entity.SaveChanges();

                    //.doc(x)、.txt、.xls(x)、.ppt(x)、.pdf

                    return File(path, "application/octet-stream", Url.Encode(file.Name));
                }
                else
                {
                    //file.Groups.Clear();
                    entity.WebFiles.Remove(file);
                    entity.SaveChanges();
                    return Content(WebHelper.SweetAlert("操作提示", "对不起，该资源已丢失!给你带来困扰，非常抱歉!"));
                }
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadColumnResource(int id)
        {
            var file = entity.WebFiles.Find(id);
            if (file != null && file.Groups.Count(m => (m.Accessible & Accessible.Inner) != 0) > 0)
            {
                var path = Server.MapPath("~/WingStudio/Resource/") + file.FilePath;
                if (System.IO.File.Exists(path))
                {
                    file.LoadCount++;
                    entity.SaveChanges();
                    return File(path, "application/octet-stream", Url.Encode(file.Name));
                }
                else
                {
                    //file.Groups.Clear();
                    entity.WebFiles.Remove(file);
                    entity.SaveChanges();
                    return Content(WebHelper.SweetAlert("操作提示", "对不起，该资源已丢失!给你带来困扰，非常抱歉!"));
                }
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 播放视频
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PlayColumnVideo(int id)
        {
            var file = entity.WebFiles.Find(id);
            if (file != null && file.Type == FileType.Video && file.Groups.Count(m => (m.Accessible & Accessible.Inner) != 0) > 0)
            {
                var path = Server.MapPath("~/WingStudio/Resource/") + file.FilePath;
                if (System.IO.File.Exists(path))
                {
                    file.LookCount++;
                    entity.SaveChanges();
                    ViewBag.FileName = file.Name;
                    ViewBag.TargetSource = file.FilePath;
                    return View("PlayVideo");
                }
                else
                {
                    //file.Groups.Clear();
                    entity.WebFiles.Remove(file);
                    entity.SaveChanges();
                    return Content(WebHelper.SweetAlert("操作提示", "对不起，该资源已丢失!给你带来困扰，非常抱歉!"));
                }
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 搜索博客
        /// </summary>
        /// <param name="searchContent"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult SearchColumnResource(string searchContent, int? page)
        {
            ViewBag.IsSearch = true;
            if (string.IsNullOrWhiteSpace(searchContent))
            {
                return View("Resources", new List<WebFile>().ToPagedList(1, 10));
            }
            else
            {
                searchContent = searchContent.Trim();
                var files = entity.WebFiles.Where(m => m.Groups.Count(g => (g.Accessible & Accessible.Inner) != 0) > 0).Where(m => m.Name.Contains(searchContent)).OrderByDescending(m => m.Id);
                var pageNumber = page ?? 1;
                ViewBag.PageNumber = pageNumber;
                var onePageOfProducts = files.ToPagedList(pageNumber, 10);
                ViewBag.SearchContent = searchContent;
                return View("Resources", onePageOfProducts);
            }
        }

        #endregion

        #region 资源管理
        /// <summary>
        /// 管理资源
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageResource(string type)
        {
            var fileType = FileType.Document;
            if (type == "picture")
            {
                fileType = FileType.Picture;
            }
            else if (type == "picture")
            {
                fileType = FileType.Picture;
            }
            else if (type == "picture")
            {
                fileType = FileType.Picture;
            }
            else if (type == "picture")
            {
                fileType = FileType.Picture;
            }
            else if (type == "picture")
            {
                fileType = FileType.Picture;
            }
            ViewBag.FileType = fileType;

            var loginId = Convert.ToInt32(User.Identity.Name);
            var folders = entity.WebFolders.Where(m => m.Owner.Id == loginId && m.ParentFolder == null);
            return View(folders);
        }

        /// <summary>
        /// 显示资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Resources(int id)
        {
            var folder = entity.WebFolders.Find(id);
            if (folder != null && folder.Owner.Id == Convert.ToInt32(User.Identity.Name))
            {
                return View(folder);
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddFile(int id, HttpPostedFileBase file)
        {
            var folder = entity.WebFolders.Find(id);
            var loginId = Convert.ToInt32(User.Identity.Name);
            if (folder != null && folder.Owner.Id == loginId)
            {
                if (file != null && file.ContentLength > 0)
                {
                    int fileSize = file.ContentLength;
                    if (WebSecurity.UploadOverLimit(fileSize, entity.WebFiles.Where(m => m.Owner.Id == loginId).ToList()))
                    {
                        return Json(new { title = "上传失败", message = "所要上传的资源操过了资源容量上限(1G)，可选择删除部分已上传资源!" });
                    }
                    //文件不超过100M
                    if (fileSize <= 104857600)
                    {

                        string fileName = file.FileName.Split('\\').Last();
                        string fileExt = "", name = fileName;
                        if (fileName.LastIndexOf('.') > -1)
                        {
                            fileExt = fileName.Substring(fileName.LastIndexOf('.')).ToLower();
                            name = fileName.Substring(0, fileName.LastIndexOf('.'));
                        }
                        if (!WebSecurity.IsValidFileExt(folder.Type, fileExt))
                        {
                            return Json(new { title = "上传失败", message = "文件的后缀名不合法!" });
                        }

                        string path = Server.MapPath("~/WingStudio/Resource");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        fileName = DateTime.Now.ToFileTime().ToString() + Guid.NewGuid().ToString("N") + fileExt;
                        file.SaveAs(Path.Combine(path, fileName));//从客户端保存文件到本地

                        var newFile = new WebFile();
                        newFile.Owner = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
                        newFile.Name = name;
                        newFile.FilePath = fileName;
                        newFile.FileSize = fileSize;
                        newFile.Type = folder.Type;

                        folder.SubFiles.Add(newFile);
                        entity.SaveChanges();
                        return Json(new { title = "添加成功", message = "成功添加文件!" });
                    }
                    else
                    {
                        return Json(new { title = "添加失败", message = "文件的大小不合要求!" });
                    }
                }
                else
                {
                    return Json(new { title = "添加失败", message = "文件的不能为空!" });
                }
            }
            else
            {
                return Json(new { title = "添加失败", message = "无权访问!" });
            }
        }

        /// <summary>
        /// 添加文件夹
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddFolder(int id, string name)
        {
            var folder = entity.WebFolders.Find(id);
            var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
            if (folder != null && folder.Owner.Id == logined.Id)
            {
                if (!string.IsNullOrWhiteSpace(name) && name.Length < 21)
                {
                    var newFolder = new WebFolder();
                    newFolder.Owner = logined;
                    newFolder.Name = name;
                    newFolder.Type = folder.Type;

                    folder.SubFolders.Add(newFolder);
                    entity.SaveChanges();

                    return Content(WebHelper.SweetAlert("添加成功", "成功添加文件夹!"));
                }
                else
                {
                    return Content(WebHelper.SweetAlert("添加失败", "文件夹名称不合法!"));
                }
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 修改文件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModFile(int id, string newName)
        {
            var file = entity.WebFiles.Find(id);
            if (file != null && file.Owner.Id == Convert.ToInt32(User.Identity.Name))
            {
                if (!string.IsNullOrWhiteSpace(newName) && newName.Length < 21)
                {
                    file.Name = newName;
                    file.LastModTime = DateTime.Now;
                    entity.SaveChanges();

                    return Content(WebHelper.SweetAlert("修改成功", "成功重命名文件!"));
                }
                else
                {
                    return Content(WebHelper.SweetAlert("修改失败", "文件名称不合法!"));
                }
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 修改文件夹
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModFolder(int id, string newName)
        {
            var folder = entity.WebFolders.Find(id);
            if (folder != null && folder.Owner.Id == Convert.ToInt32(User.Identity.Name))
            {
                if (!string.IsNullOrWhiteSpace(newName) && newName.Length < 21)
                {
                    folder.Name = newName;
                    folder.LastModTime = DateTime.Now;
                    entity.SaveChanges();

                    return Content(WebHelper.SweetAlert("修改成功", "成功重命名文件夹!"));
                }
                else
                {
                    return Content(WebHelper.SweetAlert("修改失败", "文件夹名称不合法!"));
                }
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DelFile(int id)
        {
            var file = entity.WebFiles.Find(id);
            if (file != null && file.Owner.Id == Convert.ToInt32(User.Identity.Name))
            {
                //file.Groups.Clear();
                entity.WebFiles.Remove(file);
                entity.SaveChanges();
                return Content(WebHelper.SweetAlert("删除成功", "成功删除文件!"));
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DelFolder(int id)
        {
            var folder = entity.WebFolders.Find(id);
            if (folder != null && folder.Owner.Id == Convert.ToInt32(User.Identity.Name))
            {
                DelFolder(folder);
                entity.SaveChanges();
                return Content(WebHelper.SweetAlert("删除成功", "成功删除文件夹!"));
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="folder"></param>
        private void DelFolder(WebFolder folder)
        {
            entity.WebFiles.RemoveRange(folder.SubFiles);
            foreach (var item in folder.SubFolders)
            {
                DelFolder(item);
            }
            entity.WebFolders.Remove(folder);
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CopyFileTo(int id, int targetId)
        {
            var file = entity.WebFiles.Find(id);
            var folder = entity.WebFolders.Find(targetId);
            if (file != null && folder != null && file.Owner.Id == Convert.ToInt32(User.Identity.Name) && folder.Owner.Id == Convert.ToInt32(User.Identity.Name) && file.Type == folder.Type)
            {
                if (file.ParentFolder.Id == folder.Id)
                {
                    return Content(WebHelper.SweetAlert("复制失败", "不能见文件复制到自身!"));
                }
                else
                {
                    var newFile = (WebFile)file.Clone();
                    folder.SubFiles.Add(newFile);
                    entity.SaveChanges();

                    return Content(WebHelper.SweetAlert("复制成功", "成功复制文件到指定文件夹!"));
                }
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="id"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CopyFolderTo(int id, int targetId)
        {
            var folder = entity.WebFolders.Find(id);
            var targetFolder = entity.WebFolders.Find(targetId);
            if (folder != null && folder.ParentFolder != null && targetFolder != null && targetFolder.Owner.Id == Convert.ToInt32(User.Identity.Name) && folder.Owner.Id == Convert.ToInt32(User.Identity.Name) && targetFolder.Type == folder.Type)
            {
                if (folder.ParentFolder.Id == targetId)
                {
                    return Content(WebHelper.SweetAlert("复制失败", "不能见文件夹复制到自身或其子目录!"));
                }
                else
                {
                    var temp = targetFolder.ParentFolder;
                    while (temp != null)
                    {
                        if (temp.ParentFolder.Id == id)
                        {
                            return Content(WebHelper.SweetAlert("移动失败", "不能见文件夹移动到自身或其子目录!"));
                        }
                        else
                        {
                            temp = temp.ParentFolder;
                        }
                    }

                    var newFolder = (WebFolder)folder.Clone();
                    targetFolder.SubFolders.Add(newFolder);
                    entity.SaveChanges();
                    return Content(WebHelper.SweetAlert("复制成功", "成功复制文件夹到指定文件夹!"));
                }
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MoveFileTo(int id, int targetId)
        {
            var file = entity.WebFiles.Find(id);
            var folder = entity.WebFolders.Find(targetId);
            if (file != null && folder != null && file.Owner.Id == Convert.ToInt32(User.Identity.Name) && folder.Owner.Id == Convert.ToInt32(User.Identity.Name) && file.Type == folder.Type)
            {
                if (file.ParentFolder.Id == targetId)
                {
                    return Content(WebHelper.SweetAlert("移动失败", "不能见文件移动到自身!"));
                }
                else
                {
                    file.LastModTime = DateTime.Now;
                    folder.SubFiles.Add(file);

                    entity.SaveChanges();

                    return Content(WebHelper.SweetAlert("移动成功", "成功移动文件到指定文件夹!"));
                }
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 移动文件夹
        /// </summary>
        /// <param name="id"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MoveFolderTo(int id, int targetId)
        {
            var folder = entity.WebFolders.Find(id);
            var targetFolder = entity.WebFolders.Find(targetId);
            if (folder != null && folder.ParentFolder != null && targetFolder != null && targetFolder.Owner.Id == Convert.ToInt32(User.Identity.Name) && folder.Owner.Id == Convert.ToInt32(User.Identity.Name) && targetFolder.Type == folder.Type)
            {
                if (folder.ParentFolder.Id == targetId)
                {
                    return Content(WebHelper.SweetAlert("移动失败", "不能见文件夹移动到自身或其子目录!"));
                }
                else
                {
                    var temp = targetFolder.ParentFolder;
                    while (temp != null)
                    {
                        if (temp.ParentFolder.Id == id)
                        {
                            return Content(WebHelper.SweetAlert("移动失败", "不能见文件夹移动到自身或其子目录!"));
                        }
                        else
                        {
                            temp = temp.ParentFolder;
                        }
                    }

                    folder.LastModTime = DateTime.Now;
                    targetFolder.SubFolders.Add(folder);

                    entity.SaveChanges();
                    return Content(WebHelper.SweetAlert("移动成功", "成功移动文件夹到指定文件夹!"));
                }
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 阅读文档
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LookDocument(int id)
        {
            var file = entity.WebFiles.Find(id);
            if (file != null && file.Type == FileType.Document && file.Owner.Id == Convert.ToInt32(User.Identity.Name))
            {
                var path = Server.MapPath("~/WingStudio/Resource/") + file.FilePath;
                if (System.IO.File.Exists(path))
                {

                    file.LookCount++;
                    entity.SaveChanges();

                    //.doc(x)、.txt、.xls(x)、.ppt(x)、.pdf

                    return File(path, "application/octet-stream", Url.Encode(file.Name));
                }
                else
                {
                    //file.Groups.Clear();
                    entity.WebFiles.Remove(file);
                    entity.SaveChanges();
                    return Content(WebHelper.SweetAlert("操作提示", "对不起，该资源已丢失!给你带来困扰，非常抱歉!"));
                }
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LoadResource(int id)
        {
            var file = entity.WebFiles.Find(id);
            if (file != null && file.Owner.Id == Convert.ToInt32(User.Identity.Name))
            {
                var path = Server.MapPath("~/WingStudio/Resource/") + file.FilePath;
                if (System.IO.File.Exists(path))
                {
                    file.LoadCount++;
                    entity.SaveChanges();
                    return File(path, "application/octet-stream", Url.Encode(file.Name));
                }
                else
                {
                    //file.Groups.Clear();
                    entity.WebFiles.Remove(file);
                    entity.SaveChanges();
                    return Content(WebHelper.SweetAlert("操作提示", "对不起，该资源已丢失!给你带来困扰，非常抱歉!"));
                }
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 播放视频
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PlayVideo(int id)
        {
            var file = entity.WebFiles.Find(id);
            if (file != null && file.Type == FileType.Video && file.Owner.Id == Convert.ToInt32(User.Identity.Name))
            {
                var path = Server.MapPath("~/WingStudio/Resource/") + file.FilePath;
                if (System.IO.File.Exists(path))
                {
                    file.LookCount++;
                    entity.SaveChanges();
                    ViewBag.FileName = file.Name;
                    ViewBag.TargetSource = file.FilePath;
                    return View("PlayVideo");
                }
                else
                {
                    //file.Groups.Clear();
                    entity.WebFiles.Remove(file);
                    entity.SaveChanges();
                    return Content(WebHelper.SweetAlert("操作提示", "对不起，该资源已丢失!给你带来困扰，非常抱歉!"));
                }
            }
            else
            {
                return View("Error");
            }
        }

        #endregion

        #endregion

        #region 消息

        /// <summary>
        /// 管理接收的信息
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ManageReceivedMsg(int? page)
        {

            ViewBag.Read = true;
            var id = Convert.ToInt32(User.Identity.Name);
            var msgs = from msg in entity.Messages
                       join packet in entity.Packets
                       on msg.Id equals packet.Message.Id
                       where packet.TargetId == id && !packet.Deleted && packet.Read
                       select msg;
            ViewBag.ReadingMsgCount = entity.Messages.Count(m => m.Packets.Count(n => !n.Deleted && !n.Read && n.TargetId == id) > 0);
            var pageNumber = page ?? 1;
            ViewBag.PageNumber = pageNumber;
            var onePageOfProducts = msgs.OrderByDescending(m => m.Id).ToPagedList(pageNumber, 10);
            return View("ManageReceivedMsg", onePageOfProducts);
        }

        /// <summary>
        /// 待读信息
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ReadingMsg(int? page)
        {
            ViewBag.Read = false;
            var id = Convert.ToInt32(User.Identity.Name);
            var msgs = from msg in entity.Messages
                       join packet in entity.Packets
                       on msg.Id equals packet.Message.Id
                       where packet.TargetId == id && !packet.Deleted && !packet.Read
                       select msg;

            int msgCount = msgs.Count();
            if (msgCount > 0)
            {
                ViewBag.ReadingMsgCount = msgCount;
            }
            else
            {
                return RedirectToAction("ManageReceivedMsg");
            }
            var pageNumber = page ?? 1;
            ViewBag.PageNumber = pageNumber;
            var onePageOfProducts = msgs.OrderByDescending(m => m.Id).ToPagedList(pageNumber, 10);
            return View("ManageReceivedMsg", onePageOfProducts);
        }

        /// <summary>
        /// 查看信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult LookMessage(int id)
        {
            var msg = entity.Messages.Find(id);
            if (msg != null)
            {
                var state = msg.Packets.SingleOrDefault(m => m.TargetId == Convert.ToInt32(User.Identity.Name));
                if (state != null && !state.Deleted)
                {
                    if (!state.Read)
                    {
                        state.Read = true;
                        entity.SaveChanges();
                    }
                    ViewBag.Owner = "管理员";
                    return View(msg);
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 标识为已读
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult HadRead(int id, int? page)
        {
            var msg = entity.Messages.Find(id);
            if (msg != null)
            {
                var loginId = Convert.ToInt32(User.Identity.Name);
                var state = msg.Packets.SingleOrDefault(m => m.TargetId == loginId);
                if (state != null && !state.Deleted && !state.Read)
                {
                    state.Read = true;
                    entity.SaveChanges();
                    var msgCount = entity.Messages.Count(m => m.Packets.Count(n => !n.Deleted && !n.Read && n.TargetId == loginId) > 0);
                    if (msgCount > 0)
                    {
                        return RedirectToAction("ReadingMsg", new { page = page ?? 1 });
                    }
                    else
                    {
                        return RedirectToAction("ManageReceivedMsg");
                    }
                }
                else
                {
                    return Content(WebHelper.SweetAlert("设置失败", "非法操作!"));
                }
            }
            else
            {
                return Content(WebHelper.SweetAlert("设置失败", "非法操作!"));
            }
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DelMessage(int id)
        {
            var msg = entity.Messages.Find(id);
            if (msg != null)
            {
                var state = msg.Packets.SingleOrDefault(m => m.TargetId == Convert.ToInt32(User.Identity.Name));
                if (state != null && !state.Deleted)
                {
                    state.Deleted = true;
                    entity.SaveChanges();
                    return Content(WebHelper.SweetAlert("删除成功", "成功删除信息!"));
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult SendMessage()
        {
            var id = Convert.ToInt32(User.Identity.Name);
            ViewBag.ReadingMsgCount = entity.Messages.Count(m => m.Packets.Count(n => !n.Deleted && !n.Read && n.TargetId == id) > 0);
            return View("SaveMessage");
        }

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(Message msg)
        {
            if (msg != null && WebSecurity.IsValidMessage(msg))
            {
                var logined = entity.Users.Find(Convert.ToInt32(User.Identity.Name));
                Message newMsg = new Message();
                newMsg.Content = msg.Content;
                newMsg.Theme = msg.Theme.Trim();
                newMsg.OwnId = logined.Id;
                newMsg.Packets = new HashSet<Packet>();
                var packet = new Packet();
                packet.TargetId = 0;
                newMsg.Packets.Add(packet);
                entity.Messages.Add(newMsg);
                entity.SaveChanges();
                return Content(WebHelper.SweetAlert("发送成功", "成功发送反馈信息!", "location.href='/User/ManageReceivedMsg'"));
            }
            else
            {
                return Content(WebHelper.SweetAlert("发送失败", "提交的信息不合法!"));
            }
        }

        #endregion

    }
}
