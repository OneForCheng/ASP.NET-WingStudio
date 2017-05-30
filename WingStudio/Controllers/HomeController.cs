using ForCheng.Security.AES;
using ForCheng.Web.ValidateCode;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using WingStudio.Models;

namespace WingStudio.Controllers
{
    public class HomeController : BaseController
    {

        #region 主页

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            
            if (TempData["NoLogin"] != null)
            {   
                ViewBag.Login = true;
            }
            var notices = Entity.Notices.Where(m => m.IsPublic);
            var dynamics = Entity.Dynamics.Where(m => m.IsPublic);
            var timeLimit = DateTime.Now.AddMonths(-1);
            ViewBag.LongNotices = notices.Where(m => m.IsLong).OrderByDescending(m => m.Id).Take(7);
            ViewBag.Notices = notices.OrderByDescending(m => m.Id).Take(7);
            ViewBag.FormalDynamics = dynamics.Where(m => m.IsFormal).OrderByDescending(m => m.Id).Take(7);
            ViewBag.UnFormalDynamics = dynamics.Where(m => !m.IsFormal).OrderByDescending(m => m.Id).Take(10);
            ViewBag.BlogGroups = Entity.BlogGroups.Where(m => (m.Accessible & Accessible.Outer) != 0).OrderByDescending(m => m.Id).Take(10);
            ViewBag.FileGroups = Entity.FileGroups.Where(m => (m.Accessible & Accessible.Outer) != 0).OrderByDescending(m => m.Id).Take(10);
            //ViewBag.NewBlogs = entity.Blogs.Where(m => m.IsPublic).Where(m => m.Owner.UserConfig.PublicBlog || m.Checked && m.Groups.Count(n => (n.Accessible & Accessible.Outer) != 0) > 0).OrderByDescending(m => m.PublicTime).Take(5);
            ViewBag.HotBlogs = Entity.Blogs.Where(m => m.IsPublic).Where(m => m.Owner.UserConfig.PublicBlog || m.Checked && m.Groups.Count(n => (n.Accessible & Accessible.Outer) != 0) > 0).Where(m => m.CreateTime > timeLimit).OrderByDescending(m => m.LookCount).Take(5);
            //ViewBag.NewFiles = entity.WebFiles.Where(m => m.Checked && m.Groups.Count(n => (n.Accessible & Accessible.Outer) != 0) > 0).OrderByDescending(m => m.Id).Take(5);
            var apps = Entity.Applications.Where(m => m.StartTime <= DateTime.Now && m.EndTime >= DateTime.Now).OrderByDescending(m => m.Id);
            if(apps.Any())
            {
                ViewBag.ExistApp = true;
                ViewBag.App = apps.First();
            }
            else
            {
                ViewBag.ExistApp = false;
            }
            return View();
        }

        #endregion

        #region 关于联系页面

        /// <summary>
        /// 关于我们页面
        /// </summary>
        /// <returns></returns>
        public ActionResult About()
        {
            ViewBag.Page = "About";
            return View("About");
        }

