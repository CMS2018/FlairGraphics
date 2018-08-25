using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FlairGraphic.Models;
using FlairGraphic.Base.Models;
using System.IO;
using System.Reflection;
namespace FlairGraphic.Controllers
{
    public class ClientController : BaseController
    {
        // GET: Client
        private UserUtil userUtil;
        private Result result;
        public ClientController()
        {
            db = new BaseEntities();
            result = new Result();
            userUtil = new UserUtil();
        }
        public ActionResult Index()
        {
            user user = new user();
            int company_id = SessionUtil.GetCompanyID();
            user.company_id = company_id;
            return View(user);
        }
        public ActionResult ClientList()
        {
            var CompanyId = SessionUtil.GetCompanyID();
            var data = (from list in db.users.AsEnumerable()
                        where list.role_bit == (Int32)Role.Client && list.company_id == CompanyId
                        select new
                        {
                            user_id = list.user_id,
                            user_name = list.user_name,
                            email_id = list.email_id,
                            mobile = list.mobile,
                            gender = list.gender,
                            is_active = list.is_active,
                        }).ToList();
            return Json(data);
        }
        [HttpPost]
        public ActionResult CreateEditClient(user user, HttpPostedFileBase user_photo, FormCollection frmAdminUser)
        {
            try
            {
                user.role_bit = (Int32)Role.Client;
                user.role_id =db.roles.AsEnumerable().Where(x=>x.role_bit == (Int32)Role.Client && x.company_id==(Int32)SessionUtil.GetCompanyID()).FirstOrDefault().role_id;
                var role = db.roles.AsEnumerable().Where(x => x.company_id == user.company_id && x.role_bit == (Int32)Role.Client && x.is_active).ToList();
                user.role_id = role.Count > 0 ? role.Select(x => x.role_id).FirstOrDefault() : 0;
                var userdata = db.users.Find(user.user_id);
                company CompanyInformation = db.companies.Find(user.company_id);
                if (user_photo != null)
                {
                    string AWSProfileName = STUtil.GetWebConfigValue("AWSProfileName");
                    string GenFileName = STUtil.GetTodayDate().ToString("yyyyMMdd") + "_" + CompanyInformation.company_id.ToString() + "_" + Path.GetFileName(user_photo.FileName).Replace(" ", "_");
                    String companyFolderName = CompanyInformation.company_folder_name.ToString().Replace("/", "");
                    //// AWSUtil.UploadFile(user_photo.InputStream, AWSProfileName, companyFolderName, GenFileName);
                    user.user_photo = GenFileName;
                    UploadFile(SessionUtil.GetCompanyFolderName().ToString(), user_photo);
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
            return RedirectToAction("Index", "Client", new { Result = result.Message, MessageType = result.MessageType });
        }
        public ActionResult ClientEdit(string id)
        {
            var userList = db.users.AsEnumerable().Where(u => u.user_id == Convert.ToInt32(id)).ToList();
            int company_id = userList.FirstOrDefault().company_id;
            company company = new company();
            company = company_id > 0 ? db.companies.Find(company_id) : company;
            string SNAG_AWS_S3 = STUtil.GetWebConfigValue("ImagePath");
            String companyFolderName = company.company_folder_name != null ? company.company_folder_name.ToString() : "";
            var UserPhoto = userList.FirstOrDefault().user_photo != null ? SNAG_AWS_S3 + companyFolderName + userList.FirstOrDefault().user_photo : SNAG_AWS_S3 + userList.FirstOrDefault().user_photo;
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
                            user_photo = u.user_photo,
                            user_photoSRC = AWSUtil.GetFileURL(u.user_photo),
                            hdnUserPhoto = u.user_photo,
                            role_id = u.role_id,
                        }).FirstOrDefault();
            return Json(data);
        }
    }
}