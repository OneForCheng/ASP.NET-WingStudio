using log4net;
using System.Web.Mvc;
using WingStudio.Models;

namespace WingStudio.Controllers
{
    public class BaseController : Controller
    {
        protected WebAppContext Entity = new WebAppContext();
        protected ILog InfoLog  = LogManager.GetLogger("InfoLogger");
    }
}