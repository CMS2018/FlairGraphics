using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlairGraphic.Base.Models;
using FlairGraphic.Models;

namespace FlairGraphic.Base.Models
{
    #region Classes & Enum
    public class RoleActionModel
    {
        public role_action RoleAction { get; set; }
        public IList<role_action> ListRoleActionAssigned { get; set; }
        public RoleActionModel()
        {
            RoleAction = new role_action();
            ListRoleActionAssigned = new List<role_action>();
        }
    }
    public class ControllerAction
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool IsAssigned { get; set; }
    }

    public class AssignController
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool IsAssigned { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
    }

    public enum Role
    {
        SuperAdmin = 1,
        Admin = 2,
        Manager = 4,
        Client = 8,
        Operator = 16,
    }

    public enum ThemeColor
    {
        default_them = 1,
        green = 2,
        red = 3,
        blue = 4,
        purple = 5,
        megna = 6,
        default_dark = 7,
        green_dark = 8,
        blue_dark = 9,
        purple_dark = 10,
        megna_dark = 11,
    }

    public enum UserInfo
    {
        UserID, LoginID, FullName, EmailID, Mobile, RoleName, Last_Login_Date, UserPhoto, Gender, RoleID, RoleBit, CompanyID, CompanyName, CompanyFolderName,
        IsCompanySetup, IsCompanyAddUpdate,
        time_zone, currency_id, currency_name, currency_symbol,
        date_format_id, date_format_name, date_format_code_csharp, date_format_code_js,
        time_format_id, time_format_name, time_format_code_csharp, time_format_code_js,
        application_type_id, application_type_name,
        SERVER_TIME_ZONE,
        MenuList,
        SuperAdmin,
        //, Admin, CRMExecutive, Account, Manager, Agent
        system_company_folder, default_contact_id,
        CompanyLogo,
        company_doc_file_name,
        //is_account_access, is_account_add_payment,   dld_payment_per,    admin_fees_price,    is_property_edit,

        theme_color, sludge, pageTitle,
        create_work_order_access_id,
        view_work_order_access_id,
        is_change_requester,
        is_service_provider
    }

    #endregion
    #region Business
    public static class RoleUtil
    {
        private static FLAIR_GRAPHICSEntities db = new FLAIR_GRAPHICSEntities();
        private static Result result;


        public static List<string> GetRoles(Int64 RoleBit)
        {
            List<Int64> authlevels = new List<Int64>();
            List<string> roles = new List<string>();
            try
            {
                string roleBit = RoleBit == 0 ? STUtil.GetSessionValue(UserInfo.RoleBit.ToString()) : RoleBit.ToString();
                if (!string.IsNullOrEmpty(roleBit))
                {
                    Int64 Value = Convert.ToInt64(roleBit);
                    Int64 result = 0;
                    for (Int64 i = 0; Value >= (Int64)Math.Pow(2, i); i++)
                    {
                        result = Value & (Int64)Math.Pow(2, i);
                        authlevels.Add(result);
                    }

                    foreach (var item in authlevels)
                    {
                        var au = (from R in db.roles
                                  where R.role_bit == item
                                  select new
                                  {
                                      ROLE_BIT = R.role_bit,
                                      ROLE_NAME = R.role_name.Trim().ToUpper()
                                  }
                                  ).FirstOrDefault();

                        if (au != null)
                        {
                            roles.Add(au.ROLE_NAME);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return roles;
        }

        public static bool IsInRole(Role role, Int64 RoleBit = 0, Boolean IsGetFromSession = true)
        {
            bool isRole = false;
            string roleValue = string.Empty;
            roleValue = IsGetFromSession ? STUtil.GetSessionValue(role.ToString()) : string.Empty;
            if (!string.IsNullOrEmpty(roleValue))
            {
                isRole = Convert.ToString(roleValue).ToUpper() == "TRUE" ? true : false;
                return isRole;
            }
            else
            {
                //return GetRoles(RoleBit).Where(x => x.Contains(role.ToString().ToUpper())).Count() > 0 ? true : false;
                var r = GetRoles(RoleBit);
                return GetRoles(RoleBit).Where(x => x == (role.ToString().ToUpper())).Count() > 0 ? true : false;
            }
        }

        public static List<SelectListItem> GetPublicRole()
        {
            return (from c in db.roles.AsEnumerable()
                    where c.is_public
                    orderby c.role_name
                    select new SelectListItem
                    {
                        Value = c.role_bit.ToString(),
                        Text = c.role_name
                    }).ToList();
        }
        public static List<SelectListItem> GetParentRoleByCompany(int company_id = 0)
        {
            db = new FLAIR_GRAPHICSEntities();
            var list = (from c in db.roles.AsEnumerable()
                        where c.company_id == company_id
                        orderby c.role_name
                        select new SelectListItem
                        {
                            Value = c.role_id.ToString(),
                            Text = c.role_name
                        }).ToList();
            return list;
        }
        public static List<SelectListItem> GetParentRoleBitByCompany(int company_id = 0)
        {
            db = new FLAIR_GRAPHICSEntities();
            var list = (from c in db.roles.AsEnumerable()
                        where c.company_id == company_id && c.is_public == true
                        orderby c.role_name
                        select new SelectListItem
                        {
                            Value = c.role_bit.ToString(),
                            Text = c.role_name
                        }).ToList();
            return list;
        }
        public static List<TreeNode> GetMenusOfRoleId(int roleId, int CompanyID)
        {
            var list = db.GetMenu(roleId, CompanyID).OrderBy(x => x.sequence_order).ToList();
            List<TreeNode> list1 = new List<TreeNode>();

            for (int i = 0; i < list.Count(); i++)
            {
                GetMenu_Result item = list[i] as GetMenu_Result;
                list1.Add(new TreeNode()
                {
                    Id = Convert.ToString(item.menu_id),
                    ParentId = Convert.ToString(item.menu_parent_id),
                    Name = item.menu_name,
                    Icon = item.icon,
                    Sequence = item.sequence_order,
                    Sludge = item.sludge_name,
                    PageTitle = item.title_name,
                    ControllerName = item.controller_name,
                    ActionName = item.action_name,
                    Url = VirtualPathUtility.ToAbsolute(string.Format("~/{0}/{1}/{2}", item.controller_name, item.action_name, string.IsNullOrEmpty(item.param_id) ? "" : item.param_id)),
                });
            }

            //var list = (from me in db.menus.AsEnumerable()
            //            join rm in db.role_menu.AsEnumerable() on me.menu_id equals rm.menu_id 
            //            where rm.role_id == roleId
            //            select new TreeNode
            //            {
            //                Id = Convert.ToString(me.menu_id),
            //                ParentId = Convert.ToString(me.menu_parent_id),
            //                Name = me.menu_name,
            //                Url = string.Format("/{0}/{1}/{2}",me.controller_name, me.action_name, string.IsNullOrEmpty(me.id) ? "" : me.id)
            //            }).ToList();

            return list1;
        }

        public static String CheckUserFrofile(String LoginID, String PWD, Int32 userID = 0)
        {
            String result = "Invalid Login ID or Password ";

            var userInfo = (from U in db.USP_Login(LoginID, PWD, true)
                            select new
                            {
                                UserID = U.user_id,
                                IsActive = U.is_active,
                                CompanyActive = U.CompanyActive,
                                FullName = U.user_name,
                                LoginID = U.email_id,
                                Email = U.email_id,
                                gender = U.gender,
                                Mobile = U.mobile,
                                RoleBit = U.role_bit,
                                RoleName = U.role_name,
                                RoleID = U.role_id,
                                CompanyID = U.company_id,
                                CompanyName = U.business_name,
                                CompanyFolderName = U.company_folder_name,
                                company_logo = U.company_logo,

                                time_zone = U.time_zone,
                                currency_id = (int)U.currency_id,
                                currency_name = U.currency_name,
                                currency_symbol = U.currency_symbol,
                                date_format_id = (int)U.date_format_id,
                                date_format_name = U.date_format_name,
                                date_format_code_csharp = U.date_format_code_csharp,
                                date_format_code_js = U.date_format_code_js,
                                time_format_id = (int)U.time_format_id,
                                time_format_name = U.time_format_name,
                                time_format_code_csharp = U.time_format_code_csharp,
                                time_format_code_js = U.time_format_code_js,

                                Photo = U.user_photo,

                                last_login_date = U.last_login_date,
                                password_failed_attempt = U.password_failed_attempt,
                                is_account_locked = U.is_account_locked,

                                system_company_folder = U.system_company_folder,
                                company_doc_file_name = U.company_doc_file_name,

                                create_work_order_access_id = U.create_work_order_access_id,
                                view_work_order_access_id = U.view_work_order_access_id,
                                is_change_requester = U.is_change_requester,
                                is_service_provider = U.is_service_provider,

                                theme_color = U.theme_color,

                            }).FirstOrDefault();
            #region AutoLogin By UserID


            if (userID > 0)
            {
                userInfo = (from U in db.users.AsEnumerable()
                            join C in db.companies.AsEnumerable() on U.company_id equals C.company_id
                            join R in db.roles.AsEnumerable() on U.role_bit equals R.role_bit
                            join CU in db.currencies.AsEnumerable() on C.currency_id equals CU.currency_id into curr
                            from CU in curr.DefaultIfEmpty()
                            join DF in db.date_format.AsEnumerable() on C.date_format_id equals DF.date_format_id into DateFo
                            from DF in DateFo.DefaultIfEmpty()
                            join TF in db.time_format.AsEnumerable() on C.time_format_id equals TF.time_format_id into TimeFo
                            from TF in TimeFo.DefaultIfEmpty()
                            where U.user_id == userID
                            select new
                            {
                                UserID = U.user_id,
                                IsActive = U.is_active,
                                CompanyActive = C.is_active,
                                FullName = U.user_name,
                                LoginID = U.email_id,
                                Email = U.email_id,
                                gender = U.gender,
                                Mobile = U.mobile,
                                RoleBit = U.role_bit,
                                RoleName = R.role_name,
                                RoleID = R.role_id,

                                CompanyID = U.company_id,
                                CompanyName = C.business_name,
                                CompanyFolderName = C.company_folder_name,
                                company_logo = C.company_logo,

                                time_zone = C.time_zone,
                                currency_id = CU != null ? CU.currency_id : 0,
                                currency_name = CU != null ? CU.currency_name : "",
                                currency_symbol = CU != null ? CU.currency_symbol : "",

                                date_format_id = DF != null ? DF.date_format_id : 0,
                                date_format_name = DF != null ? DF.date_format_name : "",
                                date_format_code_csharp = DF != null ? DF.date_format_code_csharp : "",
                                date_format_code_js = DF != null ? DF.date_format_code_js : "",

                                time_format_id = TF != null ? TF.time_format_id : 0,
                                time_format_name = TF != null ? TF.time_format_name : "",
                                time_format_code_csharp = TF != null ? TF.time_format_code_csharp : "",
                                time_format_code_js = TF != null ? TF.time_format_code_js : "",

                                Photo = U.user_photo,

                                last_login_date = U.last_login_date,
                                password_failed_attempt = U.password_failed_attempt,
                                is_account_locked = U.is_account_locked,

                                system_company_folder = C.system_company_folder,
                                company_doc_file_name = C.company_doc_file_name,

                                create_work_order_access_id = U.create_work_order_access_id,
                                view_work_order_access_id = U.view_work_order_access_id,
                                is_change_requester = U.is_change_requester,
                                is_service_provider = U.is_service_provider,

                                theme_color = U.theme_color_id == null ? C.theme_color.theme_css : U.theme_color.theme_css,

                            }).FirstOrDefault();
            }
            #endregion
            if (userInfo == null)
            {
                var userRecord = (from U in db.users
                                  join C in db.companies on U.company_id equals C.company_id
                                  where (U.login_id == LoginID)
                                  select U).FirstOrDefault();
                if (userRecord != null)
                {
                    result = STUtil.UserLoginPolicy(userRecord.user_id, true);
                }
            }
            try
            {
                if (userInfo != null)
                {

                    if (userInfo.is_account_locked)
                    {
                        result = "Your acount has been locked. Due to multiple invalid password ! Please contact to Admin";
                    }
                    else
                    {
                        if (userInfo.IsActive)
                        {
                            STUtil.SetSessionValue(UserInfo.UserID.ToString(), Convert.ToString(userInfo.UserID));
                            STUtil.SetSessionValue(UserInfo.FullName.ToString(), Convert.ToString(userInfo.FullName));

                            STUtil.SetSessionValue(UserInfo.LoginID.ToString(), Convert.ToString(userInfo.LoginID));
                            STUtil.SetSessionValue(UserInfo.EmailID.ToString(), Convert.ToString(userInfo.Email));
                            STUtil.SetSessionValue(UserInfo.Mobile.ToString(), Convert.ToString(userInfo.Mobile));
                            STUtil.SetSessionValue(UserInfo.RoleBit.ToString(), Convert.ToString(userInfo.RoleBit));
                            STUtil.SetSessionValue(UserInfo.RoleID.ToString(), Convert.ToString(userInfo.RoleID));
                            STUtil.SetSessionValue(UserInfo.RoleName.ToString(), Convert.ToString(userInfo.RoleName));
                            STUtil.SetSessionValue(UserInfo.CompanyID.ToString(), Convert.ToString(userInfo.CompanyID));

                            STUtil.SetSessionValue(UserInfo.CompanyName.ToString(), Convert.ToString(userInfo.CompanyName));
                            STUtil.SetSessionValue(UserInfo.CompanyFolderName.ToString(), Convert.ToString(userInfo.CompanyFolderName));
                            STUtil.SetSessionValue(UserInfo.CompanyLogo.ToString(), Convert.ToString(userInfo.company_logo));

                            STUtil.SetSessionValue(UserInfo.UserPhoto.ToString(), Convert.ToString(userInfo.Photo));
                            STUtil.SetSessionValue(UserInfo.Gender.ToString(), Convert.ToString(userInfo.gender.ToUpper()));

                            STUtil.SetSessionValue(UserInfo.time_zone.ToString(), Convert.ToString(userInfo.time_zone));

                            STUtil.SetSessionValue(UserInfo.currency_id.ToString(), Convert.ToString(userInfo.currency_id));
                            STUtil.SetSessionValue(UserInfo.currency_name.ToString(), Convert.ToString(userInfo.currency_name));
                            STUtil.SetSessionValue(UserInfo.currency_symbol.ToString(), Convert.ToString(userInfo.currency_symbol));

                            STUtil.SetSessionValue(UserInfo.date_format_id.ToString(), Convert.ToString(userInfo.date_format_id));
                            STUtil.SetSessionValue(UserInfo.date_format_name.ToString(), Convert.ToString(userInfo.date_format_name));
                            STUtil.SetSessionValue(UserInfo.date_format_code_csharp.ToString(), Convert.ToString(userInfo.date_format_code_csharp));
                            STUtil.SetSessionValue(UserInfo.date_format_code_js.ToString(), Convert.ToString(userInfo.date_format_code_js));

                            STUtil.SetSessionValue(UserInfo.time_format_id.ToString(), Convert.ToString(userInfo.time_format_id));
                            STUtil.SetSessionValue(UserInfo.time_format_name.ToString(), Convert.ToString(userInfo.time_format_name));
                            STUtil.SetSessionValue(UserInfo.time_format_code_csharp.ToString(), Convert.ToString(userInfo.time_format_code_csharp));
                            STUtil.SetSessionValue(UserInfo.time_format_code_js.ToString(), Convert.ToString(userInfo.time_format_code_js));

                            STUtil.SetSessionValue(UserInfo.system_company_folder.ToString(), Convert.ToString(userInfo.system_company_folder));

                            STUtil.SetSessionValue(UserInfo.company_doc_file_name.ToString(), Convert.ToString(userInfo.company_doc_file_name));

                            STUtil.SetSessionValue(UserInfo.create_work_order_access_id.ToString(), Convert.ToString(userInfo.create_work_order_access_id));
                            STUtil.SetSessionValue(UserInfo.view_work_order_access_id.ToString(), Convert.ToString(userInfo.view_work_order_access_id));
                            STUtil.SetSessionValue(UserInfo.is_change_requester.ToString(), Convert.ToString(userInfo.is_change_requester));
                            STUtil.SetSessionValue(UserInfo.is_service_provider.ToString(), Convert.ToString(userInfo.is_service_provider));

                            STUtil.SetSessionValue(UserInfo.theme_color.ToString(), Convert.ToString(userInfo.theme_color));

                            STUtil.SetSessionValue(UserInfo.SERVER_TIME_ZONE.ToString(), STUtil.GetWebConfigValue("SERVER_TIME_ZONE"));

                            STUtil.SetSessionValue(UserInfo.SuperAdmin.ToString(), Convert.ToString(RoleUtil.IsInRole(Role.SuperAdmin, userInfo.RoleBit)));

                            STUtil.SetSessionValue(UserInfo.IsCompanySetup.ToString(), "1");
                            //STUtil.SetSessionValue(UserInfo.IsCompanySetup.ToString(), string.IsNullOrEmpty(userInfo.time_zone) ? "0" : "1");
                            //STUtil.SetSessionValue(UserInfo.IsCompanySetup.ToString(), string.IsNullOrEmpty(userInfo.currency_name) ? "0" : "1");
                            //STUtil.SetSessionValue(UserInfo.IsCompanySetup.ToString(), string.IsNullOrEmpty(userInfo.time_format_name) ? "0" : "1");
                            //STUtil.SetSessionValue(UserInfo.IsCompanySetup.ToString(), string.IsNullOrEmpty(userInfo.date_format_name) ? "0" : "1");
                            //STUtil.SetSessionValue(UserInfo.IsCompanySetup.ToString(), string.IsNullOrEmpty(userInfo.system_company_folder) ? "0" : "1");
                            //STUtil.SetSessionValue(UserInfo.IsCompanySetup.ToString(), userInfo.default_contact_id == null ? "0" : "1");

                            STUtil.SetSessionValue(UserInfo.IsCompanyAddUpdate.ToString(), "1");

                            //STUtil.SetSessionValue(UserInfo.Admin.ToString(), Convert.ToString(RoleUtil.IsInRole(Role.Admin, userInfo.RoleBit)));
                            //STUtil.SetSessionValue(UserInfo.CRMExecutive.ToString(), Convert.ToString(RoleUtil.IsInRole(Role.CRMExecutive, userInfo.RoleBit)));
                            //STUtil.SetSessionValue(UserInfo.Account.ToString(), Convert.ToString(RoleUtil.IsInRole(Role.Account, userInfo.RoleBit)));
                            //STUtil.SetSessionValue(UserInfo.Manager.ToString(), Convert.ToString(RoleUtil.IsInRole(Role.Manager, userInfo.RoleBit)));
                            //STUtil.SetSessionValue(UserInfo.Agent.ToString(), Convert.ToString(RoleUtil.IsInRole(Role.Agent, userInfo.RoleBit)));


                            STUtil.UserLoginPolicy(userInfo.UserID, false);
                            result = "PASS";

                        }
                        else
                        {
                            result = !userInfo.IsActive ? "Your are Inactive! Please contact to Admin" : String.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
                //// ExceptionLogging.SendErrorToText(ex);  
            }

            return result;
        }

        public static Result GetRoleList(string id = "")
        {
            try
            {
                result = new Result();
                string[] info = id.Split(',');
                if ((info[0]).Trim() == ("-1").Trim())
                {
                    int company_id = String.IsNullOrEmpty(info[1]) ? 0 : Convert.ToInt32(info[1]);
                    //result.Object = (from R in db.roles
                    //                 join R1 in db.roles on R.role_id equals R1.role_id into b_temp
                    //                 from b_value in b_temp.DefaultIfEmpty()
                    //                 where R.company_id == company_id
                    //                 select new
                    //                 {
                    //                     role_id = R.role_id,
                    //                     role_name = R.role_name,
                    //                     is_public = R.is_public == true ? "True" : "False",
                    //                     hierarchy_level = R.hierarchy_level,
                    //                     is_active = R.is_active,
                    //                     company_id = R.company_id,
                    //                     parent_id = R.parent_id,
                    //                     role_bit = R.role_bit,
                    //                     parent_role_name = b_value.role_name,
                    //                 });


                    result.Object = (from R in db.GetParentRole(company_id)
                                     select new
                                     {
                                         role_id = R.role_id,
                                         role_name = R.role_name,
                                         is_public = R.is_public == true ? "True" : "False",
                                         hierarchy_level = R.hierarchy_level,
                                         is_active = R.is_active,
                                         company_id = R.company_id,
                                         parent_id = R.parent_id,
                                         role_bit = R.role_bit,
                                         parent_role_name = R.role_name,
                                         Parent = R.Parent,
                                     }).ToList();
                }
                else
                {
                    result.Object = (from R in db.roles
                                     select new
                                     {
                                         role_id = R.role_id,
                                         role_name = R.role_name,
                                         role_bit = R.role_bit,

                                     }).ToList();
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return result;
        }
    }
    #endregion
}