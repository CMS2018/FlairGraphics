using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FlairGraphic.Base.Models;

namespace FlairGraphic.Models
{
    #region Business

    public class UserUtil
    {

        private Result result;
        private BaseEntities db;
        public UserUtil()
        {
            this.result = new Result();
            this.db = new BaseEntities();
        }

        public String userRegistrion(String fullName, String mobileNumber, String emailaddress)
        {
            String result = "";

            try
            {

                //var userReg = db.USP_Process_Registration(fullName, mobileNumber, emailaddress, 1).FirstOrDefault();
                //if (userReg != null)
                //{
                //    string AWSProfileName = STUtil.GetWebConfigValue("AWSProfileName");
                //    string ImagePath = STUtil.GetWebConfigValue("ImagePath");
                //    string EMAIL_TEMPALTE_FOLDER = STUtil.GetWebConfigValue("EMAIL_TEMPALTE_FOLDER");
                //    AWSUtil.FolderCreation(AWSProfileName, userReg.company_folder_name);
                //    var emailTempRecord = db.email_template.AsEnumerable().Where(x => x.email_template_type_id == 1).OrderByDescending(x => x.email_template_id).FirstOrDefault();
                //    if (emailTempRecord != null)
                //    {

                //        String templateName = ImagePath + EMAIL_TEMPALTE_FOLDER + "/" + emailTempRecord.email_template_file_name;
                //        var filePath = (new WebClient()).DownloadString(templateName);
                //        filePath = filePath.Replace("{UserName}", userReg.user_name);
                //        filePath = filePath.Replace("{ACTIVATE_ACCOUNT_LINK}", userReg.activation_link);

                //        STUtil.SendMail(userReg.email_id, "Verify your Agency CRM email address ", filePath, null);


                //    }
                //}


            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }
        public String resetPassword(String loginID)
        {
            String result = "";

            try
            {
                //var userReg = db.USP_Process_ResetPWDLink(loginID).FirstOrDefault();
                //if (userReg != null)
                //{
                //    var emailTempRecord = db.email_template.AsEnumerable().Where(x => x.email_template_type_id == 2).OrderByDescending(x => x.email_template_id).FirstOrDefault();
                //    if (emailTempRecord != null)
                //    {

                //        string ImagePath = STUtil.GetWebConfigValue("ImagePath");
                //        string EMAIL_TEMPALTE_FOLDER = STUtil.GetWebConfigValue("EMAIL_TEMPALTE_FOLDER");

                //        String templateName = ImagePath + EMAIL_TEMPALTE_FOLDER + "/" + emailTempRecord.email_template_file_name;
                //        var filePath = (new WebClient()).DownloadString(templateName);
                //        filePath = filePath.Replace("{UserName}", userReg.user_name);
                //        filePath = filePath.Replace("{RESET_PASSWORD_LINK}", userReg.reset_password_link);

                //        STUtil.SendMail(userReg.email_id, "Reset your Agency CRM password for login ID :" + loginID, filePath, null);
                //    }
                //}
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return result;
        }
        public user IsEmailAddressExist(string emailID, int RoleBit)
        {

            return db.users.AsEnumerable().Where(x => x.email_id.ToUpper() == emailID.ToUpper() && x.role_bit == RoleBit).FirstOrDefault();

        }
        public user IsLoginIDExist(string LoginID)
        {

            return db.users.AsEnumerable().Where(x => x.login_id.ToUpper() == LoginID.ToUpper()).FirstOrDefault();

        }
        public string ActivateUser(Int32 userID, String key, String LoginID, String Password, String BusinessName)
        {
            try
            {
                var userInfo = db.users.AsEnumerable().Where(x => x.user_id == userID && x.activation_reset_key == key && x.is_active == false).FirstOrDefault();

                if (userInfo != null)
                {


                    db.USP_ActivateAccount(userInfo.user_id, userInfo.company_id, LoginID, Password, BusinessName);

                    db = new BaseEntities();
                    var userInfoForEmail = db.users.Find(userInfo.user_id);

                    //var emailTempRecord = db.email_template.AsEnumerable().Where(x => x.email_template_type_id == 5).OrderByDescending(x => x.email_template_id).FirstOrDefault();//5 - ACCOUNT_ACTIVATED
                    //if (emailTempRecord != null)
                    //{


                    //    string ImagePath = STUtil.GetWebConfigValue("ImagePath");
                    //    string EMAIL_TEMPALTE_FOLDER = STUtil.GetWebConfigValue("EMAIL_TEMPALTE_FOLDER");
                    //    string APPLICATION_URL = STUtil.GetWebConfigValue("APPLICATION_URL");

                    //    String templateName = ImagePath + EMAIL_TEMPALTE_FOLDER + "/" + emailTempRecord.email_template_file_name;
                    //    var filePath = (new WebClient()).DownloadString(templateName);
                    //    filePath = filePath.Replace("{UserName}", userInfoForEmail.user_name);
                    //    filePath = filePath.Replace("{business_name}", userInfoForEmail.company.business_name);
                    //    filePath = filePath.Replace("{LOGIN_ID}", userInfoForEmail.login_id);
                    //    filePath = filePath.Replace("{PASSWORD}", Password);
                    //    filePath = filePath.Replace("{EMAIL_ID}", userInfoForEmail.email_id);
                    //    filePath = filePath.Replace("{APPLICATION_URL}", APPLICATION_URL);

                    //    STUtil.SendMail(userInfo.email_id, "Welcome to Agency CRM : " + userInfoForEmail.login_id, filePath, null);


                    //}
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return "";
        }
        public string ResetUser(Int32 userID, String key, string Password)
        {
            try
            {
                var userInfo = db.users.AsEnumerable().Where(x => x.user_id == userID && x.activation_reset_key == key && x.is_active == true).FirstOrDefault();

                if (userInfo != null)
                {

                    db.USP_RestAccount(userInfo.user_id, Password);

                    db = new BaseEntities();
                    var userInfoForEmail = db.users.Find(userInfo.user_id);

                    //var emailTempRecord = db.email_template.AsEnumerable().Where(x => x.email_template_type_id == 6).OrderByDescending(x => x.email_template_id).FirstOrDefault();//6 - ACCOUNT_RESET
                    //if (emailTempRecord != null)
                    //{

                    //    string ImagePath = STUtil.GetWebConfigValue("ImagePath");
                    //    string EMAIL_TEMPALTE_FOLDER = STUtil.GetWebConfigValue("EMAIL_TEMPALTE_FOLDER");
                    //    string APPLICATION_URL = STUtil.GetWebConfigValue("APPLICATION_URL");

                    //    String templateName = ImagePath + EMAIL_TEMPALTE_FOLDER + "/" + emailTempRecord.email_template_file_name;
                    //    var filePath = (new WebClient()).DownloadString(templateName);
                    //    filePath = filePath.Replace("{UserName}", userInfoForEmail.user_name);
                    //    filePath = filePath.Replace("{business_name}", userInfoForEmail.company.business_name);
                    //    filePath = filePath.Replace("{LOGIN_ID}", userInfoForEmail.login_id);
                    //    filePath = filePath.Replace("{PASSWORD}", Password);
                    //    filePath = filePath.Replace("{EMAIL_ID}", userInfoForEmail.email_id);
                    //    filePath = filePath.Replace("{APPLICATION_URL}", APPLICATION_URL);

                    //    STUtil.SendMail(userInfo.email_id, "Welcome to Agency CRM (With new login credentials): " + userInfoForEmail.login_id, filePath, null);


                    //}
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return "";
        }
        public List<SelectListItem> GetRole(int id)
        {
            var list = (from s in db.roles.Where(x => x.is_public == true)
                        select new SelectListItem
                        {
                            Text = s.role_name,
                            Value = s.role_id.ToString(),
                        }).ToList();
            return list;
        }
        public List<SelectListItem> GetEmailOwnerList()
        {
            int companyId = (Int32)SessionUtil.GetCompanyID();
            var list = (from c in db.users.AsEnumerable()
                        where c.company_id == companyId
                        select new SelectListItem
                        {
                            Text = c.user_name + "(" + c.email_id + ")",
                            Value = c.email_id.ToString(),
                        }).ToList();
            return list;
        }

        #region USER
        public user IsEmailExist(string emailID, int userId)
        {

            var list = db.users.AsEnumerable().Where(x => x.company_id == SessionUtil.GetCompanyID()).ToList();
            return userId > 0 ?
                 list.AsEnumerable().Where(x => x.email_id.ToUpper() == emailID.ToUpper() && x.user_id != userId && x.company_id == SessionUtil.GetCompanyID()).FirstOrDefault()
                : list.AsEnumerable().Where(x => x.email_id.ToUpper() == emailID.ToUpper() && x.company_id == SessionUtil.GetCompanyID()).FirstOrDefault();
        }
        public user IsRegEmailExist(string emailID, int roleBit)
        {
            var list = db.users.AsEnumerable().Where(x => x.is_active == true && x.email_id.ToUpper() == emailID.ToUpper() && x.role_bit == roleBit).FirstOrDefault();
            return list;
        }
        public IList<SelectListItem> ParentUserList(int roleBit)
        {
            return (from c in db.users.AsEnumerable()
                    where c.company_id == SessionUtil.GetCompanyID() && c.role_bit == roleBit
                    select new SelectListItem
                    {
                        Value = c.user_id.ToString(),
                        Text = c.user_name

                    }).OrderBy(x => x.Text).ToList();
        }
        public Boolean IsParentDDL(int roleId)
        {
            var role = db.roles.Find(roleId);
            return (role.parent_id == null ? false : true);
        }
        public Result PostUserEdit(user user)
        {
            try
            {
                if (user.user_id > 0)
                {
                    string password = db.USP_GetUserPassword(user.user_id, user.company_id).ToList().FirstOrDefault().password;
                    var isUserEdit = db.USP_CreateUser(user.user_id, user.user_name, user.login_id, user.email_id, user.mobile, password,
                         user.gender, user.user_photo, user.parent_user_id, user.role_bit, user.company_id, user.created_by, user.role_id, user.payment_term_id, user.create_work_order_access_id, user.view_work_order_access_id, user.is_change_requester, user.is_service_provider, user.is_active).ToList();
                    result.MessageType = isUserEdit.FirstOrDefault().UserID > 0 ? MessageType.Success : MessageType.Error;
                    result.Message = isUserEdit.FirstOrDefault().MSG;
                    result.Id = isUserEdit.FirstOrDefault().UserID > 0 ? isUserEdit.FirstOrDefault().UserID : 0;
                }
                else
                {
                    var pass = STUtil.GetRandomPasswordNumber(6);
                    int created_by = SessionUtil.GetUserID();
                    var isUserEdit = db.USP_CreateUser(user.user_id, user.user_name, user.login_id, user.email_id, user.mobile, pass,
                         user.gender, user.user_photo, user.parent_user_id, user.role_bit, user.company_id, created_by, user.role_id,user.payment_term_id, user.create_work_order_access_id, user.view_work_order_access_id, user.is_change_requester, user.is_service_provider, true).ToList();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "User");
                    result.MessageType = isUserEdit.FirstOrDefault().MSG=="Success" ? MessageType.Success : MessageType.Error;
                    result.Message = isUserEdit.FirstOrDefault().MSG;
                    result.Id = isUserEdit.FirstOrDefault().UserID > 0 ? isUserEdit.FirstOrDefault().UserID : 0;
                }
                if ((Int32)result.Id>0)
                {
                    var UserData = db.users.Find(result.Id);
                    UserData.gstin_numer = user.gstin_numer;
                    UserData.billing_address = user.billing_address;
                    UserData.shipping_address = user.shipping_address;
                    UserData.payment_terms = user.payment_terms;
                    db.Entry(UserData).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return result;
        }
        public Result PostCreateEdit(user user, FormCollection frm)
        {
            try
            {
                string password = frm["userpassword"];
                db = new BaseEntities();
                if (user.user_id > 0)
                {
                    var msg = db.USP_CreateUser(user.user_id, user.user_name, user.login_id, user.email_id, user.mobile, password, user.gender, user.user_photo, user.parent_user_id, user.role_bit, SessionUtil.GetCompanyID(), SessionUtil.GetUserID(), user.role_id, null,user.create_work_order_access_id, user.view_work_order_access_id, user.is_change_requester, user.is_service_provider, user.is_active).FirstOrDefault().MSG;
                    result.Message = msg == "Success" ? string.Format(BaseConst.MSG_SUCCESS_CREATE, "User") : msg;
                }
                else
                {
                    user.is_active = true;
                    var msg = db.USP_CreateUser(user.user_id, user.user_name, user.login_id, user.email_id, user.mobile, password, user.gender, user.user_photo, user.parent_user_id, user.role_bit, SessionUtil.GetCompanyID(), SessionUtil.GetUserID(), user.role_id,null, user.create_work_order_access_id, user.view_work_order_access_id, user.is_change_requester, user.is_service_provider, user.is_active).FirstOrDefault().MSG;
                    result.Message = msg == "Success" ? string.Format(BaseConst.MSG_SUCCESS_UPDATE, "User") : msg;
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return result;
        }
        public Result PostProfileEdit(user user)
        {
            try
            {
                db = new BaseEntities();
                user userdata = db.users.Find(user.user_id);
                if (user.user_id > 0)
                {
                    userdata.user_name = user.user_name;
                    userdata.mobile = user.mobile;
                    userdata.gender = user.gender;
                    userdata.user_photo = user.user_photo;
                    db.Entry(userdata).State = System.Data.Entity.EntityState.Modified;
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "User Profile");
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return result;
        }
        public Result PostChangePassword(int userid, string oldpasword, string newpassword, string conformpassword)
        {
            try
            {
                db = new BaseEntities();
                user userdata = db.users.Find(userid);
                if (userdata.user_id > 0)
                {
                    var msg = db.USP_ChangePassword(userid, oldpasword, newpassword, conformpassword).FirstOrDefault().MSG;
                    result.Message = msg;
                    result.MessageType = msg == "Success" ? MessageType.Success : MessageType.Warning;
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        public List<SelectListItem> GetPaymentTermList()
        {
            int companyId = (Int32)SessionUtil.GetCompanyID();
            var list = (from c in db.payment_term.AsEnumerable()
                        where c.company_id == companyId && c.is_active
                        select new SelectListItem
                        {
                            Text = c.payment_term_name,
                            Value = c.payment_term_id.ToString(),
                        }).ToList();
            return list;
        }
        #endregion
    }
    #endregion

}