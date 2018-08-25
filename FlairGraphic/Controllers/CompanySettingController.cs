using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FlairGraphic.Base.Models;
using FlairGraphic.Models;
using System.IO;
using System.Threading;
using System.Transactions;

namespace FlairGraphic.Controllers
{
    public class CompanySettingController : BaseController
    {
        private Result result;
        private CountryUtil countryUtil;
        private CompanyUtil companyUtil;
        private UserUtil userUtil;
        private StateUtil stateUtil;
        private PackageUtil packageUtil;
        int CompanyId = 0;

        public CompanySettingController()
        {
            CompanyId = SessionUtil.GetCompanyID();
            result = new Result();
            companyUtil = new CompanyUtil();
            stateUtil = new StateUtil();
            countryUtil = new CountryUtil();
            packageUtil = new PackageUtil();
        }
        // GET: /CompanySetting/
        public ActionResult EditCompany(string id = "")
        {

            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Company Setting");
            int company_id = (Int32)SessionUtil.GetCompanyID();
            company company = db.companies.Find(company_id);
            ViewBag.Title = "Company Edit";
            ViewBag.id = company != null ? company.company_id : 0;
            return View();
        }

        [HttpPost]
        public ActionResult AddCompany(company company, HttpPostedFileBase company_logo, FormCollection frmUser)
        {
            try
            {
                string AWSProfileName = STUtil.GetWebConfigValue("AWSProfileName");
                string userName = frmUser["user_name"];
                string loginID = frmUser["login_id"];
                string emailID = frmUser["email_id"];
                string mobile = frmUser["mobile"];
                string password = frmUser["password"];
                string gender = frmUser["gender"];
                STUtil.SetSessionValue(UserInfo.IsCompanyAddUpdate.ToString(), "1");
                result = companyUtil.PostCompanyCreateEdit(company, company_logo, userName, loginID, emailID, mobile, gender, password);
                UploadFile(SessionUtil.GetCompanyFolderName().ToString(), company_logo);
                STUtil.SetSessionValue(UserInfo.IsCompanySetup.ToString(), "1");

            }
            catch (Exception ex)
            {
                STUtil.SetSessionValue(UserInfo.IsCompanyAddUpdate.ToString(), "1");
                result.Message = ex.Message;
            }
            return RedirectToAction("EditCompany", "CompanySetting", new { Result = result.Message, MessageType = result.MessageType });
        }

        public ActionResult UserEdit(string id)
        {
            var userList = db.users.AsEnumerable().Where(u => u.user_id == Convert.ToInt32(id)).ToList();
            int company_id = userList.FirstOrDefault().company_id;
            company company = new company();
            company = company_id > 0 ? db.companies.Find(company_id) : company;
            string SNAG_ImagePath = STUtil.GetWebConfigValue("ImagePath");
            String companyFolderName = company.company_folder_name != null ? company.company_folder_name.ToString() : "";
            var UserPhoto = userList.FirstOrDefault().user_photo != null ? SNAG_ImagePath + companyFolderName + userList.FirstOrDefault().user_photo : SNAG_ImagePath + userList.FirstOrDefault().user_photo;
            var data = (from u in userList
                        select new
                        {
                            user_id = u.user_id,
                            user_name = u.user_name,
                            login_id = u.login_id,
                            email_id = u.email_id,
                            mobile = u.mobile,
                            gender = u.gender,
                            parent_id = u.role_bit,
                            role_bit = u.role_bit,
                            is_active = u.is_active,
                            UserPhoto = UserPhoto,
                            hdnUserPhoto = u.user_photo
                        }).FirstOrDefault();

            return Json(data);
        }

        #region Company Tabs
        public ActionResult LoadCompanyDetail(int id)
        {
            int company_id = Convert.ToInt32(id);
            company company = new company();
            company = company_id > 0 ? db.companies.Find(company_id) : company;
            if (company != null)
            {
                ViewBag.country_id = new SelectList(countryUtil.GetCountrySelectList(), "Value", "Text", company.state_id != null ? company.state.country_id : 0);
                ViewBag.StateName = company.state != null ? stateUtil.GetStateSelectList(company.state.country_id) : stateUtil.GetStateName();
                ViewBag.TimeZone = STUtil.GetTimeZoneInfo();
            }
            return PartialView("_CompanyDetail", company);
        }


