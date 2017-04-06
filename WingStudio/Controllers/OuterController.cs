using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WingStudio.Controllers
{
    [Authorize(Roles = "Outer")]
    public class OuterController : BaseController
    {
        // GET: Outer
        public ActionResult Index()
        {
            return View();
        }
    }
}