using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web;
using FlairGraphic.Base.Models;
using System.Net;
using System.IO;

namespace FlairGraphic.Models
{

    #region Classes & Enum
    public class CompanyUser
    {
        //public company company { get; set; }
        //public user user { get; set; }
        public int company_id { get; set; }
        public string admin_user_name { get; set; }
        public int language_id { get; set; }
        public string time_zone { get; set; }
        public string company_folder_name { get; set; }
        public string business_name { get; set; }
        public string country_code { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public int state_id { get; set; }
        public string company_logo { get; set; }
        public int parent_company_id { get; set; }
        public DateTime created_date { get; set; }
        public DateTime updated_date { get; set; }
        public int updated_by { get; set; }
        public bool is_active { get; set; }

        public int country_id { get; set; }


        public int user_id { get; set; }
        public string user_name { get; set; }
        public string login_id { get; set; }
        public string email_id { get; set; }
        public string mobile { get; set; }
        public string password { get; set; }
        public string gender { get; set; }
        public int report_to { get; set; }
        public string user_photo { get; set; }
        public DateTime last_login_date { get; set; }
        public string activation_link { get; set; }
        public string reset_password_link { get; set; }
        public DateTime reset_password_link_expire_date_time { get; set; }
        public string activation_reset_key { get; set; }
        public int parent_user_id { get; set; }
        public int role_bit { get; set; }
        public bool is_account_locked { get; set; }
        public int password_failed_attempt { get; set; }
        public string imei_number { get; set; }
        public string token_number { get; set; }
        public string os_version { get; set; }
    }

    #endregion