        public ActionResult LoadRoles(int id)
        {
            ViewBag.Id = id;
            //ViewBag.parent_id = RoleUtil.GetParentRoleByCompany(id);
            return PartialView("_CompanyRoles");
        }
        public ActionResult RoleEdit(string id)
        {
            int role_id = String.IsNullOrEmpty(id) ? 0 : Convert.ToInt32(id);
            var data = (from u in db.roles.AsEnumerable()
                        where u.role_id == role_id
                        select new
                        {
                            role_id = u.role_id,
                            role_name = u.role_name,
                            role_bit = u.role_bit,
                            is_active = u.is_active,
                        }).FirstOrDefault();

            return Json(data);
        }
        public ActionResult LoadSubscription(int id)
        {
            ViewBag.packages = db.package_subscription.AsEnumerable().ToList().Where(x => x.company_id == id).ToList();
            ViewBag.company_id = id;
            return PartialView("_CompanySubscription");
        }
        public ActionResult LoadAssignView(int id)
        {
            ViewBag.Title = "Assign View";
            int company_id = Convert.ToInt32(id);
            ViewBag.CompanyId = company_id;
            IList<AssignController> list = new List<AssignController>();
            var listController = STUtil.GetListController(); //Nedd to check data in respwct of
            var listRoles = db.roles.AsEnumerable().Where(x => x.company_id == company_id).ToList();
            foreach (var rr in listRoles)
            {
                foreach (var c in listController)
                {
                    string controllerName = c.Value;

                    if (controllerName.ToUpper() != "CompanyController".ToUpper()
                        && controllerName.ToUpper() != "EmailController".ToUpper()
                        && controllerName.ToUpper() != "JsTree3Controller".ToUpper()
                        && controllerName.ToUpper() != "ListingXmlController".ToUpper()
                        && controllerName.ToUpper() != "MastersController".ToUpper()
                        && controllerName.ToUpper() != "MenuController".ToUpper()
                        && controllerName.ToUpper() != "RoleController".ToUpper()
                        )
                    {
                        // var listAllAction = STUtil.GetListAllActionByController(controllerName);
                        var listAssignedAction = STUtil.GetListActionAssignedByRoleAndController(rr.role_id, controllerName);
                        AssignController ac = new AssignController();
                        ac.ControllerName = controllerName;
                        ac.RoleName = rr.role_name;
                        ac.RoleId = rr.role_id;
                        ac.IsAssigned = db.role_action.AsEnumerable().Where(a => a.controller_name == controllerName && a.role_id == rr.role_id).Count() > 0 ? true : false;
                        list.Add(ac);
                    }
                }
            }
            return PartialView("_CompanyAssignView", list);
        }

