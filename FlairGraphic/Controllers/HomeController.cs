using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlairGraphic.Base.Models;

namespace FlairGraphic.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult AccessDenied()
        {

            ViewBag.Title = "Access Denied";

            return View();
        }

        public ActionResult General(int id)
        {
            ViewBag.Title = STUtil.GetWebConfigValue("APPLICATION_NAME") + " Error";
            ViewBag.SubTitle = "Error";
            return View(db.app_error_log.Find(id));
        }
    }
}