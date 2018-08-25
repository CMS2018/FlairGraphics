using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlairGraphic.Base.Models;
using FlairGraphic.Models;

namespace FlairGraphic.Controllers
{
    public class MyProfileController : BaseController
    {
        UserUtil userUtil;
        Result result;


        public MyProfileController()
        {
            userUtil = new UserUtil();
            result = new Result();
        }

        #region My Profile

        // GET: MyProfile
        public ActionResult MyProfile()
        {
            ViewBag.Title = "My Profile";
            int userid = SessionUtil.GetUserID();
            user user = db.users.Find(userid);
            return View(user);
        }
        [HttpPost]
        public ActionResult EditProfile(user user, HttpPostedFileBase user_photo)
        {

            try
            {
                if (user_photo != null)
                {
                    string AWSProfileName = STUtil.GetWebConfigValue("AWSProfileName");
                    string GenFileName = STUtil.GetTodayDate().ToString("yyyyMMdd") + "_" + SessionUtil.GetCompanyID().ToString() + "_" + Path.GetFileName(user_photo.FileName).Replace(" ", "_");
                    String companyFolderName = STUtil.GetSessionValue(UserInfo.CompanyFolderName.ToString()).ToString().Replace("/", "");
                    UploadFile(SessionUtil.GetCompanyFolderName().ToString(), user_photo);
                    user.user_photo = GenFileName;
                }
                else
                {

                }
                result = userUtil.PostProfileEdit(user);
                ViewBag.action_name = STUtil.GetListAllActionByController("");
                switch (result.MessageType)
                {
                    case MessageType.Success:
                        STUtil.SetSessionValue(UserInfo.FullName.ToString(), Convert.ToString(user.user_name));
                        STUtil.SetSessionValue(UserInfo.Mobile.ToString(), Convert.ToString(user.mobile));
                        STUtil.SetSessionValue(UserInfo.UserPhoto.ToString(), Convert.ToString(user.user_photo));
                        STUtil.SetSessionValue(UserInfo.Gender.ToString(), Convert.ToString(user.gender));

                        return RedirectToAction("MyProfile", "MyProfile", new { Result = result.Message, MessageType = result.MessageType });
                    default:
                        return RedirectToAction("MyProfile", "MyProfile", new { Result = result.Message, MessageType = result.MessageType });
                }
                return View(user);
            }
            catch
            {
                return View(user);
            }
        }
        public ActionResult ChangePassword()
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Change Password");
            ViewBag.Title = "Change Password";
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(FormCollection frm)
        {
            try
            {
                int User_id = SessionUtil.GetUserID();
                string Old_password = frm["oldPassword"];
                string New_password = frm["newpassword"];
                string conform_password = frm["conformpassword"];
                result = userUtil.PostChangePassword(User_id, Old_password, New_password, conform_password);

                switch (result.MessageType)
                {
                    case MessageType.Success:
                        return RedirectToAction("ChangePassword", "MyProfile", new { Result = result.Message, MessageType = result.MessageType });
                    default:
                        return RedirectToAction("ChangePassword", "MyProfile", new { Result = result.Message, MessageType = result.MessageType });
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        #endregion

    }
}
