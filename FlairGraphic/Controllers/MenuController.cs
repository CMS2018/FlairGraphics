using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlairGraphic.Base.Models;
using FlairGraphic.Models;

namespace FlairGraphic.Controllers
{
    public class MenuController : BaseController
    {
        private Result result;
        private MenuUtil menuUtil;
        public MenuController()
        {
            result = new Result();
            menuUtil = new MenuUtil();
        }
        // GET: /Menu/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListMenu()
        {
            result = menuUtil.ListMenu("");
            return new JsonResult()
            {
                Data = result.Object,
                MaxJsonLength = Int32.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
        public ActionResult CreateEdit(Int32 Id = 0)
        {
            menu menu = Id > 0 ? db.menus.Find(Id) : new menu();
            ViewBag.Title = menu == null ? "Menu Create" : "Menu Edit";
            ViewBag.controller_name = new SelectList(menuUtil.GetController(), "Value", "Text", menu != null ? menu.controller_name : "");
            ViewBag.menu_parent_id = new SelectList(menuUtil.GetMenu(true), "Value", "Text", menu != null ? menu.menu_parent_id : 0);
            ViewBag.action_name = new SelectList(STUtil.GetListAllActionByController(menu != null ? menu.controller_name : ""), "Value", "Text", menu != null ? menu.action_name : "");
            ViewBag.SelectedMenuAction = menu != null ? menu.action_name : "";
            return View(menu);
        }
        [HttpPost]
        public ActionResult CreateEdit(menu menu)
        {
            result = menuUtil.PostMenuCreate(menu);
            ViewBag.Title = menu == null ? "Menu Create" : "Menu Edit";
            ViewBag.controller_name = menuUtil.GetController();
            ViewBag.menu_parent_id = menuUtil.GetMenu(true);
            ViewBag.menu_ddl_id = menuUtil.GetMenu(true);
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            switch (result.MessageType)
            {
                case MessageType.Success:
                    return RedirectToAction("Index", "Menu", new { result = result.Message, MessageType = result.MessageType });
                default:
                    return RedirectToAction("CreateEdit", "Menu", new { result = result.Message, MessageType = result.MessageType });
            }
            return View(menu);
        }
        public ActionResult GetActionNameByController(string id)
        {
            string controllerName = string.Format("{0}Controller", id);
            var list = STUtil.GetListAllActionByController(controllerName);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MenuRoleAction(int id)
        {
            ViewBag.Title = "Role Action";
            int menuID = id;
            int company_id = SessionUtil.GetCompanyID();
            IList<ControllerAction> list = new List<ControllerAction>();
            var listController = STUtil.GetListController(); //Nedd to check data in respwct of
            ViewBag.action_name = new List<SelectListItem>();
            ViewBag.menu_id = menuID;
            foreach (var c in listController)
            {
                string controllerName = c.Value;
                var listAllAction = STUtil.GetListAllActionByController(controllerName);
                var listAssignedAction = (from r in db.menu_access_controller_action
                                          where r.menu_id==menuID && r.controller_name == controllerName
                                          orderby r.action_name
                                          select new SelectListItem
                                          {
                                              Value = r.action_name,
                                              Text = r.action_name,
                                          }).ToList();

                foreach (var aa in listAllAction)
                {
                    ControllerAction ca = new ControllerAction();
                    ca.ControllerName = controllerName;
                    ca.ActionName = aa.Text;
                    ca.IsAssigned = listAssignedAction.Where(a => a.Text == aa.Text).Count() > 0 ? true : false;
                    list.Add(ca);
                }
            }
            return PartialView("_MenuRoleAction", list);
        }
        [HttpPost]
        public ActionResult PostAssignViewMenuBise(string id, int menuId)
        {
            string[] str = id.Split(','); //Chk_CityController_Create_False,
            for (int i = 0; i < str.Length; i++)
            {
                string[] info = str[i].Split('_');
                string controllerName = info[1];
                string actionName = info[2];
                bool isAssigned = Convert.ToBoolean(info[3]);
                menu_access_controller_action menuAccess = db.menu_access_controller_action.AsEnumerable().Where(x => x.controller_name == controllerName && x.action_name == actionName && x.menu_id == menuId && x.is_active).SingleOrDefault();
                if (menuAccess == null) // new
                {
                    menuAccess = new menu_access_controller_action();
                    menuAccess.menu_id = menuId;
                    menuAccess.controller_name = controllerName;
                    menuAccess.action_name = actionName;
                    db.menu_access_controller_action.Add(menuAccess);
                }
                else
                {
                    db.menu_access_controller_action.Remove(menuAccess);
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}