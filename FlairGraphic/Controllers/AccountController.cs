using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FlairGraphic.Base.Models;
using FlairGraphic.Models;

namespace FlairGraphic.Controllers
{
    public class AccountController : BaseController
    {
        //
        // GET: /Account/ 
        private Result result;
        private UserUtil userUtil;
        public AccountController()
        {
            result = new Result();
            userUtil = new UserUtil();
        }
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection frm)
        {
            String LoginResult = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    String LoginID = frm["LoginID"];
                    String pwd = frm["password"];
                    LoginResult = RoleUtil.CheckUserFrofile(LoginID, pwd);
                    if (LoginResult == "PASS")
                    {
                        var list = RoleUtil.GetMenusOfRoleId(Convert.ToInt32(STUtil.GetSessionValue(UserInfo.RoleID.ToString())), Convert.ToInt32(STUtil.GetSessionValue(UserInfo.CompanyID.ToString()))).ToList();
                        var menuObjects = (Session[UserInfo.MenuList.ToString()] as List<TreeNode>) ?? new List<TreeNode>();
                        menuObjects.AddRange(list);
                        Session[UserInfo.MenuList.ToString()] = menuObjects;
                        TempData["Login"] = "Login";
                        return RedirectToAction("Index/", "DashBoard");
                    }
                    ViewBag.result = LoginResult;


                }
                catch (Exception ex)
                {
                  //  ExceptionLogging.SendErrorToText(ex);  
                    ViewBag.result = ex.Message;
                }
            }
            else
            {
                ViewBag.result = STUtil.GetValidationMessage(ModelState);
            }
            return RedirectToAction("Login", new { Result = LoginResult, MessageType = "Error" });
            //return View();
        }

        public ActionResult SignUp(string id)
        {
            try
            {
                string[] info = id.Split(',');
                string fullName = "";
                string mobileNumber = "";
                string emailaddress = "";

                if (info.Length == 3)
                {
                    fullName = info[0];
                    mobileNumber = info[1];
                    emailaddress = info[2];
                    bool isEmailExist = userUtil.IsEmailAddressExist(emailaddress, 2) == null ? false : true;
                    if (!isEmailExist)
                    {
                        userUtil.userRegistrion(fullName, mobileNumber, emailaddress);
                        return Json("PASS");
                    }
                    else
                    {
                        return Json("Youre email ID '" + emailaddress + "' are Already Register With us!!");
                    }
                }
                return Json("");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public ActionResult ResetPWD(string id)
        {
            try
            {
                string[] info = id.Split(',');

                string loginID = info[0];


                bool isLoginIDExist = userUtil.IsLoginIDExist(loginID) == null ? false : true;
                if (isLoginIDExist)
                {
                    userUtil.resetPassword(loginID);
                    return Json("PASS");
                }
                else
                {
                    return Json("Youre Login ID '" + loginID + "' are not Register With us!!");
                }

                return Json("");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", new RouteValueDictionary(new { controller = "Account", action = "Login" }));
        }

        public ActionResult UserAuthenticate(Int32 userID, String key)
        {
            try
            {
                var userInfo = db.users.AsEnumerable().Where(x => x.user_id == userID && x.activation_reset_key == key && x.is_active == false).FirstOrDefault();
                if (userInfo != null)
                {
                    ViewBag.UserName = userInfo.user_name;
                    ViewBag.EmailID = userInfo.email_id;
                    ViewBag.userID = userInfo.user_id;
                    ViewBag.key = userInfo.activation_reset_key;
                    ViewBag.result = "";
                }
                else
                {
                    ViewBag.result = "Invalid Request";
                    return RedirectToAction("Login", "Account", new { result = "Invalid Request", MessageType = "Error" });

                }
            }
            catch (Exception ex)
            {
                ViewBag.result = ex.Message;
                return RedirectToAction("Login", "Account", new { result = ex.Message, MessageType = "Error" });

            }

            return View();
        }

        [HttpPost]
        public ActionResult UserAuthenticate(FormCollection frm)
        {
            try
            {
                Int32 userID = Convert.ToInt32(frm["userID"]);
                String key = frm["key"];
                String LoginID = frm["LoginID"];
                String Password = frm["password"];
                String BusinessName = frm["BusinessName"];
                Boolean isLoginIDExist = userUtil.IsLoginIDExist(LoginID) == null ? false : true;

                if (!isLoginIDExist)
                {
                    var userInfo = db.users.AsEnumerable().Where(x => x.user_id == userID && x.activation_reset_key == key && x.is_active == false).FirstOrDefault();
                    if (userInfo != null)
                    {
                        userUtil.ActivateUser(userID, key, LoginID, Password, BusinessName);


                        String LoginResult = RoleUtil.CheckUserFrofile("", "", userInfo.user_id);
                        if (LoginResult == "PASS")
                        {
                            var list = RoleUtil.GetMenusOfRoleId(Convert.ToInt32(STUtil.GetSessionValue(UserInfo.RoleID.ToString())), Convert.ToInt32(STUtil.GetSessionValue(UserInfo.CompanyID.ToString()))).ToList();
                            var menuObjects = (Session[UserInfo.MenuList.ToString()] as List<TreeNode>) ?? new List<TreeNode>();
                            menuObjects.AddRange(list);
                            Session[UserInfo.MenuList.ToString()] = menuObjects;


                             

                                return RedirectToAction("Index/", "DashBoard");
                             
                        }

                    }
                    else
                    {
                        ViewBag.result = "Invalid Request";
                    }
                }
                else
                {
                    ViewBag.result = "LoginID '" + LoginID + " already exist!!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.result = ex.Message;
                return RedirectToAction("Login", "Account", new { result = ex.Message, MessageType = "Error" });

            }
            return View();
        }

        public ActionResult UserResetPassword(Int32 userID, String key)
        {
            try
            {
                var userInfo = db.users.AsEnumerable().Where(x => x.user_id == userID && x.activation_reset_key == key && x.is_active == true).FirstOrDefault();
                if (userInfo != null)
                {
                    if (userInfo.reset_password_link_expire_date_time > STUtil.GetCurrentDateTime())
                    {
                        ViewBag.UserName = userInfo.user_name;
                        ViewBag.EmailID = userInfo.email_id;
                        ViewBag.userID = userInfo.user_id;
                        ViewBag.loginID = userInfo.login_id;
                        ViewBag.BusinessName = userInfo.company.business_name;
                        ViewBag.key = userInfo.activation_reset_key;
                        ViewBag.link_expire_date_time = userInfo.reset_password_link_expire_date_time;
                        ViewBag.result = "";
                    }
                    else
                    {
                        ViewBag.result = "Your password reset link has been expired!!!";
                        return RedirectToAction("Login", "Account", new { result = "Your password reset link has been expired!!!", MessageType = "Error" });

                    }
                }
                else
                {
                    ViewBag.result = "Invalid Request";
                    return RedirectToAction("Login", "Account", new { result = "Invalid Request", MessageType = "Error" });

                }
            }
            catch (Exception ex)
            {
                ViewBag.result = ex.Message;
                return RedirectToAction("Login", "Account", new { result = ex.Message, MessageType = "Error" });

            }

            return View();
        }

        [HttpPost]
        public ActionResult UserResetPassword(FormCollection frm)
        {
            try
            {
                Int32 userID = Convert.ToInt32(frm["userID"]);
                String key = frm["key"];
                //String LoginID = frm["LoginID"];
                String Password = frm["password"];
                //String BusinessName = frm["BusinessName"];



                var userInfo = db.users.AsEnumerable().Where(x => x.user_id == userID && x.activation_reset_key == key && x.is_active == true).FirstOrDefault();
                if (userInfo != null)
                {
                    if (userInfo.reset_password_link_expire_date_time > STUtil.GetCurrentDateTime())
                    {
                        userUtil.ResetUser(userID, key, Password);


                        String LoginResult = RoleUtil.CheckUserFrofile("", "", userInfo.user_id);
                        if (LoginResult == "PASS")
                        {
                            var list = RoleUtil.GetMenusOfRoleId(Convert.ToInt32(STUtil.GetSessionValue(UserInfo.RoleID.ToString())), Convert.ToInt32(STUtil.GetSessionValue(UserInfo.RoleID.ToString()))).ToList();
                            var menuObjects = (Session[UserInfo.MenuList.ToString()] as List<TreeNode>) ?? new List<TreeNode>();
                            menuObjects.AddRange(list);
                            Session[UserInfo.MenuList.ToString()] = menuObjects;


                            

                                return RedirectToAction("Index/", "DashBoard");
                            
                        }


                        else
                        {
                            return RedirectToAction("Login", "Account", new { result = "Invalid Request", MessageType = "Error" });

                        }
                    }
                    else
                    {
                        ViewBag.result = "Your password reset link has been expired!!!";
                        return RedirectToAction("Login", "Account", new { result = "Your password reset link has been expired!!!", MessageType = "Error" });

                    }
                }
                else
                {
                    ViewBag.result = "Invalid Request";
                    return RedirectToAction("Login", "Account", new { result = "Invalid Request", MessageType = "Error" });

                }
            }
            catch (Exception ex)
            {
                ViewBag.result = ex.Message;
                return RedirectToAction("Login", "Account", new { result = ex.Message, MessageType = "Error" });

            }
            return View();
        }

        public ActionResult ResetPassword(Int32 userID, String key)
        {
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