        /// <summary>
        /// 联系我们页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            ViewBag.Page = "Contact";
            return View("About");
        }

        /// <summary>
        /// 工作室简介
        /// </summary>
        /// <returns></returns>
        public ActionResult Studio()
        {
            ViewBag.Page = "Studio";
            return View("About");
        }
        #endregion

        #region 错误处理页面

        /// <summary>
        /// 内部服务器错误
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            ViewBag.Page = "Error";
            return View("Error");
        }

        /// <summary>
        /// 找不到指定页面或
        /// </summary>
        /// <returns></returns>
        public ActionResult PageNoFund()
        {
            ViewBag.Page = "PageNoFund";
            return View("Error");
        }

        /// <summary>
        /// 越权访问跳转页面
        /// </summary>
        /// <returns></returns>
        public ActionResult InvalidPermission()
        {
            ViewBag.Page = "InvalidPermission";
            return View("Error");
        }

        /// <summary>
        /// 浏览器版本太低
        /// </summary>
        /// <returns></returns>
        public ActionResult BrowserVersionLow()
        {
            ViewBag.Page = "BrowserVersionLow";
            return View("Error");
        }

        #endregion

        #region 登录注销

        [HttpGet]
        public ActionResult Login()
        {
            TempData["NoLogin"] = "NoLogin";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user">登录用户</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUser user)
        {
            if (WebSecurity.IsValidLoginUser(user))
            {
                var role = "User";
                dynamic logined;
                if (user.IsAdmin)
                {
                    logined = Entity.SuperUsers.SingleOrDefault(m => m.Account == user.Account);
                    if (logined != null)
                    {
                        role = "SuperAdmin";
                    }
                    else
                    {
                        logined = Entity.Users.SingleOrDefault(m => m.Account == user.Account);
                        if (logined != null && ((User)logined).Groups.Any())
                        {
                            role = "Admin";
                        }
                        else
                        {
                            return Json(new { title = "登录失败", error = "登录信息错误!" });
                        }
                    }
                }
                else
                {
                    logined = Entity.Users.SingleOrDefault(m => m.Account == user.Account);
                    if (logined == null)
                    {
                        return Json(new { title = "登录失败", error = "登录信息错误!" });
                    }
                }

                if ((logined.Password != AES.Encrypt(user.Password)) || user.SecQuestion != logined.SecQuestion || (user.SecQuestion != SecurityFlag.None && logined.SecAnswer != AES.Encrypt(user.SecAnswer)))
                {
                    return Json(new { title = "登录失败", error = "登录信息错误!" });
                    
                }
                else
                {
                    if(role == "User")
                    {
                        if(logined.IsForbidden)
                        {
                            return Json(new { title = "登录失败", error = "登录受到限制,请与管理员联系!" });
                        }
                    }
                    var authTicket = new FormsAuthenticationTicket(
                        1,
                        logined.Id.ToString(),
                        DateTime.Now,
                        DateTime.Now.AddMonths(1),
                        false,
                        role
                    );
                    var encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    System.Web.HttpContext.Current.Response.Cookies.Add(authCookie);
                    InfoLog.Info($"Envent:[登录事件] LoginUser[Role:{role} Account:{user.Account} Id:{logined.Id}]");
                    switch (role)
                    {
                        case "User":
                            return Json(new { title = "登录成功", href = "/User" });
                        case "Admin":
                            return Json(new { title = "登录成功", href = "/Admin" });
                        default:
                            return Json(new { title = "登录成功", href = "/SuperAdmin" });
                    }
                }
            }
            else
            {
                return Json(new { title = "登录失败", error = "登录信息错误!" });
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        #endregion

        #region  忘记与找回密码

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        private string GetClientIp()
        {
            //可以透过代理服务器
            var clientIp = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //判断是否有代理服务器
            if (string.IsNullOrEmpty(clientIp))
            {
                //没有代理服务器,如果有代理服务器获取的是代理服务器的IP
                clientIp = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return clientIp;
        }

        /// <summary>
        /// 获取图片验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult GetValidateCode(string type)
        {
            var vCode = new ValidateCode();
            var code = vCode.CreateValidateString(CodeType.All);
            Session["ForgetPwdCode"] = "";
            Session["ApplyCode"] = "";
            if(type !=null && type.ToLower() == "forget")
            {
                Session["ForgetPwdCode"] = code;
            }
            else
            {
                Session["ApplyCode"] = code;
            }
            var bytes = vCode.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ForgotPassword(string account, string name, string code)
        {
            if (string.IsNullOrWhiteSpace(code) || Session["ForgetPwdCode"].ToString() == "" || Session["ForgetPwdCode"].ToString().ToUpper() != code.ToUpper())
            {
                return Json(new { title = "申请失败", message = "验证码错误!" });
            }
            if (account == null || !Regex.IsMatch(account, @"^[a-zA-Z0-9]{4,20}$") || string.IsNullOrWhiteSpace(name) || name.Trim().Length > 20)
            {
                return Json(new { title = "申请失败", message = "用户信息错误!" });
            }
            else
            {
                name = name.Trim();
                var user = Entity.Users.SingleOrDefault(m => m.Account == account && m.Name == name);
                if (user == null)
                {
                    return Json(new { title = "申请失败", message = "用户信息错误!" });
                }
                else
                {
                    var resetCode = Entity.ResetCodes.SingleOrDefault(m => m.Account == account);
                    if (resetCode != null)
                    {
                        if ((DateTime.Now - resetCode.CreateTime).TotalMinutes > 30)
                        {
                            //找回密码的重置码信息失效
                            Entity.ResetCodes.Remove(resetCode);
                            Entity.SaveChanges();
                        }
                        else
                        {
                            return Json(new { title = "申请失败", message = "重置密码信息已经发送，请勿重复提交请求!" });
                        }
                    }
                    
                    var guidCode = Guid.NewGuid().ToString("N");
                    var hostUrl = Request.Url?.AbsoluteUri.Replace(Request.RawUrl, "");//网站域名
                    var subjectInfo = "[WingStudio]重置密码通知";

                    var sr = new StreamReader(Server.MapPath("~/Page/ForgotPassword.html"));
                    var bodyInfo = sr.ReadToEnd().Replace("[Account]", account).Replace("[Code]", guidCode).Replace("[HostUrl]", hostUrl??"").Replace("[ValidTime]", (DateTime.Now.ToString(CultureInfo.InvariantCulture) + " - " + DateTime.Now.AddMinutes(30).ToString(CultureInfo.InvariantCulture))).Replace("[ClientIp]", GetClientIp()).Replace("[Year]", DateTime.Now.Year.ToString());
                    sr.Close();

                    if(WebHelper.SendMail(user.Email, subjectInfo, bodyInfo))
                    {
                        var newCode = new ResetCode
                        {
                            Account = account,
                            Value = guidCode
                        };
                        Entity.ResetCodes.Add(newCode);
                        Entity.SaveChanges();

                        var mail = user.Email;
                        var strs = mail.Split('@');
                        mail = mail.Substring(0, 2) + "***" + strs[0].Substring(strs[0].Length - 2) + "@" + strs[1];
                        return Json(new { title = "申请成功", message = $"邮件已成功发送至:{mail}!请及时登录邮箱来找回你的密码!(找回密码邮件有效期为30分钟)" });
                    }
                    else
                    {
                        return Json(new { title = "申请失败", message = "可能由于服务器繁忙，邮件发送失败!请稍后重试!" });
                    }
                }
            }
        }

        /// <summary>
        /// 密码找回页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Find(string code)
        {
            if(!string.IsNullOrWhiteSpace(code) && Entity.ResetCodes.Count(m => m.Value == code) > 0)
            {
                return View();
            }
            else
            {
                return View("Error"); 
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="account">用户名</param>
        /// <param name="code">确认码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ResetPassword(string account, string code, string newPassword)
        {
            if (WebSecurity.IsValidResetPassword(account, code, newPassword))
            {
                var user = Entity.Users.SingleOrDefault(m => m.Account == account);
                var oneCode = Entity.ResetCodes.SingleOrDefault(m => m.Account == account && m.Value == code);
                if (user != null && oneCode != null)
                {
                    Entity.ResetCodes.Remove(oneCode);
                    if ((DateTime.Now - oneCode.CreateTime).TotalMinutes <= 30)
                    {
                        user.Password = AES.Encrypt(newPassword);
                        user.SecQuestion = SecurityFlag.None;
                        Entity.SaveChanges();
                        return Content(WebHelper.SweetAlert("重置成功", "成功重置密码,并且安全提问改为默认设置!", "location.href='/Index'"));
                    }
                    else
                    {
                        Entity.SaveChanges();
                        return Content(WebHelper.SweetAlert("重置失败", "无效的确认码,请重新找回密码!", "window.history.go(-1);"));
                    }
                }
                else
                {
                    return Content(WebHelper.SweetAlert("重置失败", "提交信息不合法!", "window.history.go(-1);"));
                }
            }
            else
            {
                return Content(WebHelper.SweetAlert("重置失败", "提交信息不合法!", "window.history.go(-1);"));
            }
        }

        #endregion

        #region 公告页面

        /// <summary>
        /// 公告
        /// </summary>
        /// <param name="isLong">标识显示公告的类别</param>
        /// <param name="page">分页</param>
        /// <returns></returns>
        public ActionResult Notices(bool? isLong, int? page)
        {
            var flag = isLong ?? false;
            var notices = Entity.Notices.Where(m => m.IsPublic).OrderByDescending(m => m.Id);
            if (flag)
            {
                notices = Entity.Notices.Where(m => m.IsPublic && m.IsLong).OrderByDescending(m => m.Id);
            }

            ViewBag.IsLong = flag;
            var pageNumber = page ?? 1;
            ViewBag.PageNumber = pageNumber;
            var onePageOfProducts = notices.ToPagedList(pageNumber, 12);
            return View(onePageOfProducts);
        }

        /// <summary>
        /// 显示指定的公告
        /// </summary>
        /// <param name="id">公告id</param>
        /// <returns></returns>
        public ActionResult ShowNotice(int id)
        {
            var notice = Entity.Notices.Find(id);
            if (notice != null && (notice.IsPublic || (Request.IsAuthenticated && User.IsInRole("Admin"))))
            {
                notice.LookCount++;
                Entity.SaveChanges();
                var notices = Entity.Notices.Where(m => m.IsPublic);
                ViewBag.LastNotice = notices.Where(m => m.Id < id).OrderByDescending(m => m.Id).FirstOrDefault();
                ViewBag.NextNotice = notices.Where(m => m.Id > id).OrderBy(m => m.Id).FirstOrDefault(); 
                return View(notice);
            }
            return View("Error");
        }

        #endregion

        #region 动态页面

        /// <summary>
        /// 动态
        /// </summary>
        /// <param name="isFormal"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Dynamics(bool? isFormal, int? page)
        {
            var flag = isFormal ?? false;
            var dynamics = Entity.Dynamics.Where(m => m.IsPublic && m.IsFormal == flag).OrderByDescending(m => m.Id);
            ViewBag.IsFormal = flag;
            var pageNumber = page ?? 1;
            ViewBag.PageNumber = pageNumber;
            var onePageOfProducts = dynamics.ToPagedList(pageNumber, 12);
            return View(onePageOfProducts);
        }

        /// <summary>
        /// 显示动态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowDynamic(int id)
        {
            var dynamic = Entity.Dynamics.Find(id);
            if (dynamic != null && (dynamic.IsPublic || (Request.IsAuthenticated && User.IsInRole("Admin"))))
            {
                dynamic.LookCount++;
                Entity.SaveChanges();
                var dynamics = Entity.Dynamics.Where(m => m.IsPublic);
                ViewBag.LastDynamic = dynamics.Where(m => m.IsFormal == dynamic.IsFormal && m.Id < id).OrderByDescending(m => m.Id).FirstOrDefault();
                ViewBag.NextDynamic = dynamics.Where(m => m.IsFormal == dynamic.IsFormal && m.Id > id).OrderBy(m => m.Id).FirstOrDefault();
                return View(dynamic);
            }
            else
            {
                return View("Error");
            }
        }

        #endregion

        #region 博客专栏页面

        /// <summary>
        /// 博客专栏
        /// </summary>
        /// <returns></returns>
        public ActionResult BlogColumn(int ?page)
        {
            var groups = Entity.BlogGroups.Where(m => (m.Accessible & Accessible.Outer) != 0).OrderByDescending(m => m.Id);
            var pageNumber = page ?? 1;
            var onePageOfProducts = groups.ToPagedList(pageNumber, 12);
            return View(onePageOfProducts);
        }

        /// <summary>
        /// 专栏博客
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ColumnBlogs(int id, int? page)
        {
            var blogGroup = Entity.BlogGroups.Find(id);
            if (blogGroup != null && (blogGroup.Accessible & Accessible.Outer) != 0)
            {
                ViewBag.GroupId = id;
                ViewBag.Title = blogGroup.Theme + " - 博客专栏";
                
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
        /// 显示指定用户的博客
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Blogs(string id, int? page)
        {
            var user = Entity.Users.SingleOrDefault(m => m.Account == id);
            if (user != null)
            {
                ViewBag.Account = user.Account;
                ViewBag.Title = user.Account + " - 个人博客";
                ViewBag.Avatar = user.Avatar;
                var pageNumber = page ?? 1;
                if (user.UserConfig.PublicBlog)
                {
                    ViewBag.PublicBlog = true;
                    var blogs = user.Blogs.Where(m => m.IsPublic).OrderByDescending(m => m.PublicTime);
                    var onePageOfProducts = blogs.ToPagedList(pageNumber, 10);
                    return View("Blogs", onePageOfProducts);
                }
                else
                {
                    ViewBag.PublicBlog = false;
                    var blogs = user.Blogs.Where(m => m.Groups.Count(n => (n.Accessible & Accessible.Outer) != 0) > 0).OrderByDescending(m => m.PublicTime);
                    var onePageOfProducts = blogs.ToPagedList(pageNumber, 10);
                    return View("Blogs", onePageOfProducts);
                }
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
            var blogs = Entity.Blogs.Where(m => m.Groups.Count(n => (n.Accessible & Accessible.Outer) != 0) > 0).OrderByDescending(m => m.PublicTime).ToPagedList(1, 10);
            return View("Blogs", blogs);
        }

        /// <summary>
        /// 最热博客专栏
        /// </summary>
        /// <returns></returns>
        public ActionResult ColumnHotBlogs()
        {
            ViewBag.Title = "最热博客 - 博客专栏";
            var blogs = Entity.Blogs.Where(m => m.Groups.Count(n => (n.Accessible & Accessible.Outer) != 0) > 0).OrderByDescending(m => m.LookCount).ThenBy(m => m.Id).ToPagedList(1, 10);
            return View("Blogs", blogs);
        }

        /// <summary>
        /// 强推博客专栏
        /// </summary>
        /// <returns></returns>
        public ActionResult ColumnPraiseBlogs()
        {
            ViewBag.Title = "强推博客 - 博客专栏";
            var blogs = Entity.Blogs.Where(m => m.Groups.Count(n => (n.Accessible & Accessible.Outer) != 0) > 0).OrderByDescending(m => m.Recommendations.Count).ThenBy(m => m.LookCount).ToPagedList(1, 10);
            return View("Blogs", blogs);
        }

        /// <summary>
        /// 显示博客
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowBlog(int id)
        {
            var blog = Entity.Blogs.Find(id);
            if (blog != null && blog.IsPublic)
            {
                if(blog.Owner.UserConfig.PublicBlog)
                {
                    blog.LookCount++;
                    Entity.SaveChanges();
                    var blogs = Entity.Blogs.Where(m => m.Owner.Id == blog.Owner.Id && m.IsPublic);
                    ViewBag.LastBlog = blogs.Where(m => m.Id < id).OrderByDescending(m => m.Id).FirstOrDefault();
                    ViewBag.NextBlog = blogs.Where(m => m.Id > id).OrderBy(m => m.Id).FirstOrDefault();
                    return View("ShowBlog", blog);
                }
                else if(blog.Checked && blog.Groups.Count(m => (m.Accessible & Accessible.Outer) != 0) > 0)
                {
                    blog.LookCount++;
                    Entity.SaveChanges();
                    var blogs = Entity.Blogs.Where(m => m.Owner.Id == blog.Owner.Id && m.Groups.Count(n => (n.Accessible & Accessible.Outer) != 0) > 0);
                    ViewBag.LastBlog = blogs.Where(m => m.Id < id).OrderByDescending(m => m.Id).FirstOrDefault();
                    ViewBag.NextBlog = blogs.Where(m => m.Id > id).OrderBy(m => m.Id).FirstOrDefault(); 
                    return View("ShowBlog", blog);
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

        ///// <summary>
        ///// 显示入榜博客
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public ActionResult ShowToListBlog(int id)
        //{
        //    var blog = Entity.ToListBlogs.Find(id);
        //    if (blog != null)
        //    {
        //        blog.LookCount++;
        //        Entity.SaveChanges();
        //        return View("ShowToListBlog", blog);
        //    }
        //    else
        //    {
        //        return View("Error");
        //    }
        //}

        /// <summary>
        /// 搜索博客
        /// </summary>
        /// <param name="searchContent"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult SearchPublicBlog(string searchContent, int? page)
        {
            ViewBag.Title = "搜索博客";
            if (string.IsNullOrWhiteSpace(searchContent))
            {
                return View("Blogs", new List<Blog>().ToPagedList(1, 10));
            }
            else
            {
                searchContent = searchContent.Trim();
                var blogs = Entity.Blogs.Where(m => m.IsPublic).Where(m => m.Owner.UserConfig.PublicBlog || m.Groups.Count(g => (g.Accessible & Accessible.Outer) != 0) > 0).Where(m => m.Theme.Contains(searchContent)).OrderByDescending(m => m.Id);
                var pageNumber = page ?? 1;
                
                var onePageOfProducts = blogs.ToPagedList(pageNumber, 10);
                ViewBag.SearchContent = searchContent;
                return View("Blogs", onePageOfProducts);
            }
        }

        /// <summary>
        /// 按标签搜索博客
        /// </summary>
        /// <param name="id">博客Id</param>
        /// <param name="searchContent"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult SearchTagBlog(int id, string searchContent, int? page)
        {
            var user = Entity.Users.Find(id);
            if(user != null)
            {
                ViewBag.Title = "标签搜索 - 个人博客";
                ViewBag.UserId = id;
                if (string.IsNullOrWhiteSpace(searchContent))
                {
                    return View("Blogs", new List<Blog>().ToPagedList(1, 10));
                }
                else
                {
                    searchContent = searchContent.Trim();
                    var pageNumber = page ?? 1;
                    ViewBag.SearchContent = searchContent;
                    if (user.UserConfig.PublicBlog)
                    {
                        var blogs = user.Blogs.Where(m => m.IsPublic && m.Tag.Contains(searchContent)).OrderByDescending(m => m.Id);
                        var onePageOfProducts = blogs.ToPagedList(pageNumber, 10);
                        return View("Blogs", onePageOfProducts);
                    }
                    else
                    {
                        var blogs = user.Blogs.Where(m => m.Groups.Count(n => (n.Accessible & Accessible.Outer) != 0) > 0).Where(m => m.Tag.Contains(searchContent)).OrderByDescending(m => m.Id);
                        var onePageOfProducts = blogs.ToPagedList(pageNumber, 10);
                        return View("Blogs", onePageOfProducts);
                    }
                }
            }
            else
            {
                return View("Error");
            }
        }

        #endregion

        #region 资源分享页面
        /// <summary>
        /// 资源分类区
        /// </summary>
        /// <returns></returns>
        public ActionResult ResourceColumn()
        {
            var groups = Entity.FileGroups.Where(m => (m.Accessible & Accessible.Outer) != 0);
            return View(groups);
        }

        /// <summary>
        /// 显示资源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ColumnResources(int id)
        {
            var fileGroup = Entity.FileGroups.Find(id);
            if (fileGroup != null && (fileGroup.Accessible & Accessible.Outer) != 0)
            {
                return View("Resources", fileGroup);
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
            var file = Entity.WebFiles.Find(id);
            if (file != null && file.Type == FileType.Document && file.Groups.Count(m => (m.Accessible & Accessible.Outer) != 0) > 0)
            {
                var path = Server.MapPath("~/WingStudio/Resource/") + file.FilePath;
                if (System.IO.File.Exists(path))
                {

                    file.LookCount++;
                    Entity.SaveChanges();

                    //.doc(x)、.txt、.xls(x)、.ppt(x)、.pdf

                    return File(path, "application/octet-stream", Url.Encode(file.Name));
                }
                else
                {
                    //file.Groups.Clear();
                    Entity.WebFiles.Remove(file);
                    Entity.SaveChanges();
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
            var file = Entity.WebFiles.Find(id);
            if (file != null && file.Groups.Count(m => (m.Accessible & Accessible.Outer) != 0) > 0)
            {
                var path = Server.MapPath("~/WingStudio/Resource/") + file.FilePath;
                if (System.IO.File.Exists(path))
                {
                    file.LoadCount++;
                    Entity.SaveChanges();
                    return File(path, "application/octet-stream", Url.Encode(file.Name));
                }
                else
                {
                    //file.Groups.Clear();
                    Entity.WebFiles.Remove(file);
                    Entity.SaveChanges();
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
            var file = Entity.WebFiles.Find(id);
            if (file != null && file.Type == FileType.Video && file.Groups.Count(m => (m.Accessible & Accessible.Outer) != 0) > 0)
            {
                var path = Server.MapPath("~/WingStudio/Resource/") + file.FilePath;
                if (System.IO.File.Exists(path))
                {
                    file.LookCount++;
                    Entity.SaveChanges();
                    ViewBag.FileName = file.Name;
                    ViewBag.TargetSource = file.FilePath;
                    return View("PlayVideo");
                }
                else
                {
                    //file.Groups.Clear();
                    Entity.WebFiles.Remove(file);
                    Entity.SaveChanges();
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
        public ActionResult SearchPublicResource(string searchContent, int? page)
        {
            if (string.IsNullOrWhiteSpace(searchContent))
            {
                return View("Resources", new List<WebFile>().ToPagedList(1, 10));
            }
            else
            {
                searchContent = searchContent.Trim();
                var files = Entity.WebFiles.Where(m => m.Groups.Count(g => (g.Accessible & Accessible.Outer) != 0) > 0).Where(m => m.Name.Contains(searchContent)).OrderByDescending(m => m.Id);
                var pageNumber = page ?? 1;
                ViewBag.PageNumber = pageNumber;
                var onePageOfProducts = files.ToPagedList(pageNumber, 10);
                ViewBag.SearchContent = searchContent;
                return View("Resources", onePageOfProducts);
            }
        }

        #endregion

        #region 博客日历

        /// <summary>
        /// 获取该月存在博客的情况
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMonthBlogsCase(int year, int month)
        {
            if (!(year > 2000 && year <= DateTime.Now.Year && month > 0 && month <= 12))
            {
                return Json(new { });
            }
            try
            {
                var days = DateTime.DaysInMonth(year, month);
                var list = new List<DayBlog>();
                var blogs = Entity.Blogs.Where(m => m.IsPublic && ((DateTime)m.PublicTime).Year == year && ((DateTime)m.PublicTime).Month == month);
                var controller = RouteData.Values["controller"].ToString();
                if (!controller.Equals("user", StringComparison.InvariantCultureIgnoreCase))
                {
                    blogs = blogs.Where(m => m.Owner.UserConfig.PublicBlog || (m.Checked && m.Groups.Count(n => (n.Accessible & Accessible.Outer) != 0) > 0));
                }
                for (var i = 1; i <= days; i++)
                {
                    list.Add(new DayBlog { Count = blogs.Count(m => ((DateTime)m.PublicTime).Day == i) });
                }

                return Json((new JavaScriptSerializer()).Serialize(list));
            }
            catch
            {
                return Json(new { });
            }

        }

        /// <summary>
        /// 获取每日博客
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult OneDayBlogs(int year, int month, int day, int? page)
        {
            if (!(year > 2000 && year <= DateTime.Now.Year && month > 0 && month <= 12 && day > 0 && day <= 31))
            {
                return View("Error");
            }
            try
            {
                var blogs = Entity.Blogs.Where(m => m.IsPublic && ((DateTime)m.PublicTime).Year == year && ((DateTime)m.PublicTime).Month == month && ((DateTime)m.PublicTime).Day == day);

                var controller = RouteData.Values["controller"].ToString();
                if (!controller.Equals("user", StringComparison.InvariantCultureIgnoreCase))
                {
                    blogs = blogs.Where(m => m.Owner.UserConfig.PublicBlog || (m.Checked && m.Groups.Count(n => (n.Accessible & Accessible.Outer) != 0) > 0));
                }
                else
                {
                    ViewBag.Logined = Entity.Users.Find(Convert.ToInt32(User.Identity.Name));
                }

                ViewBag.Title = $"{year}年{month}月{day}日 - 博客档案";
                ViewBag.Year = year;
                ViewBag.Month = month;
                ViewBag.Day = day;
                var pageNumber = page ?? 1;
                var onePageOfProducts = blogs.OrderByDescending(m => m.Id).ToPagedList(pageNumber, 10);
                return View("Blogs", onePageOfProducts);
            }
            catch
            {
                return View("Error");
            }
        }
        
        #endregion

        #region 报名页面
        /// <summary>
        /// 显示报名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ShowApplication(int id)
        {
            var app = Entity.Applications.Find(id);
            if (app != null && ((app.StartTime < DateTime.Now) || (Request.IsAuthenticated && User.IsInRole("Admin"))))
            {
                if(app.StartTime > DateTime.Now)
                {
                    ViewBag.AppState = -1;
                }
                else if (app.EndTime < DateTime.Now)
                {
                    ViewBag.AppState = 1;
                }
                else
                {
                    ViewBag.AppState = 0;
                }
                return View(app);
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 检查是否重复报名
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sno"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RepeatedApply(int id, string sno)
        {
            var app = Entity.Applications.Find(id);
            if(app != null)
            {
                if(app.Participants.Count(m => m.StudentNo == sno) > 0)
                {
                    return Json(new { Repeated = true, Id = id });
                }
                else
                {
                    return Json(new { Repeated = false, Id = id });
                }
            }
            else
            {
                return Json(new { Repeated = false, Id = id });
            }
        }

        /// <summary>
        /// 申请报名
        /// </summary>
        /// <param name="id"></param>
        /// <param name="part"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Apply(int id, Participant part, string code)
        {
            if (string.IsNullOrWhiteSpace(code) || Session["ApplyCode"].ToString() == "" || Session["ApplyCode"].ToString().ToUpper() != code.ToUpper())
            {
                return Json(new { title = "申请失败", message = "验证码错误!" });
            }
            var app = Entity.Applications.Find(id);
            if (app != null && DateTime.Compare(app.StartTime, DateTime.Now) <= 0)
            {
                if (DateTime.Compare(app.EndTime, DateTime.Now) >= 0)
                {
                    if(WebSecurity.IsValidParticipant(part, app.IsFormal))
                    {
                        var newPart = new Participant
                        {
                            Name = part.Name.Trim(),
                            StudentNo = part.StudentNo,
                            StudentClass = part.StudentClass.Trim(),
                            Email = part.Email
                        };
                        if(app.IsFormal)
                        {
                            newPart.Phone = part.Phone;
                            newPart.Sex = (part.Sex == "M") ? "M" : "W";
                        }
                        app.Participants.Add(newPart);
                        Entity.SaveChanges();
                        return Json(new { title = "申请成功", message = "成功报名!", count = app.Participants.Count });
                    }
                    else
                    {
                        return Json(new { title = "申请失败", message = "提交信息不合法!" });
                    }
                    
                }
                else
                {
                    return Json(new { title = "申请失败", message = "报名已经截止!" });
                }
            }
            else
            {
                return Json(new { title = "申请失败", message = "提交信息不合法!" });
            }
        }

        #endregion

        #region 服务页面
        /// <summary>
        /// 服务
        /// </summary>
        /// <returns></returns>
        public ActionResult Service()
        {
            ViewBag.Page = "Service";
            return View("About");
        }

        /// <summary>
        /// 在线工具
        /// </summary>
        /// <returns></returns>
        public ActionResult OnlineTools()
        {
            return View();
        }

        #endregion

    }
}