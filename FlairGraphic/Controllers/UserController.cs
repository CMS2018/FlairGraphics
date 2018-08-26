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

namespace FlairGraphic.Controllers
{
    public class UserController : BaseController
    {
        UserUtil userUtil;
        Result result;
        public UserController()
        {
            userUtil = new UserUtil();
            result = new Result();
        }
        public ActionResult UserIndex(string id)
        {
            int company_id = SessionUtil.GetCompanyID();
            var roleList = db.roles.Where(x => x.is_public && x.is_active && x.company_id == company_id).ToList();
            return View(roleList);
        }
        public ActionResult Index(string id = "")
        {
            user user = new user();
            int company_id = SessionUtil.GetCompanyID();
            ViewBag.role_bit = new SelectList(RoleUtil.GetParentRoleBitByCompany(company_id), "Value", "Text");
            user.company_id = company_id;
            return View(user);
        }
        public ActionResult UsersList(string id)
        {
            int CompanyId = SessionUtil.GetCompanyID();
            //int rolebit = Convert.ToInt32(id);
            IList<user> list = db.users.AsEnumerable().Where(x => x.company_id == CompanyId &&x.role_bit!=8 && x.role_bit>2).ToList();
            var data = (from li in list
                        select new
                        {
                            user_id = li.user_id,
                            user_name = li.user_name,
                            email_id = li.email_id,
                            mobile = li.mobile,
                            gender = li.gender,
                            RoleName = li.role.role_name,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        public ActionResult GetRoleDetail(string id = "")
        {
            string[] info = id.Split(',');
            int role_bit = 0;
            int company_id = 0;
            if (info.Length > 0)
            {
                role_bit = string.IsNullOrEmpty(info[0]) ? 0 : Convert.ToInt32(info[0]);
                company_id = string.IsNullOrEmpty(info[1]) ? 0 : Convert.ToInt32(info[1]);
            }
            var roleDetail = (from R in db.roles.AsEnumerable()
                              where R.role_bit == role_bit && R.company_id == company_id
                              select new
                              {
                                  role_id = R.role_id,
                              }).FirstOrDefault();
            return Json(roleDetail);
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
                            hdnUserPhoto = u.user_photo,
                            role_id = u.role_id,
                            is_service_provider = u.is_service_provider,
                            is_change_requester = u.is_change_requester,
                            view_work_order_access_id = u.view_work_order_access_id,
                            create_work_order_access_id = u.create_work_order_access_id,
                        }).FirstOrDefault();

            return Json(data);
        }
        [HttpPost]
        public ActionResult CreateEditUser(user user, HttpPostedFileBase user_photo, FormCollection frmAdminUser)
        {
            try
            {
                user.role_id = String.IsNullOrEmpty(frmAdminUser["role_id"]) ? 0 : Convert.ToInt32(frmAdminUser["role_id"]);
                var userdata = db.users.Find(user.user_id);
                company CompanyInformation = db.companies.Find(user.company_id);
                if (user_photo != null)
                {
                    string AWSProfileName = STUtil.GetWebConfigValue("AWSProfileName");
                    string GenFileName = STUtil.GetTodayDate().ToString("yyyyMMdd") + "_" + CompanyInformation.company_id.ToString() + "_" + Path.GetFileName(user_photo.FileName).Replace(" ", "_");
                    String companyFolderName = CompanyInformation.company_folder_name.ToString().Replace("/", "");
                   //// AWSUtil.UploadFile(user_photo.InputStream, AWSProfileName, companyFolderName, GenFileName);
                    user.user_photo = GenFileName;
                }
                else
                {
                    user.user_photo = userdata != null ? userdata.user_photo : "NA.jpg";
                }
                user.parent_user_id = SessionUtil.GetUserID();
                result = userUtil.PostUserEdit(user);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.MessageType = MessageType.Error;
            }
            return RedirectToAction("Index", "User", new { id = String.Format("{0},{1}", user.role_id, user.role_bit), Result = result.Message, MessageType = result.MessageType });
        }
        [HttpPost]
        public ActionResult CreateEdit(user user, FormCollection frm, HttpPostedFileBase user_photo)
        {
            try
            {

                string rol_id = frm["rol_id"];
                string hdnUserPhoto = frm["hdnUserPhoto"];
                if (user_photo != null)
                {
                    string AWSProfileName = STUtil.GetWebConfigValue("AWSProfileName");
                    string GenFileName = STUtil.GetTodayDate().ToString("yyyyMMdd") + "_" + SessionUtil.GetCompanyID().ToString() + "_" + Path.GetFileName(user_photo.FileName).Replace(" ", "_");
                    String companyFolderName = STUtil.GetSessionValue(UserInfo.CompanyFolderName.ToString()).ToString().Replace("/", "");
                    user.user_photo = GenFileName;
                }
                else
                {
                    user.user_photo = hdnUserPhoto != null ? (hdnUserPhoto != "" ? hdnUserPhoto : user.gender + ".JPG") : user.gender + ".JPG";
                }
                result = userUtil.PostCreateEdit(user, frm);
                ViewBag.Title = user == null ? "User Create" : "User Edit";
                ViewBag.action_name = STUtil.GetListAllActionByController("");
                switch (result.MessageType)
                {
                    case MessageType.Success:
                        return RedirectToAction("Index", "User", new { id = rol_id, Result = result.Message, MessageType = result.MessageType });

                    default:
                        return RedirectToAction("CreateEdit", "User", new { id = user.user_id, Result = result.Message, MessageType = result.MessageType });
                }
                return View(user);
            }
            catch (Exception ex)
            {
                return View(user);
            }
        }
    
 
   
    }
}