using ForCheng.Security.AES;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WingStudio.Models;

namespace WingStudio.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : BaseController
    {

        private SuperUser Loginer => Entity.SuperUsers.Find(Convert.ToInt32(User.Identity.Name));

        #region 修改个人信息
        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangeInfo()
        {
            return View("SaveInfo", Loginer);
        }

        /// <summary>
        /// 修改用户名
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChangeAccount(string account)
        {
            if(WebSecurity.IsValidAccount(account))
            {
                if(Entity.Users.Count(m => m.Account == account) > 0 || Entity.SuperUsers.Count(m => m.Account == account) > 0)
                {
                    return Json(new { title = "修改失败", message = "该用户账号已存在!" });
                }
                else
                {
                    var logined = Loginer;
                    logined.Account = account;
                    Entity.SaveChanges();
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
            if(WebSecurity.IsValidPassword(oldPassword) && WebSecurity.IsValidPassword(newPassword))
            {
                var logined = Loginer;
                if(AES.Encrypt(oldPassword) == logined.Password)
                {
                    logined.Password = AES.Encrypt(newPassword);
                    Entity.SaveChanges();
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
            if(WebSecurity.IsValidSecQusetion(qusetion, answer))
            {
                var flag = (SecurityFlag)qusetion;
                var logined = Loginer;
                logined.SecQuestion = flag;
                if(flag != SecurityFlag.None)
                {
                    logined.SecAnswer = AES.Encrypt(answer.Trim());
                }
                Entity.SaveChanges();
                return Json(new { title = "修改成功", message = "成功修改安全提问!" });
            }
            else
            {
                return Json(new { title = "修改失败", message = "提交信息不合法!" });
            }
        }

        #endregion

        #region 管理用户组
        /// <summary>
        /// 管理组
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ManageUserGroup(int? page)
        {
            var groups = Entity.UserGroups.OrderByDescending(m => m.Id);
            var pageNumber = page ?? 1;
            ViewBag.PageNumber = pageNumber;
            var onePageOfProducts = groups.ToPagedList(pageNumber, 10);
            return View("ManageUserGroup", onePageOfProducts);
        }

        /// <summary>
        /// 添加组
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddUserGroup()
        {
            ViewBag.IsModified = false;
            return View("SaveUserGroup");
        }

        /// <summary>
        /// 添加组
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUserGroup(UserGroup group)
        {
            if(WebSecurity.IsValidUserGroup(group))
            {
                var newGroup = new UserGroup
                {
                    Theme = @group.Theme,
                    Authority = @group.Authority
                };
                Entity.UserGroups.Add(newGroup);
                Entity.SaveChanges();
                return Content(WebHelper.SweetAlert("添加成功", "成功添加用户组!", "location.href='/SuperAdmin/ManageUserGroup'"));
            }
            else
            {
                return Content(WebHelper.SweetAlert("添加失败", "提交信息不合法!"));
            }
        }

        /// <summary>
        /// 修改指定组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ModUserGroup(int id)
        {
            var group = Entity.UserGroups.Find(id);
            if (group != null)
            {
                ViewBag.IsModified = true;
                return View("SaveUserGroup", group);
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 修改指定组
        /// </summary>
        /// <param name="id"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModUserGroup(int id, UserGroup group)
        {
            var target = Entity.UserGroups.Find(id);
            if (target != null)
            {
                if(WebSecurity.IsValidUserGroup(group))
                {
                    target.Theme = group.Theme;
                    target.Authority = group.Authority;
                    Entity.SaveChanges();
                    return Content(WebHelper.SweetAlert("修改成功", "成功修改指定用户组!", "location.href='/SuperAdmin/ManageUserGroup'"));
                }
                else
                {
                    return Content(WebHelper.SweetAlert("修改失败", "提价信息不合法!"));
                }
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 删除指定组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DelUserGroup(int id)
        {
            var group = Entity.UserGroups.Find(id);
            if (group != null)
            {
                //group.InnerUsers.Clear();
                Entity.UserGroups.Remove(group);
                Entity.SaveChanges();
                return Content(WebHelper.SweetAlert("删除成功", "成功删除指定用户组!", "location.href='/SuperAdmin/ManageUserGroup'"));
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 显示用户组
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ShowGroupUsers(int id, int? page)
        {
            var group = Entity.UserGroups.Find(id);
            if (group != null)
            {
                ViewBag.GroupId = id;
                ViewBag.GroupName = group.Theme;
                var admins = group.Users;
                var pageNumber = page ?? 1;
                ViewBag.PageNumber = pageNumber;
                var onePageOfProducts = admins.ToPagedList(pageNumber, 10);
                return View("ShowGroupUsers", onePageOfProducts);
            }
            else
            {
                return View("Error");
            }

        }

        /// <summary>
        /// 添加管理员到指定组
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUserToGroup(int id, int groupId)
        {
            var admin = Entity.Users.Find(id);
            var group = Entity.UserGroups.Find(groupId);
            if (admin != null && group != null)
            {
                group.Users.Add(admin);
                Entity.SaveChanges();
                return Json(new {title = "添加成功", message = "成功添加指定用户到指定组!" });
            }
            else
            {
                return Json(new { title = "添加失败", message = "提交信息不合法!" });
            }
        }

        /// <summary>
        /// 获取指定用户组之外的用户基本信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetBaseUserInfos(int id)
        {
            var group = Entity.UserGroups.Find(id);
            if(group != null)
            {
                var userInfos = Entity.Users.ToList().Except(group.Users).Select(m => new {m.Id, m.Account, m.Name }).OrderByDescending(m => m.Id);
                if(userInfos.Any())
                {
                    return Json((new JavaScriptSerializer()).Serialize(userInfos));
                }
                else
                {
                    return Json(new { });
                }
            }
            else
            {
                return Json(new { });
            }
        }

        /// <summary>
        /// 从指定组中移除指定管理员
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public ActionResult DelUserFromGroup(int id, int groupId)
        {
            var admin = Entity.Users.Find(id);
            var group = Entity.UserGroups.Find(groupId);
            if (admin != null && group != null && group.Users.Count(m => m.Id == id) > 0)
            {
                group.Users.Remove(admin);
                Entity.SaveChanges();
                return Content(WebHelper.SweetAlert("删除成功", "成功从指定组中删除指定用户!", "location.href='/SuperAdmin/ShowGroupUsers/" + groupId + "'"));
            }
            else
            {
                return View("Error");
            }
        }

        #endregion

        #region 用户管理
        /// <summary>
        /// 管理用户
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ManageUser(int? page)
        {
            ViewBag.IsSearch = false;
            var users = Entity.Users.OrderByDescending(m => m.Id);
            var pageNumber = page ?? 1;
            ViewBag.PageNumber = pageNumber;
            var onePageOfProducts = users.ToPagedList(pageNumber, 10);
            return View("ManageUser", onePageOfProducts);
        }

        /// <summary>
        /// 获取当前登录用户的个人信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserDetailInfo(int id)
        {
            var user = Entity.Users.Find(id);
            if(user != null)
            {
                var userInfo = user.UserInfo;
                var birthday = "";
                if (userInfo.Birthday != null)
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
                    userInfo.Introduction,
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


        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddUser()
        {
            return View("SaveUser");
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUser(User user)
        {
            if (WebSecurity.IsValidUser(user))
            {
                if (Entity.Users.Count(m => m.Account == user.Account) > 0 || Entity.SuperUsers.Count(m => m.Account == user.Account) > 0)
                {
                    return Content(WebHelper.SweetAlert("添加失败", "此账号已被注册!"));
                }
                var newUser = new User
                {
                    Account = user.Account,
                    Password = AES.Encrypt(user.Password),
                    Email = user.Email,
                    Name = user.Name,
                    Grade = user.Grade,
                    Sex = (user.Sex == "M") ? "M" : "W",
                    UserInfo = new UserInfo(),
                    UserConfig = new UserConfig(),
                    Favorites = new Favorites(),
                    Recommendations = new Recommendations()
                };

                Entity.Users.Add(newUser);
                Entity.SaveChanges();

                //添加初始文件资源配置
                var dic = new Dictionary<FileType, string> {
                    { FileType.Picture,"图片" },
                    { FileType.Video,"视频" },
                    { FileType.Music,"音乐" },
                    { FileType.Application,"应用" },
                    { FileType.Document,"文档" },
                    { FileType.Other,"其它" }
                };
                foreach (var item in dic)
                {
                    var folder = new WebFolder
                    {
                        Owner = newUser,
                        Type = item.Key,
                        Name = item.Value
                    };
                    Entity.WebFolders.Add(folder);
                }
                Entity.SaveChanges();
                var logined = Loginer;
                InfoLog.Info($"Envent:[添加用户事件] AddUser[Name:{newUser.Name} Account:{newUser.Account}] Operator[Role:SuperAdmin Account:{logined.Account} Id:{logined.Id}]");
                return Content(WebHelper.SweetAlert("添加成功", "成功添加用户!"));
            }
            else
            {
                return Content(WebHelper.SweetAlert("添加失败", "提交信息不合法!"));
            }
        }

        /// <summary>
        /// 删除指定用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DelUser(int id)
        {
            //此功能应该在后期去掉，只用于测试

            var user = Entity.Users.Find(id);
            if (user != null)
            {
                //user.Groups.Clear();
                Entity.UserInfos.Remove(user.UserInfo);
                Entity.UserConfigs.Remove(user.UserConfig);
                Entity.Favorites.Remove(user.Favorites);
                if(user.Recommendations != null)
                {
                    Entity.Recommendations.Remove(user.Recommendations);
                }
               
                Entity.Users.Remove(user);

                var msgs = Entity.Messages.Where(m => m.OwnId == id);
                if(msgs.Any())
                {
                    foreach (var msg in msgs)
                    {
                        Entity.Packets.RemoveRange(msg.Packets);
                    }
                    Entity.Messages.RemoveRange(msgs);
                }

                var files = Entity.WebFiles.Where(m => m.Owner.Id == id);
                if (files.Any())
                {
                    //foreach (var file in files)
                    //{
                    //    file.Groups.Clear();
                    //}
                    Entity.WebFiles.RemoveRange(files);
                }
                
                var folders = Entity.WebFolders.Where(m => m.Owner.Id == id);
                if (folders.Any())
                {
                    foreach (var folder in folders)
                    {
                        folder.SubFolders.Clear();
                    }
                    Entity.WebFolders.RemoveRange(folders);
                }

                var blogs = Entity.Blogs.Where(m => m.Owner.Id == id);
                if (blogs.Any())
                {
                    //foreach(var blog in blogs)
                    //{
                    //    blog.Groups.Clear();
                    //}
                    Entity.Blogs.RemoveRange(blogs);
                }
                Entity.SaveChanges();
                var logined = Loginer;
                InfoLog.Info($"Envent:[删除用户事件] DelUser[Name:{user.Name} Account:{user.Account}] Operator[Role:SuperAdmin Account:{logined.Account} Id:{logined.Id}]");
                return Content(WebHelper.SweetAlert("删除成功", "成功删除用户!"));
            }
            else
            {
                return View("Error");
            }

        }

        /// <summary>
        /// 禁用指定的用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ForbiddenUser(int id)
        {
            var user = Entity.Users.Find(id);
            if (user != null)
            {
                if (user.IsForbidden)
                {
                    user.IsForbidden = false;
                    Entity.SaveChanges();
                    return Content(WebHelper.SweetAlert("操作成功", "已成功解除该用户的禁用!"));
                }
                else
                {
                    user.IsForbidden = true;
                    Entity.SaveChanges();
                    return Content(WebHelper.SweetAlert("操作成功", "已成功禁用该用户!"));
                }
            }
            else
            {
                return View("Error");
            }
        }

        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="searchContent">搜索内容</param>
        /// <param name="page">分页</param>
        /// <returns></returns>
        public ActionResult SearchUser(string searchContent, int? page)
        {
            ViewBag.IsSearch = true;
            if (string.IsNullOrWhiteSpace(searchContent))
            {
                return View("ManageUser", new List<User>().ToPagedList(1,10));
            }
            else
            {
                searchContent = searchContent.Trim();
                var users = Entity.Users.Where(m => m.Name.Contains(searchContent) || m.Account.Contains(searchContent)).OrderByDescending(m => m.Id);
                var pageNumber = page ?? 1;
                ViewBag.PageNumber = pageNumber;
                var onePageOfProducts = users.ToPagedList(pageNumber, 10);
                ViewBag.SearchContent = searchContent;
                return View("ManageUser", onePageOfProducts);
            }
        }
        #endregion
    }
}