    #region Business
    public class CompanyUtil
    {
        private Result result;
        private BaseEntities db;
        public CompanyUtil()
        {
            this.result = new Result();
            this.db = new BaseEntities();
        }
        public IList<company> CompanyList()
        {
            int userID = 0, companyID = 0;
            IList<company> companyList = new List<company>();
            try
            {
                company companyCountRecord = new company();
                foreach (var item in db.companies.Where(x => x.is_active))
                {
                    companyCountRecord = new company();
                    companyCountRecord.business_name = item.business_name;
                    companyCountRecord.is_active = item.is_active;
                    companyCountRecord.company_id = item.company_id;
                    companyList.Add(companyCountRecord);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return companyList;
        }
        public CompanyUser GetCompanyData(string id)
        {
            int companyID = Convert.ToInt32(id);
            CompanyUser companyUserData = new CompanyUser();
            company company = db.companies.Find(companyID);
            companyUserData.company_id = company.company_id;
            companyUserData.business_name = company.business_name;
            companyUserData.time_zone = company.time_zone;
            companyUserData.country_code = company.country_code;
            companyUserData.phone = company.phone;
            companyUserData.country_id = company.state.country_id;
            companyUserData.city = company.city;
            companyUserData.address = company.address;
            companyUserData.zip = company.zip;
            companyUserData.is_active = company.is_active;

            user user = db.users.Where(x => x.company_id == companyID).FirstOrDefault();
            companyUserData.user_name = user.user_name;
            companyUserData.login_id = user.login_id;
            companyUserData.password = Convert.ToString(user.password);
            companyUserData.mobile = user.mobile;
            companyUserData.email_id = user.email_id;
            return companyUserData;
        }
        public Result PostCompanyCreateEdit(company company, HttpPostedFileBase company_logo, string userName, string loginID, string emailID, string mobile, string gender, string password)
        {
            try
            {
                string AWSProfileName = STUtil.GetWebConfigValue("AWSProfileName");
                int userID = SessionUtil.GetUserID();
                company companyEdit = new company();
                if (company_logo != null)
                {
                    string GenFileName = STUtil.GetTodayDate().ToString("yyyyMMdd") + "_" + company.company_id.ToString() + "_" + Path.GetFileName(company_logo.FileName).Replace(" ", "_");
                    company.company_logo = GenFileName;
                }
                if (company.company_id > 0)
                {
                   
                    companyEdit = db.companies.Find(company.company_id);
                    company.company_logo = (company.company_logo != "" && company.company_logo != null) ? company.company_logo : (companyEdit.company_logo);
                    var isUpadte = db.USP_UpdateCompany(company.company_id, userID, company.business_name, company.time_zone, company.country_code,
                        company.phone, company.state_id, company.city, company.address, company.zip, companyEdit.admin_user_name, company.is_active,
                        System.DateTime.Now,company.licence_no, company.company_logo);
                    db.SaveChanges();

                    #region FileUpload
                    if (company_logo != null)
                    {
                        String companyFolderName = companyEdit.company_folder_name.Replace("/", "");
                       //// AWSUtil.UploadFile(company_logo.InputStream, AWSProfileName, companyFolderName, company.company_logo);
                    }
                    #endregion

                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Company");
                    result.Id = company.company_id;
                }
                else
                {
                    var companyInfo = db.USP_Create_Company_user_SA(company.time_zone, company.business_name, company.country_code, company.phone,
                            company.address, company.city, company.zip, company.state_id, userName, loginID,
                            mobile, emailID, password, gender, company.licence_no, company.company_logo);
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Company");
                    result.MessageType = MessageType.Success;
                    ///result.Id = companyInfo.FirstOrDefault().company_id;
                    #region FolderCreation
                    //string company_folder_name = companyInfo.FirstOrDefault().company_folder_name;
                    //string company_folder_name = "COMPANY_73_20180719/";
                    //////AWSUtil.FolderCreation(AWSProfileName, company_folder_name);
                    #endregion

                    #region FileUpload
                    if (company_logo != null)
                    {
                       // String companyFolderName = company_folder_name.Replace("/", "");
                    //    AWSUtil.UploadFile(company_logo.InputStream, AWSProfileName,  companyFolderName, company.company_logo);
                    }
                    #endregion


                    //#region SendMail
                    //string EMAIL_TEMPALTE_FOLDER = STUtil.GetWebConfigValue("EMAIL_TEMPALTE_FOLDER");
                    //var emailTempRecord = db.email_template.AsEnumerable().Where(x => x.email_template_type_id == 5).OrderByDescending(x => x.email_template_id).FirstOrDefault();//5 - ACCOUNT_ACTIVATED
                    //if (emailTempRecord != null)
                    //{
                    //    ImagePath = STUtil.GetWebConfigValue("ImagePath");
                    //    EMAIL_TEMPALTE_FOLDER = STUtil.GetWebConfigValue("EMAIL_TEMPALTE_FOLDER");
                    //    string APPLICATION_URL = STUtil.GetWebConfigValue("APPLICATION_URL");
                    //    String templateName = ImagePath + EMAIL_TEMPALTE_FOLDER + "/" + emailTempRecord.email_template_file_name;
                    //    var filePath = (new WebClient()).DownloadString(templateName);
                    //    //filePath = filePath.Replace("{UserName}", companyInfo.user_name);
                    //    //filePath = filePath.Replace("{business_name}", companyInfo.business_name);
                    //    filePath = filePath.Replace("{LOGIN_ID}", loginID);
                    //    filePath = filePath.Replace("{PASSWORD}", password);
                    //    filePath = filePath.Replace("{EMAIL_ID}", emailID);
                    //    filePath = filePath.Replace("{APPLICATION_URL}", APPLICATION_URL);
                    //    STUtil.SendMail(emailID, "Welcome to SnagTick: " + loginID, filePath, null);
                    //}
                    //#endregion
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return result;
        }
        public role GetRoleNameByCompanyId(int role_id, string role_name,int company_id)
        {
            return role_id > 0 ?
                db.roles.AsEnumerable().Where(x => x.role_name.ToLower() == role_name.ToLower() && x.role_id != role_id && x.company_id == company_id).FirstOrDefault()
                : db.roles.AsEnumerable().Where(x => x.role_name.ToLower() == role_name.ToLower() && x.company_id == SessionUtil.GetCompanyID()).FirstOrDefault();
        }
        public Result PostUserEdit(user user)
        {
            try
            {
                if (user.user_id>0)
                {
                    string password = db.USP_GetUserPassword(user.user_id, user.company_id).ToList().FirstOrDefault().password;
                    var isUserEdit = db.USP_CreateUser(user.user_id, user.user_name, user.login_id, user.email_id, user.mobile, password,
                         user.gender, user.user_photo, user.parent_user_id, user.role_bit, user.company_id, user.created_by, user.role_id, user.create_work_order_access_id, user.view_work_order_access_id, user.is_change_requester, user.is_service_provider, user.is_active);
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "User");
                }
                else
                {
                    var pass = STUtil.GetRandomPasswordNumber(6);
                    int created_by = SessionUtil.GetUserID();
                    var isUserEdit = db.USP_CreateUser(user.user_id, user.user_name, user.login_id, user.email_id, user.mobile, pass,
                         user.gender, user.user_photo, user.parent_user_id, user.role_bit, user.company_id, created_by, user.role_id, user.create_work_order_access_id, user.view_work_order_access_id, user.is_change_requester, user.is_service_provider, true);
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "User");
                }
                
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return result;
        }

        public Result GetUsersList(int id)
        {
            result = new Result();
            try
            {
                result.Object = (from li in db.users.AsEnumerable()
                                 where li.is_active && li.company_id == id
                                 select new
                                 {
                                     user_id = li.user_id,
                                     user_name = li.user_name,
                                     email_id = li.email_id,
                                     mobile = li.mobile,
                                     gender = li.gender,
                                     is_active = li.is_active
                                 }).ToList();
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return result;
        }

        #region COMPANY HOLIDAY
        public Result PostCompanyHolidayCreate(company_holiday company_Holiday)
        {
            try
            {

                if (company_Holiday.company_holiday_id > 0)
                {
                    
                    db.Entry(company_Holiday).State = System.Data.Entity.EntityState.Modified;
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Company Holiday");
                }
                else
                {
                    db.company_holiday.Add(company_Holiday);
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Company Holiday");
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
        public company_holiday GetHolidayDateByHolidayId(int companyholidayId, DateTime companyholidayDate) 
        {
            return companyholidayId > 0 ?
                db.company_holiday.AsEnumerable().Where(x => x.holiday_date == companyholidayDate && x.company_holiday_id != companyholidayId && x.company_id == SessionUtil.GetCompanyID()).FirstOrDefault()
                : db.company_holiday.AsEnumerable().Where(x => x.holiday_date == companyholidayDate && x.company_id == SessionUtil.GetCompanyID()).FirstOrDefault();
        }
        #endregion
    }


    #endregion



}