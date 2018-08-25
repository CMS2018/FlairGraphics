using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using FlairGraphic.Base.Models;

namespace FlairGraphic.Models
{
    public class JobUtil
    {
        private Result result;
        private BaseEntities db;
        public JobUtil()
        {
            this.result = new Result();
            this.db = new BaseEntities();
        }
        public IList<SelectListItem> GetJobType()
        {
            var CompanyId = SessionUtil.GetCompanyID();
            var list = (from li in db.job_type.AsEnumerable()
                        where li.company_id == CompanyId && li.is_active
                        select new SelectListItem
                        {
                            Text = li.job_type_name,
                            Value = li.job_type_id.ToString()
                        }).ToList();
            return list;
        }
        public IList<SelectListItem> GetJobStatus()
        {
            var CompanyId = SessionUtil.GetCompanyID();
            var list = (from li in db.job_status.AsEnumerable()
                        where li.company_id == CompanyId && li.is_active
                        select new SelectListItem
                        {
                            Text = li.job_status_name,
                            Value = li.job_status_id.ToString()
                        }).ToList();
            return list;
        }
        public IList<SelectListItem> GetClient()
        {
            var CompanyId = SessionUtil.GetCompanyID();
            var list = (from li in db.users.AsEnumerable()
                        where li.role_bit == Convert.ToInt32(Role.Client) && li.company_id == CompanyId && li.is_active
                        select new SelectListItem
                        {
                            Text = li.user_name,
                            Value = li.user_id.ToString()
                        }).ToList();
            return list;
        }
        public IList<SelectListItem> GetManager()
        {
            var CompanyId = SessionUtil.GetCompanyID();
            var list = (from li in db.users.AsEnumerable()
                        where li.role_bit == Convert.ToInt32(Role.Manager) && li.company_id == CompanyId && li.is_active
                        select new SelectListItem
                        {
                            Text = li.user_name,
                            Value = li.user_id.ToString()
                        }).ToList();
            return list;
        }
        public IList<SelectListItem> GetPaperType()
        {
            var CompanyId = SessionUtil.GetCompanyID();
            var list = (from li in db.paper_type.AsEnumerable()
                        where li.company_id == CompanyId && li.is_active
                        select new SelectListItem
                        {
                            Text = li.paper_type_name,
                            Value = li.paper_type_id.ToString()
                        }).ToList();
            return list;
        }
        public IList<SelectListItem> GetPaperSubType()
        {
            var CompanyId = SessionUtil.GetCompanyID();
            var list = (from li in db.paper_sub_type.AsEnumerable()
                        where li.company_id == CompanyId && li.is_active
                        select new SelectListItem
                        {
                            Text = li.paper_sub_type_name,
                            Value = li.paper_sub_type_id.ToString()
                        }).ToList();
            return list;
        }
        public IList<SelectListItem> GetLeminationType()
        {
            var CompanyId = SessionUtil.GetCompanyID();
            var list = (from li in db.lemination_type.AsEnumerable()
                        where li.company_id == CompanyId && li.is_active
                        select new SelectListItem
                        {
                            Text = li.lemination_type_name,
                            Value = li.lemination_type_id.ToString()
                        }).ToList();
            return list;
        }
        public Result PostCreteEditJob(job job ,string Jobcomment)
        {
            try
            {
                var createdBy = STUtil.GetSessionValue(UserInfo.FullName.ToString());
                var JobStatus = db.job_status.Find(job.job_status_id);
                if (job.job_id > 0)
                {

                    string comment = "<div class ='container left'><div class='content'><b>Update By :" + createdBy + "</b></br><b>Updated ON:" + System.DateTime.Now.ToString("dd-MMM-yyyy") + "</b></br> <p>Job Update Successfully and Job Status is <b>"+ JobStatus.job_status_name+ "</b></p><b>Comment: </b><p>"+ job.comment + "</p></div></div>";
                    job.comment = comment +( string.IsNullOrEmpty(Jobcomment) ? "": Jobcomment);
                    db.Entry(job).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Job");
                }
                else
                {
                    string comment = "<div class ='container left'><div class='content'><b>Created By :" + createdBy + "</b></br><b>Created ON:" + System.DateTime.Now.ToString("dd-MMM-yyyy") + "</b></br> <p>Job Create Successfully and Job Status is <b>" + JobStatus.job_status_name + "</b></p><b>Comment: </b><p>" + job.comment + "</p></div></div>";
                    job.job_status_id = 1;
                    job.comment = comment;
                    db.jobs.Add(job);
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Job");
                }
                result.Id = job.job_id;

            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return result;
        }
        public Result PostCreateEditJobAttachment(job_attachment obj)
        {
            try
            {
                db.job_attachment.Add(obj);
                db.SaveChanges();
                result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Job Attachment");
                result.MessageType = MessageType.Success;
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return result;
        }
        public Result DeleteJobAttachment(int id)
        {
            try
            {
                var data = db.job_attachment.Find(id);
                if (data != null)
                {
                    db.job_attachment.Remove(data);
                    db.SaveChanges();
                    result.MessageType = MessageType.Success;
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_DELETE, "Job Attachment");
                }
                else
                {
                    result.Message = "Data Not Found";
                    result.MessageType = MessageType.Error;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.MessageType = MessageType.Error;
            }
            return result;
        }
        public Result DeleteJobAssign(int id)
        {
            try
            {
                var data = db.job_assignment.Find(id);
                if (data != null)
                {
                    db.job_assignment.Remove(data);
                    db.SaveChanges();
                    result.MessageType = MessageType.Success;
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_DELETE, "Job Assign");
                }
                else
                {
                    result.Message = "Data Not Found";
                    result.MessageType = MessageType.Error;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.MessageType = MessageType.Error;
            }
            return result;
        }
    }
}