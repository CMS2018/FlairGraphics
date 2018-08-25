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
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        public BaseEntities db = null;
         
        public void UploadFile(string company_folder_name , HttpPostedFileBase file,string GenFileName="")
        {
            if (file != null)
            {
                string fileName = string.Empty;
                String companyFolderName = company_folder_name.Replace("/", "");
                 GenFileName =string.IsNullOrEmpty(GenFileName)? STUtil.GetTodayDate().ToString("yyyyMMdd") + "_" + SessionUtil.GetCompanyID().ToString() + "_" + Path.GetFileName(file.FileName).Replace(" ", "_"): GenFileName;
                var path = Path.Combine(Server.MapPath("~/Files/" + companyFolderName), GenFileName);
                file.SaveAs(path);
            }

        }


        public BaseModel BaseModel { get; set; }
        public BaseController()
        {
            db = new BaseEntities();
            this.BaseModel = new BaseModel();
            this.BaseModel.ControllerName = this.ToString().Split('.')[this.ToString().Split('.').Length - 1];
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //filterContext.ActionDescriptor.ActionName.ToUpper().Equals("INDEX") &&

            String ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToUpper();
            String ActionName = filterContext.ActionDescriptor.ActionName.ToUpper();



            List<TreeNode> AllList = Session[UserInfo.MenuList.ToString()] as List<TreeNode>;
            if (AllList!=null)
            {
                var menuData = AllList.AsEnumerable().Where(x => (x.ControllerName ?? "").Trim().ToUpper() == ControllerName && (x.ActionName?? "").Trim().ToUpper() == ActionName).FirstOrDefault();
                string sludge = menuData == null ? "" : menuData.Sludge;
                string pageTitle = menuData == null ? "" : menuData.PageTitle;
                if (menuData != null)
                {
                    STUtil.SetSessionValue(UserInfo.sludge.ToString(), sludge);
                    STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), pageTitle);
                }
            }
           
            if (STUtil.ListControllerExcluded().Contains(ControllerName))
            {
                if (ControllerName == "ACCOUNT" && ActionName == "LOGIN" && STUtil.IsAuthenticated())
                {

                    filterContext.Result = new RedirectResult("/DashBoard/Index/");
                }

                return;
            }
            else
            {
                if (STUtil.GetSessionValue(UserInfo.UserID.ToString()) == "")
                {
                    filterContext.Result = null;
                    filterContext.Result = new RedirectResult("/Account/Login/");
                    return;
                }
                if (!STUtil.CheckAuthentication(filterContext))
                {
                    filterContext.Result = null;
                    filterContext.Result = new RedirectResult("/Home/AccessDenied/");
                    return;
                }

                //if (STUtil.IsAuthenticated() && STUtil.GetSessionValue(UserInfo.IsCompanySetup.ToString()) == "0" && ActionName != "COMPANYINDEX" && ActionName != "COMPANYCREATEEDIT")
                if (STUtil.IsAuthenticated()  && STUtil.GetSessionValue(UserInfo.IsCompanySetup.ToString()) == "0" && ActionName != "COMPANYACCOUNT" )
                {
                    filterContext.Result = null;
                    filterContext.Result = new RedirectResult("/Settings/CompanyAccount/");
                    return;
                }
                return;
            }
        }
        protected override void Dispose(bool disposing)
            {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}