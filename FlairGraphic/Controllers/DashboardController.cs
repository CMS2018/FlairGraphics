using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlairGraphic.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            ViewBag.Title = "Dashboard";
            return View();
        }

        // GET: Dashboard/Details/5
        
    }
}
