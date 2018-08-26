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
        public IList<SelectListItem> GetAssignedUsers()
        {
            var CompanyId = SessionUtil.GetCompanyID();
            var list = (from li in db.users.AsEnumerable()
                        where li.role_bit == Convert.ToInt32(Role.Manager) || li.role_bit == Convert.ToInt32(Role.Operator) && li.company_id == CompanyId && li.is_active
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
        public Result PostCreteEditJob(job job ,string Jobcomment,string jobCode ="100")
        {
            try
            {
                var createdBy = STUtil.GetSessionValue(UserInfo.FullName.ToString());
                job.job_status_id = job.job_status_id == 0 ? 1 : job.job_status_id;
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
                    job.job_code = "FG-";
                    db.jobs.Add(job);
                    db.SaveChanges();
                    var jobObj = db.jobs.Find(job.job_id);
                    jobObj.job_code = "FG-" + jobObj.job_id.ToString();
                    db.Entry(jobObj).State = System.Data.Entity.EntityState.Modified;
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

        public List<SelectListItem> GetPaperSizeList()
        {
            int companyId = (Int32)SessionUtil.GetCompanyID();
            var list = (from c in db.paper_size.AsEnumerable()
                        where c.company_id == companyId && c.is_active
                        select new SelectListItem
                        {
                            Text = c.paper_size_name,
                            Value = c.paper_size_id.ToString(),
                        }).ToList();
            return list;
        }
        public List<SelectListItem> GetPrintingSideList()
        {
            int companyId = (Int32)SessionUtil.GetCompanyID();
            var list = (from c in db.printing_side.AsEnumerable()
                        where c.company_id == companyId && c.is_active
                        select new SelectListItem
                        {
                            Text = c.printing_side_name,
                            Value = c.printing_side_id.ToString(),
                        }).ToList();
            return list;
        }

        public List<SelectListItem> GetOrientationList()
        {
            int companyId = (Int32)SessionUtil.GetCompanyID();
            var list = (from c in db.orientations.AsEnumerable()
                        where c.company_id == companyId && c.is_active
                        select new SelectListItem
                        {
                            Text = c.orientation_name,
                            Value = c.orientation_id.ToString(),
                        }).ToList();
            return list;
        }
    }
}