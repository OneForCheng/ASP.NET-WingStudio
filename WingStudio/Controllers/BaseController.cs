using log4net;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WingStudio.Models;

namespace WingStudio.Controllers
{

    public class BaseController : Controller
    {
        protected WebAppContext Entity = new WebAppContext();
        protected ILog InfoLog  = LogManager.GetLogger("InfoLogger");

        /// <summary>
        /// 获取该月存在博客的情况
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMonthBlogsCase(int year, int month)
        {
            if(!(year > 2000 && year <= DateTime.Now.Year && month > 0 && month <= 12))
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
                    list.Add(new DayBlog {  Count = blogs.Count(m => ((DateTime)m.PublicTime).Day == i) });
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
    }
}