        public ActionResult ListRoles(string id = "")
        {
            result = RoleUtil.GetRoleList(id);
            return new JsonResult()
            {
                Data = result.Object,
                MaxJsonLength = Int32.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpPost]
        public ActionResult CreateEditUser(user user, HttpPostedFileBase user_photo, FormCollection frmAdminUser)
        {
            try
            {
                var userdata = db.users.Find(user.user_id);
                company CompanyInformation = db.companies.Find(user.company_id);
                if (user_photo != null)
                {
                    string AWSProfileName = STUtil.GetWebConfigValue("AWSProfileName");
                    string GenFileName = STUtil.GetTodayDate().ToString("yyyyMMdd") + "_" + CompanyInformation.company_id.ToString() + "_" + Path.GetFileName(user_photo.FileName).Replace(" ", "_");
                    String companyFolderName = CompanyInformation.company_folder_name.ToString().Replace("/", "");
                   /// AWSUtil.UploadFile(user_photo.InputStream, AWSProfileName, companyFolderName, GenFileName);
                    user.user_photo = GenFileName;
                }
                else if (userdata != null)
                {
                    user.user_photo = userdata.user_photo;
                }
                user.parent_user_id = SessionUtil.GetUserID();
                result = companyUtil.PostUserEdit(user);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.MessageType = MessageType.Error;
            }
            return RedirectToAction("EditCompany", "CompanySetting", new { id = user.company_id, Result = result.Message, MessageType = result.MessageType });
        }

        [HttpPost]
        public ActionResult CreateEditAdmin(user user, HttpPostedFileBase user_photo, FormCollection frmAdminUser)
        {
            try
            {
                var userid = user.user_id;
                user UserInformation = db.users.Find(userid);
                company CompanyInformation = db.companies.Find(UserInformation.company_id);
                if (user_photo != null)
                {
                    string AWSProfileName = STUtil.GetWebConfigValue("AWSProfileName");
                    string GenFileName = STUtil.GetTodayDate().ToString("yyyyMMdd") + "_" + CompanyInformation.company_id.ToString() + "_" + Path.GetFileName(user_photo.FileName).Replace(" ", "_");
                    String companyFolderName = CompanyInformation.company_folder_name.ToString().Replace("/", "");
                   // AWSUtil.UploadFile(user_photo.InputStream, AWSProfileName, companyFolderName, GenFileName);
                    UserInformation.user_photo = GenFileName;
                }
                UserInformation.user_name = user.user_name;
                UserInformation.mobile = user.mobile;
                UserInformation.gender = user.gender;
                result = companyUtil.PostUserEdit(UserInformation);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return RedirectToAction("EditCompany", "CompanySetting", new { id = user.company_id, Result = result.Message, MessageType = result.MessageType });
        }

        #region Company RoleMenu
        public ActionResult CompanyRoleMenu(int roleID, string companyId = "")
        {
            ViewBag.Title = "Menu Tree List";
            ViewBag.role_id = roleID;
            ViewBag.CompanyId = companyId;
            return PartialView("_CompanyRoleMenu");
        }
        public ActionResult SaveCompanyRoleMenu(string id, string ids = "")
        {

            string[] menuIds = id.Split(',');
            string[] info = ids.Split(',');
            int roleID = Convert.ToInt32(info[0]);
           // int CompanyId = Convert.ToInt32(info[1]);

            //var listToRemove = db.role_menu.AsEnumerable().Where(r => r.role_id == roleID && r.company_id == companyID);

            //db.role_menu.RemoveRange(listToRemove);
            //db.SaveChanges();
            db.USP_AddRoleMenuAndAction(roleID, CompanyId, 0, true);

            for (int i = 0; i < menuIds.Length; i++)
            {
                 int menuId = Convert.ToInt32(menuIds[i]);
                //role_menu rm = new role_menu();
                //rm.role_id = roleID;
                //rm.menu_id = menuId;
                //rm.is_active = true;
                //rm.company_id = CompanyId;
                //db.role_menu.Add(rm);
                //STUtil.SetSessionValue(UserInfo.IsCompanyAddUpdate.ToString(), "0");
                //db.SaveChanges();
               // STUtil.SetSessionValue(UserInfo.IsCompanyAddUpdate.ToString(), "1");
                db.USP_AddRoleMenuAndAction(roleID, CompanyId, menuId, false);

            }
            ViewBag.role_id = id;
            return Json(id);
            //return View();
        }
        public ActionResult GetCompanyMenuByRoleId(string ids = "")
        {
            string[] info = ids.Split(',');
            int roleId = Convert.ToInt32(info[0]);
            long roleBit = roleId > 0 ? db.roles.Find(roleId).role_bit : 0;
            var menuData = db.menus.AsEnumerable();
            if (roleBit == Convert.ToInt32(Role.SuperAdmin))
            {
                menuData = menuData.AsEnumerable().Where(x => x.is_for_super_admin && x.is_active);
            }
            else if (roleBit == Convert.ToInt32(Role.Admin))
            {
                menuData = menuData.AsEnumerable().Where(x => x.is_for_admin && x.is_active);
            }
            else
            {
                menuData = menuData.AsEnumerable().Where(x => x.is_for_normal_user && x.is_active);
            }
            int companyID = Convert.ToInt32(info[1]);
            var list = (from me in menuData.AsEnumerable()
                        join rm in db.role_menu.AsEnumerable() on me.menu_id equals rm.menu_id into rmTemp
                        from rmLj in rmTemp.Where(f => f.role_id == roleId && f.company_id == companyID).DefaultIfEmpty()
                        orderby me.sequence_order
                        select new
                        {
                            id = me.menu_id,
                            parent = (me.menu_parent_id != null ? Convert.ToString(me.menu_parent_id) : "#"),
                            text = me.menu_name,
                            selected = rmLj == null ? false : true,
                        }).ToList();
            return Json(list);
        }
        #endregion

        #region Company RoleAction
        public ActionResult CompanyRoleAction(int id, string companyId = "")
        {
            ViewBag.Title = "Role Action";
            int roleID = id;
            int company_id = Convert.ToInt32(companyId);
            IList<ControllerAction> list = new List<ControllerAction>();
            var listController = STUtil.GetListController(); //Nedd to check data in respwct of
            ViewBag.action_name = new List<SelectListItem>();
            ViewBag.role_id = id;
            ViewBag.RoleName = db.roles.Where(r => r.role_id == id && r.company_id == company_id).FirstOrDefault().role_name;

            foreach (var c in listController)
            {
                string controllerName = c.Value;
                var listAllAction = STUtil.GetListAllActionByController(controllerName);
                var listAssignedAction = STUtil.GetListActionAssignedByRoleAndController(roleID, controllerName);
                foreach (var aa in listAllAction)
                {
                    ControllerAction ca = new ControllerAction();
                    ca.ControllerName = controllerName;
                    ca.ActionName = aa.Text;
                    ca.IsAssigned = listAssignedAction.Where(a => a.Text == aa.Text).Count() > 0 ? true : false;
                    list.Add(ca);
                }
            }
            return PartialView("_CompanyRoleAction", list);
        }
        [HttpPost]
        public ActionResult PostCompanyRoleAction(string id, int roleID)
        {
            string[] str = id.Split(','); //Chk_CityController_Create_False,
            for (int i = 0; i < str.Length; i++)
            {
                string[] info = str[i].Split('_');
                string controllerName = info[1];
                string actionName = info[2];
                bool isAssigned = Convert.ToBoolean(info[3]);
                role_action roleAction = db.role_action.Where(ra => ra.controller_name == controllerName && ra.action_name == actionName && ra.role_id == roleID).SingleOrDefault();
                if (roleAction == null) // new
                {
                    roleAction = new role_action();
                    roleAction.role_id = roleID;
                    roleAction.controller_name = controllerName;
                    roleAction.action_name = actionName;
                    db.role_action.Add(roleAction);
                }
                else
                {
                    db.role_action.Remove(roleAction);
                }
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        #endregion

        [HttpPost]
        public ActionResult UpdatePackage(int packageid, company company)
        {
            try
            {
                packageUtil = new PackageUtil();
                package_subscription objsubscription = new package_subscription();
                var data = db.packages.AsEnumerable().Where(x => x.package_id == packageid).First();
                objsubscription.subscription_start_date = DateTime.Now;
                objsubscription.subscription_end_date = DateTime.Now.AddDays(Convert.ToDouble(data.duration_days.duration_days_value));
                objsubscription.subscription_price = data.setup_cost;
                objsubscription.subscription_discount = 0;
                objsubscription.subscription_tax = 0;
                objsubscription.user_id = SessionUtil.GetUserID();
                objsubscription.per_user_price = data.per_user_price;
                objsubscription.package_id = data.package_id;
                objsubscription.duration_days_id = data.duration_days_id;
                objsubscription.payment_status_id = 1;
                objsubscription.company_id = company.company_id;
                result = packageUtil.Post_Package_Subscription(objsubscription);
                switch (result.MessageType)
                {
                    case MessageType.Success:
                        return RedirectToAction("EditCompany", "CompanySetting", new { id = company.company_id, Result = result.Message, MessageType = result.MessageType });

                    default:
                        return RedirectToAction("Index", "CompanySetting", new { Result = result.Message, MessageType = result.MessageType });
                }
            }
            catch
            {
                return View();
            }
            return View();
            //return RedirectToAction("Index", "Company", new { Result = result.Message, MessageType = result.MessageType });
        }

        #region Assign Controller
        [HttpPost]
        public ActionResult PostAssignViewAction(string id, int companyID)
        {
            result = new Result();
            try
            {
                string[] str = id.Split(','); //Chk_CityController_Create_False,
                for (int i = 0; i < str.Length; i++)
                {
                    if (!String.IsNullOrEmpty(str[i]))
                    {
                        string[] info = str[i].Split('_');
                        if (!String.IsNullOrEmpty(info[1]))
                        {
                            string roleName = info[1];
                            string controllerName = info[2];
                            bool isAssigned = Convert.ToBoolean(info[3]);
                            var roleID = db.roles.AsEnumerable().Where(x => x.role_name == roleName && x.company_id == companyID).Select(x => x.role_id).FirstOrDefault();
                            var listAllAction = STUtil.GetListAllActionByController(controllerName);
                            //var listController = STUtil.GetListController();
                            var roleaction = db.role_action.Where(ra => ra.controller_name == controllerName && ra.role_id == roleID).ToList();
                            if (roleaction.Count == 0) // new
                            {
                                foreach (var item in listAllAction)
                                {
                                    role_action roleAction = new role_action();
                                    roleAction.role_id = roleID;
                                    roleAction.controller_name = controllerName;
                                    roleAction.action_name = item.Value;
                                    db.role_action.Add(roleAction);
                                }
                            }
                            else
                            {
                                db.role_action.RemoveRange(roleaction);
                            }
                            db.SaveChanges();
                            Thread.Sleep(4000);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return RedirectToAction("Index", result);
        }
        #endregion

        #region Subscription
        public ActionResult updatepaymentstatus(string remark, string paid, string can, string package_subscription_id)
        {
            packageUtil = new PackageUtil();
            int val = Convert.ToInt32(package_subscription_id);
            package_subscription packageSubscription = new package_subscription();

            packageSubscription.package_subscription_id = val;
            if (paid != null && paid != "")
            {
                packageSubscription.payment_detail = remark;
                packageSubscription.payment_status_id = Convert.ToInt32(paid);
            }
            if (can != null && can != "")
            {
                packageSubscription.cancel_remark = remark;
                packageSubscription.payment_status_id = Convert.ToInt32(can);
            }
            result = packageUtil.Post_Package_Subscription(packageSubscription);
            switch (result.MessageType)
            {
                case MessageType.Success:
                    return RedirectToAction("Index", "CompanySetting", new { Result = result.Message, MessageType = result.MessageType });
                default:
                    return RedirectToAction("Index", "CompanySetting", new { Result = result.Message, MessageType = result.MessageType });
            }
        }
        #endregion

        [HttpPost]
        public ActionResult PostCreateEditRole(role role)
        {
            try
            {
                result = new Result();
                if (role.role_id > 0)
                {
                    role find = db.roles.Find(role.role_id);
                    find.is_active = role.is_active;
                    find.role_name = role.role_name;
                    db.Entry(find).State = System.Data.Entity.EntityState.Modified;
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Role");
                }
                else
                {
                    long prevRoleBit = db.roles.Where(x => x.company_id == role.company_id).Max(x => x.role_bit);
                    role.role_bit = prevRoleBit * 2;
                    role.company_id = role.company_id;
                    db.roles.Add(role);
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Role");
                }
                STUtil.SetSessionValue(UserInfo.IsCompanyAddUpdate.ToString(), "0");
                db.SaveChanges();
                STUtil.SetSessionValue(UserInfo.IsCompanyAddUpdate.ToString(), "1");
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return Json(result);
        }


        public ActionResult getinvoice(int id)
        {
            ViewBag.Title = "Invoice";
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Invoice");
            var li = db.package_subscription.Where(x => x.package_subscription_id == id).FirstOrDefault();
            ViewBag.admincompany = db.companies.AsEnumerable().Where(x => x.company_id == Convert.ToInt32(SessionUtil.GetCompanyID())).FirstOrDefault();
            return View(li);

        }

        #region COMPANY HOLIDAY
        public ActionResult CompanyHoliday(string id)
        {
            company_holiday ch = new company_holiday();
            ch.company_id = Convert.ToInt32(id);
            return PartialView(BaseModel.GetPartialViewPath("CompanyHoliday"), ch);
        }
        public ActionResult EditCompanyHolidayIndex(int id)
        {
            var list = db.company_holiday.AsEnumerable().Where(x => x.company_holiday_id == id && x.company_id == SessionUtil.GetCompanyID()).ToList();
            var data = (from ch in list
                        select new
                        {
                            company_id = ch.company_id,
                            created_by = ch.created_by,
                            created_date = ch.created_date.ToString(),
                            company_holiday_id = ch.company_holiday_id,
                            holiday_name = ch.holiday_name,
                            holiday_date = DateTime.ParseExact(ch.holiday_date.ToShortDateString(), "dd/mm/yyyy", null).ToString("mm/dd/yyyy"),
                            is_active = ch.is_active,
                        }).FirstOrDefault();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CompanyHolidayList(string id)
        {
            var list = db.company_holiday.AsEnumerable().Where(x => x.company_id == SessionUtil.GetCompanyID()).ToList();
            var data = (from li in list
                        select new
                        {
                            company_holiday_id = li.company_holiday_id,
                            holiday_name = li.holiday_name,
                            holiday_date = DateTime.ParseExact(li.holiday_date.ToShortDateString(), "dd/mm/yyyy", null).ToString("mm/dd/yyyy"),
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        [HttpPost]
        public ActionResult CompanyHolidayCreateEdit(company_holiday company_Holiday)
        {
            try
            {
                result = companyUtil.PostCompanyHolidayCreate(company_Holiday);
                result.MessageType = MessageType.Success;
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return Json(result);
        }
        #endregion

        
        #endregion
    }
}