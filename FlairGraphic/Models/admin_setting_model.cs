using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlairGraphic.Base.Models;

namespace FlairGraphic.Models
{
    public class admin_setting_model
    {
    }
    #region Lemination Type
    public class LeminationTypeUtill
    {
        private Result result;
        private BaseEntities db;
        public LeminationTypeUtill()
        {
            this.result = new Result();
            this.db = new BaseEntities();
        }
        public Result LeminationTypeCreateEdit(lemination_type lemination_type)
        {
            try
            {
                if (lemination_type.lemination_type_id > 0)
                {
                    lemination_type ct = db.lemination_type.Find(lemination_type.lemination_type_id);
                    ct.lemination_type_name = lemination_type.lemination_type_name;
                    ct.is_active = lemination_type.is_active;
                    db.Entry(ct).State = System.Data.Entity.EntityState.Modified;
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Lemination Type");
                }
                else
                {
                    lemination_type.is_active = true;
                    db.lemination_type.Add(lemination_type);
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Lemination Type");
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
        public lemination_type GetTypeDetailsById(int id, string name)
        {
            return id > 0 ?
                  db.lemination_type.AsEnumerable().Where(x => x.lemination_type_name.ToUpper() == name.ToUpper() && x.lemination_type_id != id).FirstOrDefault()
                  : db.lemination_type.AsEnumerable().Where(x => x.lemination_type_name.ToUpper() == name.ToUpper()).FirstOrDefault();
        }
    }
    #endregion

    #region Job Type
    public class JobTypeUtill
    {
        private Result result;
        private BaseEntities db;
        private int CompanyId;
        public JobTypeUtill()
        {
            this.result = new Result();
            this.db = new BaseEntities();
            this.CompanyId = SessionUtil.GetCompanyID();
        }
        public Result JobTypeCreateEdit(job_type job_type)
        {
            try
            {
                if (job_type.job_type_id > 0)
                {
                    job_type ct = db.job_type.Find(job_type.job_type_id);
                    ct.job_type_name = job_type.job_type_name;
                    ct.is_active = job_type.is_active;
                    db.Entry(ct).State = System.Data.Entity.EntityState.Modified;
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Job Type");
                }
                else
                {
                    job_type.is_active = true;
                    db.job_type.Add(job_type);
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Job Type");
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
        public job_type GetJobTypeDetailsById(int id, string name)
        {
            return id > 0 ?
                  db.job_type.AsEnumerable().Where(x => x.job_type_name.ToUpper() == name.ToUpper() && x.job_type_id != id && x.company_id == CompanyId).FirstOrDefault()
                  : db.job_type.AsEnumerable().Where(x => x.job_type_name.ToUpper() == name.ToUpper() && x.company_id == CompanyId).FirstOrDefault();
        }
    }
    #endregion

    #region Job Status
    public class JobStatusUtill
    {
        private Result result;
        private BaseEntities db;
        private int CompanyId;
        public JobStatusUtill()
        {
            this.result = new Result();
            this.db = new BaseEntities();
            this.CompanyId = SessionUtil.GetCompanyID();
        }
        public Result JobStatusCreateEdit(job_status job_status)
        {
            try
            {
                if (job_status.job_status_id > 0)
                {
                    job_status ct = db.job_status.Find(job_status.job_status_id);
                    ct.job_status_name = job_status.job_status_name;
                    ct.is_active = job_status.is_active;
                    db.Entry(ct).State = System.Data.Entity.EntityState.Modified;
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Job Status");
                }
                else
                {
                    job_status.is_active = true;
                    db.job_status.Add(job_status);
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Job Status");
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
        public job_status GetJobStatusDetailsById(int id, string name)
        {
            return id > 0 ?
                  db.job_status.AsEnumerable().Where(x => x.job_status_name.ToUpper() == name.ToUpper() && x.job_status_id != id && x.company_id == CompanyId).FirstOrDefault()
                  : db.job_status.AsEnumerable().Where(x => x.job_status_name.ToUpper() == name.ToUpper() && x.company_id == CompanyId).FirstOrDefault();
        }
    }
    #endregion

    #region Package Type
    public class PackageTypeUtill
    {
        private Result result;
        private BaseEntities db;
        private int CompanyId;
        public PackageTypeUtill()
        {
            this.result = new Result();
            this.db = new BaseEntities();
            this.CompanyId = SessionUtil.GetCompanyID();
        }
        public Result PackageTypeCreateEdit(package_type package_type)
        {
            try
            {
                if (package_type.package_type_id > 0)
                {
                    package_type ct = db.package_type.Find(package_type.package_type_id);
                    ct.package_type_name = package_type.package_type_name;
                    ct.is_active = package_type.is_active;
                    db.Entry(ct).State = System.Data.Entity.EntityState.Modified;
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Package Type");
                }
                else
                {
                    package_type.is_active = true;
                    db.package_type.Add(package_type);
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Package Type");
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
        public package_type GetPackageTypeDetailsById(int id, string name)
        {
            return id > 0 ?
                  db.package_type.AsEnumerable().Where(x => x.package_type_name.ToUpper() == name.ToUpper() && x.package_type_id != id).FirstOrDefault()
                  : db.package_type.AsEnumerable().Where(x => x.package_type_name.ToUpper() == name.ToUpper()).FirstOrDefault();
        }
    }
    #endregion

    #region Paper Type
    public class PaperTypeUtill
    {
        private Result result;
        private BaseEntities db;
        private int CompanyId;
        public PaperTypeUtill()
        {
            this.result = new Result();
            this.db = new BaseEntities();
            this.CompanyId = SessionUtil.GetCompanyID();
        }
        public Result PaperTypeCreateEdit(paper_type paper_type)
        {
            try
            {
                if (paper_type.paper_type_id > 0)
                {
                    paper_type ct = db.paper_type.Find(paper_type.paper_type_id);
                    ct.paper_type_name = paper_type.paper_type_name;
                    ct.is_active = paper_type.is_active;
                    db.Entry(ct).State = System.Data.Entity.EntityState.Modified;
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Paper Type");
                }
                else
                {
                    paper_type.is_active = true;
                    db.paper_type.Add(paper_type);
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Paper Type");
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
        public paper_type GetPaperTypeDetailsById(int id, string name)
        {
            return id > 0 ?
                  db.paper_type.AsEnumerable().Where(x => x.paper_type_name.ToUpper() == name.ToUpper() && x.paper_type_id != id && x.company_id == CompanyId).FirstOrDefault()
                  : db.paper_type.AsEnumerable().Where(x => x.paper_type_name.ToUpper() == name.ToUpper() && x.company_id == CompanyId).FirstOrDefault();
        }
    }
    #endregion

    #region Paper Sub Type
    public class PaperSubTypeUtill
    {
        private Result result;
        private BaseEntities db;
        private int CompanyId;
        public PaperSubTypeUtill()
        {
            this.result = new Result();
            this.db = new BaseEntities();
            this.CompanyId = SessionUtil.GetCompanyID();
        }
        public Result PaperSubTypeCreateEdit(paper_sub_type paper_sub_type)
        {
            try
            {
                if (paper_sub_type.paper_sub_type_id > 0)
                {
                    paper_sub_type ct = db.paper_sub_type.Find(paper_sub_type.paper_sub_type_id);
                    ct.paper_sub_type_name = paper_sub_type.paper_sub_type_name;
                    ct.is_active = paper_sub_type.is_active;
                    db.Entry(ct).State = System.Data.Entity.EntityState.Modified;
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Paper Sub Type");
                }
                else
                {
                    paper_sub_type.is_active = true;
                    db.paper_sub_type.Add(paper_sub_type);
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Paper Sub Type");
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
        public paper_sub_type GetPaperSubTypeDetailsById(int id, string name)
        {
            return id > 0 ?
                  db.paper_sub_type.AsEnumerable().Where(x => x.paper_sub_type_name.ToUpper() == name.ToUpper() && x.paper_sub_type_id != id && x.company_id == CompanyId).FirstOrDefault()
                  : db.paper_sub_type.AsEnumerable().Where(x => x.paper_sub_type_name.ToUpper() == name.ToUpper() && x.company_id == CompanyId).FirstOrDefault();
        }
    }
    #endregion

    #region Payment Status
    public class PaymentStatusUtill
    {
        private Result result;
        private BaseEntities db;
        private int CompanyId;
        public PaymentStatusUtill()
        {
            this.result = new Result();
            this.db = new BaseEntities();
            this.CompanyId = SessionUtil.GetCompanyID();
        }
        public Result PaymentStatusCreateEdit(payment_status payment_status)
        {
            try
            {
                if (payment_status.payment_status_id > 0)
                {
                    payment_status ct = db.payment_status.Find(payment_status.payment_status_id);
                    ct.payment_status_name = payment_status.payment_status_name;
                    ct.is_active = payment_status.is_active;
                    db.Entry(ct).State = System.Data.Entity.EntityState.Modified;
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Payment Status");
                }
                else
                {
                    payment_status.is_active = true;
                    db.payment_status.Add(payment_status);
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Payment Status");
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
        public payment_status GetPaymentStatusDetailsById(int id, string name)
        {
            return id > 0 ?
                  db.payment_status.AsEnumerable().Where(x => x.payment_status_name.ToUpper() == name.ToUpper() && x.payment_status_id != id).FirstOrDefault()
                  : db.payment_status.AsEnumerable().Where(x => x.payment_status_name.ToUpper() == name.ToUpper()).FirstOrDefault();
        }
    }
    #endregion
}