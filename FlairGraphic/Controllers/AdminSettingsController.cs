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
    public class AdminSettingsController : BaseController
    {
        private Result result;
        private JobTypeUtill jobTypeUtill;
        private JobStatusUtill jobStatusUtill;
        private PackageTypeUtill packageTypeUtill;
        private PaperTypeUtill paperTypeUtill;
        private PaperSubTypeUtill paperSubTypeUtill;
        private PaymentStatusUtill paymentStatusUtill;
        private CurrencyUtil currencyUtil;
        private LeminationTypeUtill leminationTypeUtill;
        public AdminSettingsController()
        {
            result = new Result();
            jobTypeUtill = new JobTypeUtill();
            jobStatusUtill = new JobStatusUtill();
            packageTypeUtill = new PackageTypeUtill();
            paperTypeUtill = new PaperTypeUtill();
            paperSubTypeUtill = new PaperSubTypeUtill();
            paymentStatusUtill = new PaymentStatusUtill();
            currencyUtil = new CurrencyUtil();
            leminationTypeUtill = new LeminationTypeUtill();
        }
        public ActionResult Index()
        {
            return View();
        }

        #region Lemination Type
        public ActionResult LeminationType(string id)
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Lemination Type");
            lemination_type c = new lemination_type();
            if (id != null && id != "")
            {
                c = db.lemination_type.Find(Convert.ToInt32(id));
            }
            return View(c);
        }
        public ActionResult LeminationTypeList(int id)
        {
            var list = db.lemination_type.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            lemination_type_name = li.lemination_type_name,
                            lemination_type_id = li.lemination_type_id,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        public ActionResult LeminationTypeEdit(string id)
        {
            var data = (from li in db.lemination_type.AsEnumerable()
                        where li.lemination_type_id == Convert.ToInt32(id)
                        select new
                        {
                            lemination_type_name = li.lemination_type_name,
                            lemination_type_id = li.lemination_type_id,
                            is_active = li.is_active,
                        }).FirstOrDefault();

            return Json(data);
        }
        [HttpPost]
        public ActionResult LeminationTypeCreateEdit(lemination_type lemination_type)
        {
            result = leminationTypeUtill.LeminationTypeCreateEdit(lemination_type);
            ViewBag.Title = lemination_type == null ? "LeminationType Create" : "LeminationType Edit";
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            return Json(result);
        }
        #endregion

        #region Job Type
        public ActionResult JobType(string id)
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Job Type");
            job_type c = new job_type();
            if (id != null && id != "")
            {
                c = db.job_type.Find(Convert.ToInt32(id));
            }
            return View(c);
        }
        public ActionResult JobTypeList(int id)
        {
            var list = db.job_type.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            job_type_name = li.job_type_name,
                            job_type_id = li.job_type_id,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        public ActionResult JobTypeEdit(string id)
        {
            var data = (from li in db.job_type.AsEnumerable()
                        where li.job_type_id == Convert.ToInt32(id)
                        select new
                        {
                            job_type_name = li.job_type_name,
                            job_type_id = li.job_type_id,
                            is_active = li.is_active,
                        }).FirstOrDefault();
            return Json(data);
        }
        public ActionResult JobTypeCreateEdit(job_type job_type)
        {
            result = jobTypeUtill.JobTypeCreateEdit(job_type);
            ViewBag.Title = job_type == null ? "Job Create" : "Job Edit";
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            return Json(result);
        }
        #endregion

        #region Job Status
        public ActionResult jobStatus(string id)
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Job Status");
            job_status c = new job_status();
            if (id != null && id != "")
            {
                c = db.job_status.Find(Convert.ToInt32(id));
            }
            return View(c);
        }
        public ActionResult JobStatusList(int id)
        {
            var list = db.job_status.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            job_status_name = li.job_status_name,
                            job_status_id = li.job_status_id,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        public ActionResult JobStatusEdit(string id)
        {
            var data = (from li in db.job_status.AsEnumerable()
                        where li.job_status_id == Convert.ToInt32(id)
                        select new
                        {
                            job_status_name = li.job_status_name,
                            job_status_id = li.job_status_id,
                            is_active = li.is_active,
                        }).FirstOrDefault();
            return Json(data);
        }
        public ActionResult JobStatusCreateEdit(job_status job_status)
        {
            result = jobStatusUtill.JobStatusCreateEdit(job_status);
            ViewBag.Title = job_status == null ? "Job Status Create" : "Job Status Edit";
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            return Json(result);
        }
        #endregion

        #region Package Type
        public ActionResult PackageType(string id)
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Package Type");
            package_type c = new package_type();
            if (id != null && id != "")
            {
                c = db.package_type.Find(Convert.ToInt32(id));
            }
            return View(c);
        }
        public ActionResult PackageTypeList(int id)
        {
            var list = db.package_type.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            package_type_name = li.package_type_name,
                            package_type_id = li.package_type_id,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        public ActionResult PackageTypeEdit(string id)
        {
            var data = (from li in db.package_type.AsEnumerable()
                        where li.package_type_id == Convert.ToInt32(id)
                        select new
                        {
                            package_type_name = li.package_type_name,
                            package_type_id = li.package_type_id,
                            is_active = li.is_active,
                        }).FirstOrDefault();
            return Json(data);
        }
        public ActionResult PackageTypeCreateEdit(package_type package_type)
        {
            result = packageTypeUtill.PackageTypeCreateEdit(package_type);
            ViewBag.Title = package_type == null ? "Package Type Create" : "Package Type Edit";
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            return Json(result);
        }
        #endregion

        #region Paper Type
        public ActionResult PaperType(string id)
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Paper Type");
            paper_type c = new paper_type();
            if (id != null && id != "")
            {
                c = db.paper_type.Find(Convert.ToInt32(id));
            }
            return View(c);
        }
        public ActionResult PaperTypeList(int id)
        {
            var list = db.paper_type.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            paper_type_name = li.paper_type_name,
                            paper_type_id = li.paper_type_id,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        public ActionResult PaperTypeEdit(string id)
        {
            var data = (from li in db.paper_type.AsEnumerable()
                        where li.paper_type_id == Convert.ToInt32(id)
                        select new
                        {
                            paper_type_name = li.paper_type_name,
                            paper_type_id = li.paper_type_id,
                            is_active = li.is_active,
                        }).FirstOrDefault();
            return Json(data);
        }
        public ActionResult PaperTypeCreateEdit(paper_type paper_type)
        {
            result = paperTypeUtill.PaperTypeCreateEdit(paper_type);
            ViewBag.Title = paper_type == null ? "Paper Type Create" : "Paper Type Edit";
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            return Json(result);
        }
        #endregion

        #region Paper Sub Type
        public ActionResult PaperSubType(string id)
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Paper Sub Type");
            paper_sub_type c = new paper_sub_type();
            if (id != null && id != "")
            {
                c = db.paper_sub_type.Find(Convert.ToInt32(id));
            }
            return View(c);
        }
        public ActionResult PaperSubTypeList(int id)
        {
            var list = db.paper_sub_type.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            paper_sub_type_name = li.paper_sub_type_name,
                            paper_sub_type_id = li.paper_sub_type_id,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        public ActionResult PaperSubTypeEdit(string id)
        {
            var data = (from li in db.paper_sub_type.AsEnumerable()
                        where li.paper_sub_type_id == Convert.ToInt32(id)
                        select new
                        {
                            paper_sub_type_name = li.paper_sub_type_name,
                            paper_sub_type_id = li.paper_sub_type_id,
                            is_active = li.is_active,
                        }).FirstOrDefault();
            return Json(data);
        }
        public ActionResult PaperSubTypeCreateEdit(paper_sub_type paper_sub_type)
        {
            result = paperSubTypeUtill.PaperSubTypeCreateEdit(paper_sub_type);
            ViewBag.Title = paper_sub_type == null ? "Paper Sub Type Create" : "Paper Sub Type Edit";
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            return Json(result);
        }
        #endregion

        #region Payment Status
        public ActionResult PaymentStatus(string id)
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Payment Status");
            payment_status c = new payment_status();
            if (id != null && id != "")
            {
                c = db.payment_status.Find(Convert.ToInt32(id));
            }
            return View(c);
        }
        public ActionResult PaymentStatusList(int id)
        {
            var list = db.payment_status.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            payment_status_name = li.payment_status_name,
                            payment_status_id = li.payment_status_id,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        public ActionResult PaymentStatusEdit(string id)
        {
            var data = (from li in db.payment_status.AsEnumerable()
                        where li.payment_status_id == Convert.ToInt32(id)
                        select new
                        {
                            payment_status_name = li.payment_status_name,
                            payment_status_id = li.payment_status_id,
                            is_active = li.is_active,
                        }).FirstOrDefault();
            return Json(data);
        }
        public ActionResult PaymentStatusCreateEdit(payment_status payment_status)
        {
            result = paymentStatusUtill.PaymentStatusCreateEdit(payment_status);
            ViewBag.Title = payment_status == null ? "Payment Status Create" : "Payment Status Edit";
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            return Json(result);
        }
        #endregion
    }
}