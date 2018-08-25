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
    public class JobController : BaseController
    {
        private Result result;
        private JobUtil jobUtil;
        public JobController()
        {
            result = new Result();
            jobUtil = new JobUtil();
            db = new BaseEntities();
        }
        // GET: Job
        public ActionResult Index()
        {
            ViewBag.user_id = jobUtil.GetManager();
            return View();
        }
        public ActionResult JobList(string id)
        {
            var Company_Id = SessionUtil.GetCompanyID();
            var data = (from li in db.jobs.AsEnumerable()
                        where li.is_active
                        select new
                        {
                            job_id = li.job_id,
                            job_type_name = li.job_type.job_type_name,
                            client_name = li.user.user_name,
                            paper_type_name = li.paper_type.paper_type_name,
                            paper_sub_type_name = li.paper_sub_type.paper_sub_type_name,
                            lemination_type_name = li.lemination_type.lemination_type_name,
                            job_status_name = li.job_status.job_status_name,
                            quantity = li.quantity,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        public ActionResult JobAtachmentList(int id)
        {
            var list = (from li in db.job_attachment.AsEnumerable()
                        where li.job_id == id && li.is_active
                        select new
                        {
                            job_attachment_id = li.job_attachment_id,
                            job_attachment =AWSUtil.GetFileURL(li.job_attachment1),
                            user_name = li.user.user_name,
                            is_active = li.is_active,
                        }).ToList();
            return Json(list);
        }
        public ActionResult CreateEditJob(int id)
        {
            int client_id = 0;
            int job_id = 0;
            if (id > 0)
            {
                var job = db.jobs.Find(id);
                client_id = job != null ? job.client_id : 0;
                job_id = job != null ? job.job_id : 0;
            }
            ViewBag.client_id = client_id;
            ViewBag.id = job_id;
            return View();
        }
        public ActionResult LoadJobDetail(int id)
        {
            job obj = new job();
            if (id > 0)
            {
                obj = db.jobs.Find(id);
            }
            ViewBag.job_type_id = new SelectList(jobUtil.GetJobType(), "Value", "Text", obj != null ? obj.job_type_id : 0);
            ViewBag.job_status_id = new SelectList(jobUtil.GetJobStatus(), "Value", "Text", obj != null ? obj.job_status_id : 0);
            ViewBag.client_id = new SelectList(jobUtil.GetClient(), "Value", "Text", obj != null ? obj.client_id : 0);
            ViewBag.paper_type_id = new SelectList(jobUtil.GetPaperType(), "Value", "Text", obj != null ? obj.paper_type_id : 0);
            ViewBag.paper_sub_type_id = new SelectList(jobUtil.GetPaperSubType(), "Value", "Text", obj != null ? obj.paper_sub_type_id : 0);
            ViewBag.lemination_type_id = new SelectList(jobUtil.GetLeminationType(), "Value", "Text", obj != null ? obj.lemination_type_id : 0);
            return PartialView("_JobDetail", obj);
        }
        public ActionResult LoadJobAttachment(string id)
        {
            string[] info = id.Split(',');
            int jobId = 0;
            int ClientId = 0;
            if (info.Length == 2)
            {
                jobId = info[0] != "" ? Convert.ToInt32(info[0]) : jobId;
                ClientId = info[1] != "" ? Convert.ToInt32(info[1]) : ClientId;
            }
            ViewBag.jobId = jobId;
            ViewBag.ClientId = ClientId;
            return PartialView("_jobAttachment");
        }
        [HttpPost]
        public ActionResult CreteEditJob(job job, FormCollection frm)
        {
            try
            {
                string jobComment = "";
                if (job.job_id>0)
                {
                    jobComment = db.jobs.Find(job.job_id).comment;
                }
                result = jobUtil.PostCreteEditJob(job, jobComment);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return RedirectToAction("CreateEditJob", "Job", new { id = result.Id, Result = result.Message, MessageType = result.MessageType });
        }


        [HttpPost]
        public ActionResult CreateEditJobAttachment()
        {
            job_attachment job_attachment = new job_attachment();
            try
            {
                job_attachment.job_id = Convert.ToInt32(Request["job_id"] != "" ? Request["job_id"] : "0");
                job_attachment.client_id = Convert.ToInt32(Request["client_id"] != "" ? Request["client_id"] : "0");
                job_attachment.description = Request["description"] != null ? Request["description"] : "";
                job_attachment.is_active = Convert.ToBoolean(Request["is_active"]);
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase attachment = files.Count > 0 ? files[0] : null;

                if (job_attachment != null)
                {
                    string GenFileName = STUtil.GetTodayDate().ToString("yyyyMMdd") + "_" + Path.GetFileName(attachment.FileName).Replace(" ", "_");
                    String companyFolderName = SessionUtil.GetCompanyFolderName();
                    companyFolderName = companyFolderName.Replace("/", "");
                    UploadFile(companyFolderName,attachment, GenFileName);
                    job_attachment.job_attachment1 = GenFileName;
                }
                else
                {
                    job_attachment.job_attachment1 = "";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.MessageType = MessageType.Error;
            }
            result = jobUtil.PostCreateEditJobAttachment(job_attachment);
            return Json(result);
        }
        public ActionResult JobAttachmentDelete(int id)
        {
            result = jobUtil.DeleteJobAttachment(id);
            return Json(result);
        }
        public ActionResult AssignJob(job_assignment job_assignment, string id)
        {
            try
            {
                string[] userIds = id.Split(',');
                for (int i = 0; i < userIds.Length; i++)
                {
                    if (userIds[i] != "")
                    {
                        int userId = Convert.ToInt32(userIds[i]);
                        job_assignment.user_id = userId;
                        var jobAssign = db.job_assignment.AsEnumerable().Where(x => x.job_id == job_assignment.job_id && x.user_id == userId && x.is_active).FirstOrDefault();
                        if (jobAssign != null)
                        {
                            db.job_assignment.Remove(jobAssign);
                            db.SaveChanges();
                            result.Message = string.Format(BaseConst.MSG_SUCCESS_DELETE, "Job Assign");
                        }
                        db.job_assignment.Add(job_assignment);
                        db.SaveChanges();
                        result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Job Assign");
                    }
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return RedirectToAction("Index", new { Result = result.Message, MessageType = result.MessageType });
        }
        public ActionResult JobAssignUsersList(int id)
        {
            var Company_id = SessionUtil.GetCompanyID();
            var list = (from li in db.job_assignment.AsEnumerable()
                        where li.job_id == id && li.company_id == Company_id && li.is_active
                        select new
                        {
                            user_name = li.user.user_name,
                            is_active = li.is_active,
                            job_assignment_id = li.job_assignment_id,
                        }).ToList();
            return Json(list);
        }
        public ActionResult JobAssignDelete(int id)
        {
            result = jobUtil.DeleteJobAssign(id);
            return Json(result);
        }
    }
}