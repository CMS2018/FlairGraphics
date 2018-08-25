using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlairGraphic.Base.Models;
using FlairGraphic.Models;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.IO;
using System.Globalization;

namespace FlairGraphic.Controllers
{
    public class JsonController : BaseController
    {
        public Result Result { get; set; }
        private CountryUtil countryUtil;
        private StateUtil stateUtil;
        private UserUtil userUtil;
        private AreaUtil areaUtil;
        private SubAreaUtil subareaUtil;
        private AppointmentTypeUtil appointmentTypeUtil;

        private CurrencyUtil currencyUtil;
        private DateFormatUtil dateFormatUtil;
        private TimeFormatUtil timeFormatUtil;
        private DocumentTypeUtil documentTypeUtil;
        private LanguageUtil languageUtil;
        private ReligionUtil religionUtil;
        private NationalityUtil nationalityUtil;
        private SourceUtil sourceUtil;
        private PackageUtil packageUtil;
        private CityUtil cityUtil;
        // private ScoreColumnUtil scoreColumnUtil;
        private DurationDaysUtil durationDaysUtil;
        private IndustryUtil industryUtil;
        private PaymentModeUtil paymentModeUtil;
        private ApplicationTemplatePlaceHolderUtil applicationTemplatePlaceHolderUtil;
        private JobTypeUtill jobTypeUtill;
        private JobStatusUtill jobStatusUtill;
        private PackageTypeUtill packageTypeUtill;
        private PaperTypeUtill paperTypeUtill;
        private PaperSubTypeUtill paperSubTypeUtill;
        private PaymentStatusUtill paymentStatusUtill;
        private LeminationTypeUtill leminationTypeUtill;
        public JsonController()
        {
            Result = new Result();
            countryUtil = new CountryUtil();
            stateUtil = new StateUtil();
            areaUtil = new AreaUtil();
            userUtil = new UserUtil();
            subareaUtil = new SubAreaUtil();
            appointmentTypeUtil = new AppointmentTypeUtil();
            currencyUtil = new CurrencyUtil();
            dateFormatUtil = new DateFormatUtil();
            languageUtil = new LanguageUtil();
            religionUtil = new ReligionUtil();
            durationDaysUtil = new DurationDaysUtil();
            sourceUtil = new SourceUtil();
            packageUtil = new PackageUtil();
            cityUtil = new CityUtil();
            //scoreColumnUtil = new ScoreColumnUtil();
            nationalityUtil = new NationalityUtil();
            timeFormatUtil = new TimeFormatUtil();
            industryUtil = new IndustryUtil();
            paymentModeUtil = new PaymentModeUtil();
            applicationTemplatePlaceHolderUtil = new ApplicationTemplatePlaceHolderUtil();
            jobTypeUtill = new JobTypeUtill();
            jobStatusUtill = new JobStatusUtill();
            packageTypeUtill = new PackageTypeUtill();
            paperTypeUtill = new PaperTypeUtill();
            paperSubTypeUtill = new PaperSubTypeUtill();
            paymentStatusUtill = new PaymentStatusUtill();
            currencyUtil = new CurrencyUtil();
            leminationTypeUtill = new LeminationTypeUtill();
        }

        #region Email & Login ID
        public ActionResult IsEmailAddressExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string emailID = id;
            int RoleBit = 0;

            if (info.Length == 2)
            {
                emailID = info[0];
                RoleBit = info[1] != "" ? Convert.ToInt32(info[1]) : RoleBit;
            }
            return Json(userUtil.IsEmailAddressExist(emailID, RoleBit) == null ? false : true);
        }
        public ActionResult IsLoginIDExist(string id)
        {
            string LoginID = id;
            return Json(userUtil.IsLoginIDExist(LoginID) == null ? false : true);
        }
        #endregion

        #region User
        public ActionResult IsEmailIdExist(string id)
        {
            string[] info = id.Split(',');
            string emailID = id;
            int userId = 0;

            if (info.Length == 2)
            {
                emailID = info[0];
                userId = info[1] != "" ? Convert.ToInt32(info[1]) : userId;
            }
            return Json(userUtil.IsEmailExist(emailID, userId) == null ? false : true);
        }
        public ActionResult getRegPassword(string id)
        {
            int user_id = Convert.ToInt32(id);
            //int company_id = SessionUtil.GetCompanyID();
            var userData = db.users.Find(user_id);
            int company_id = userData.company_id;
            //string password = Convert.ToString(db.USP_Decrypt_TEXT(userData.password));
            string password = db.USP_GetUserPassword(user_id, company_id).ToList().FirstOrDefault().password;
            return Json(password);
        }
        public ActionResult IsRegEmailIdExist(string id)
        {
            string[] info = id.Split(',');
            string emailID = id;
            int roleBit = 0;

            if (info.Length == 2)
            {
                emailID = info[0];
                roleBit = info[1] != "" ? Convert.ToInt32(info[1]) : roleBit;
            }
            return Json(userUtil.IsRegEmailExist(emailID, roleBit) == null ? false : true);
        }
        public ActionResult getPassword(string id)
        {
            int user_id = Convert.ToInt32(id);
            int company_id = SessionUtil.GetCompanyID();
            string password = db.USP_GetUserPassword(user_id, company_id).ToList().FirstOrDefault().password;
            return Json(password);
        }
        #endregion

        #region COUNTRY
        public ActionResult IsCountryNameExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string countryName = id;
            int countryId = 0;
            if (info.Length == 2)
            {
                countryName = info[0];
                countryId = info[1] != "" ? Convert.ToInt32(info[1]) : countryId;
            }
            return Json(countryUtil.GetCountryByCountryId(countryId, countryName) == null ? false : true);
        }

        #endregion

        #region STATE
        public ActionResult IsStateExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string stateName = id;
            int countryId = 0;
            int stateId = 0;
            if (info.Length == 3)
            {
                stateName = info[0];
                stateId = info[1] != "" ? Convert.ToInt32(info[1]) : stateId;
                countryId = info[2] != "" ? Convert.ToInt32(info[2]) : countryId;
            }
            return Json(stateUtil.GetStateByName(stateId, stateName, countryId) == null ? false : true);

        }
        public ActionResult GetStateByCountry(int id)
        {
            var list = (from c in db.states.AsEnumerable()
                        where c.country_id == id
                        select new SelectListItem
                        {
                            Text = c.state_name,
                            Value = c.state_id.ToString(),
                        }).ToList();
            return Json(list);
        }
        #endregion

        #region City
        public ActionResult IsCityExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string cityName = id;
            int cityId = 0;
            int countryId = 0;
            int stateId = 0;
            if (info.Length == 4)
            {
                cityName = info[0];
                cityId = info[1] != "" ? Convert.ToInt32(info[1]) : cityId;
                stateId = info[2] != "" ? Convert.ToInt32(info[2]) : stateId;
                countryId = info[3] != "" ? Convert.ToInt32(info[3]) : countryId;
            }
            return Json(cityUtil.GetCityByName(cityName, cityId, stateId, countryId) == null ? false : true);
        }
        public ActionResult GetCityByState(int id)
        {
            var list = (from c in db.cities.AsEnumerable()
                        where c.state_id == id
                        select new SelectListItem
                        {
                            Text = c.city_name,
                            Value = c.city_id.ToString(),
                        }).ToList();
            return Json(list);
        }
        #endregion

        #region Area
        public ActionResult IsAreaExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string areaName = id;
            int countryId = 0;
            int stateId = 0;
            int cityId = 0;
            int areaId = 0;
            if (info.Length == 5)
            {
                areaName = info[0];
                areaId = info[1] != "" ? Convert.ToInt32(info[1]) : areaId;
                countryId = info[2] != "" ? Convert.ToInt32(info[2]) : countryId;
                stateId = info[3] != "" ? Convert.ToInt32(info[3]) : stateId;
                cityId = info[4] != "" ? Convert.ToInt32(info[4]) : cityId;
            }
            return Json(areaUtil.GetAreaByName(areaId, areaName, countryId, stateId, cityId) == null ? false : true);
        }
        public ActionResult GetAreaByCity(int id)
        {
            var list = (from c in db.areas.AsEnumerable()
                        where c.city_id == id
                        select new SelectListItem
                        {
                            Text = c.area_name,
                            Value = c.area_id.ToString(),
                        }).ToList();
            return Json(list);
        }
        #endregion

        #region SUB AREA
        public ActionResult IsSubAreaExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string subareaName = id;
            int countryId = 0;
            int stateId = 0;
            int cityId = 0;
            int areaId = 0;
            int subareaId = 0;
            if (info.Length == 6)
            {
                subareaName = info[0];
                areaId = info[1] != "" ? Convert.ToInt32(info[1]) : areaId;
                subareaId = info[2] != "" ? Convert.ToInt32(info[2]) : subareaId;
                countryId = info[3] != "" ? Convert.ToInt32(info[3]) : countryId;
                stateId = info[4] != "" ? Convert.ToInt32(info[4]) : stateId;
                cityId = info[5] != "" ? Convert.ToInt32(info[5]) : cityId;
            }
            return Json(subareaUtil.GetSubAreaByName(subareaName, areaId, subareaId, countryId, stateId, cityId) == null ? false : true);
        }
        #endregion

        #region ContactTitle
        //public ActionResult IsContactTitleExist(string id)
        //{
        //    //id="country_name,country_id"
        //    string[] info = id.Split(',');
        //    string contactTitleName = id;
        //    int contactTitleId = 0;
        //    if (info.Length == 2)
        //    {
        //        contactTitleName = info[0];
        //        contactTitleId = info[1] != "" ? Convert.ToInt32(info[1]) : contactTitleId;
        //    }
        //    return Json(contactTitleUtil.GetContactTitleByContactTitleId(contactTitleName, contactTitleId) == null ? false : true);
        //}
        #endregion

        #region ContactType
        //public ActionResult IsContactTypeExist(string id)
        //{
        //    //id="country_name,country_id"
        //    string[] info = id.Split(',');
        //    string contactTypeName = id;
        //    int contactTypeId = 0;
        //    if (info.Length == 2)
        //    {
        //        contactTypeName = info[0];
        //        contactTypeId = info[1] != "" ? Convert.ToInt32(info[1]) : contactTypeId;
        //    }
        //    return Json(contactTypeUtil.GetContactTypeByContactTypeId(contactTypeName, contactTypeId) == null ? false : true);
        //}
        #endregion

        #region Currency
        public ActionResult IsCurrencyExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string contactTypeName = id;
            int currencyId = 0;
            if (info.Length == 2)
            {
                contactTypeName = info[0];
                currencyId = info[1] != "" ? Convert.ToInt32(info[1]) : currencyId;
            }
            return Json(currencyUtil.GetCurrencyByCurrencyId(contactTypeName, currencyId) == null ? false : true);
        }
        #endregion

        #region Date Format
        public ActionResult IsDateFormatExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string dateFormatName = id;
            int dateFormatId = 0;
            if (info.Length == 2)
            {
                dateFormatName = info[0];
                dateFormatId = info[1] != "" ? Convert.ToInt32(info[1]) : dateFormatId;
            }
            return Json(dateFormatUtil.GetDateFormatByDateFormatId(dateFormatName, dateFormatId) == null ? false : true);
        }
        #endregion

        #region TIME FORMAT
        public ActionResult IsTimeFormatExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string timeFormatName = id;
            int timeFormatId = 0;
            if (info.Length == 2)
            {
                timeFormatName = info[0];
                timeFormatId = info[1] != "" ? Convert.ToInt32(info[1]) : timeFormatId;
            }
            return Json(timeFormatUtil.GetTimeFormatByTimeFormatId(timeFormatName, timeFormatId) == null ? false : true);
        }
        #endregion

        #region Document Type
        public ActionResult IsDocumentTypeExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string documentTypeName = id;
            int documentTypeId = 0;
            if (info.Length == 2)
            {
                documentTypeName = info[0];
                documentTypeId = info[1] != "" ? Convert.ToInt32(info[1]) : documentTypeId;
            }
            return Json(documentTypeUtil.GetDocumentTypeByDocumentTypeId(documentTypeName, documentTypeId) == null ? false : true);
        }
        #endregion

        #region Language
        public ActionResult IsLanguageExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string languageName = id;
            int languageId = 0;
            if (info.Length == 2)
            {
                languageName = info[0];
                languageId = info[1] != "" ? Convert.ToInt32(info[1]) : languageId;
            }
            return Json(languageUtil.GetLanguageByLanguageId(languageName, languageId) == null ? false : true);
        }
        #endregion

        #region Religion
        public ActionResult IsReligionExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string religionName = id;
            int religionId = 0;
            if (info.Length == 2)
            {
                religionName = info[0];
                religionId = info[1] != "" ? Convert.ToInt32(info[1]) : religionId;
            }
            return Json(religionUtil.GetReligionByReligionId(religionName, religionId) == null ? false : true);
        }
        #endregion

        #region Nationality
        public ActionResult IsNationalityExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string nationalityName = id;
            int nationalityId = 0;
            if (info.Length == 2)
            {
                nationalityName = info[0];
                nationalityId = info[1] != "" ? Convert.ToInt32(info[1]) : nationalityId;
            }
            return Json(nationalityUtil.GetNationalityByNationalityId(nationalityName, nationalityId) == null ? false : true);
        }
        #endregion

        #region Duration Days
        public ActionResult IsDurationDaysExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string durationdaystext = id;
            string durationdaysvalue = id;
            int durationdaysid = 0;
            if (info.Length == 3)
            {
                durationdaysvalue = info[0];
                durationdaystext = info[1];
                durationdaysid = info[2] != "" ? Convert.ToInt32(info[2]) : durationdaysid;
            }
            return Json(durationDaysUtil.GetDurationDaysByDurationId(durationdaystext, durationdaysvalue, durationdaysid) == null ? false : true);
        }
        public ActionResult IsDurationTextExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string durationdaystext = id;
            int durationdaysid = 0;
            if (info.Length == 2)
            {
                durationdaystext = info[0];
                durationdaysid = info[1] != "" ? Convert.ToInt32(info[1]) : durationdaysid;
            }
            return Json(durationDaysUtil.GetDurationTextByDurationId(durationdaystext, durationdaysid) == null ? false : true);
        }
        #endregion

        #region Industry Type
        public ActionResult IsIndustryTypeNameExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string industryName = id;
            int industryId = 0;
            if (info.Length == 2)
            {
                industryName = info[0];
                industryId = info[1] != "" ? Convert.ToInt32(info[1]) : industryId;
            }
            return Json(industryUtil.GetIndustryByIndustryId(industryId, industryName) == null ? false : true);
        }
        #endregion

        #region Payment Mode
        public ActionResult IspaymentmodeNameExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string paymentmodename = id;
            int paymentmodeId = 0;
            if (info.Length == 2)
            {
                paymentmodename = info[0];
                paymentmodeId = info[1] != "" ? Convert.ToInt32(info[1]) : paymentmodeId;
            }
            return Json(paymentModeUtil.GetPaymentModeByPaymentId(paymentmodename, paymentmodeId) == null ? false : true);
        }
        #endregion

        #region Score Column
        //public ActionResult IsScoreColumnExist(string id)
        //{
        //    //id="country_name,country_id"
        //    string[] info = id.Split(',');
        //    string scorecolumnname = id;
        //    int scorecolumnid = 0;
        //    if (info.Length == 2)
        //    {

        //        scorecolumnname = info[0];
        //        scorecolumnid = info[1] != "" ? Convert.ToInt32(info[1]) : scorecolumnid;
        //    }
        //    return Json(scoreColumnUtil.GetScoreColumnByScoreId(scorecolumnname, scorecolumnid) == null ? false : true);
        //}
        #endregion


        public ActionResult ChangeTheme(string id)
        {
            Result result = new Result();
            var colorCss = "";
            int themeColorId = Convert.ToInt32(id);
            switch ((ThemeColor)(themeColorId))
            {
                case ThemeColor.default_them:
                    colorCss = "default.css";
                    break;
                case ThemeColor.green:
                    colorCss = "green.css";
                    break;
                case ThemeColor.red:
                    colorCss = "red.css";
                    break;
                case ThemeColor.blue:
                    colorCss = "blue.css";
                    break;
                case ThemeColor.purple:
                    colorCss = "purple.css";
                    break;
                case ThemeColor.megna:
                    colorCss = "megna.css";
                    break;
                case ThemeColor.default_dark:
                    colorCss = "default-dark.css";
                    break;
                case ThemeColor.green_dark:
                    colorCss = "green-dark.css";
                    break;
                case ThemeColor.blue_dark:
                    colorCss = "blue-dark.css";
                    break;
                case ThemeColor.purple_dark:
                    colorCss = "purple-dark.css";
                    break;
                case ThemeColor.megna_dark:
                    colorCss = "megna-dark.css";
                    break;
                default:
                    break;
            }
            STUtil.SetSessionValue(UserInfo.theme_color.ToString(), Convert.ToString(colorCss));
            user u = db.users.Find((Int32)SessionUtil.GetUserID());
            u.theme_color_id = themeColorId;///colorCss.ToString();
            db.Entry(u).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            result.MessageType = MessageType.Success;
            result.Message = "Theme Apply Successfully";
            return RedirectToAction("Index", "DashBoard", new { Result = result.Message, MessageType = result.MessageType });
        }

        #region Package
        public ActionResult IsPackageNameExist(string id)
        {
            string[] info = id.Split(',');
            string packageName = id;
            int packageId = 0;
            if (info.Length == 2)
            {
                packageName = info[0];
                packageId = info[1] != "" ? Convert.ToInt32(info[1]) : packageId;
            }
            return Json(packageUtil.GetPackageByPackageId(packageId, packageName) == null ? false : true);
        }
        public JsonResult GetPackageById(int id)
        {
            var list = db.packages.AsEnumerable().ToList().Where(x => x.package_id == id).FirstOrDefault();
            var CurrencyLst = db.currencies.AsEnumerable().ToList().Where(x => x.currency_id == list.currency_id).FirstOrDefault();
            return Json(
                            new
                            {
                                per_user_price = list.per_user_price,

                                package_name = list.package_name,
                                duration_days = list.duration_days.duration_days_text,
                                setup_cost = list.setup_cost,
                                currency_symbol = CurrencyLst.currency_symbol,
                                tax_display = list.tax_display,
                                tax_percentage = list.tax_percentage,
                            }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult MyPackagesDetail(int id)
        {
            var list = db.package_subscription.AsEnumerable().ToList().Where(x => x.company_id == SessionUtil.GetCompanyID() && x.package_id == id).ToList();

            return Json(from li in list
                        select

                            new
                            {
                                package_subscription_id = li.package_subscription_id,
                                package_name = li.package.package_name,
                                subscription_start_date = li.subscription_start_date,
                                subscription_end_date = li.subscription_end_date,
                                duration_days_text = li.duration_days.duration_days_text,
                                Payment_Status = li.payment_status.payment_status_name,
                            }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult CustomerPackageslist(int id)
        {
            var list = db.package_subscription.AsEnumerable().ToList().Where(x => x.company_id == id).ToList(); ;
            var data = (from li in list
                        select new
                        {
                            package_name = li.package.package_name,
                            allow_users = li.allow_users,
                            subscription_start_date = li.subscription_start_date,
                            subscription_end_date = li.subscription_end_date,
                            package_id = li.package_id,
                            payment_status_name = li.payment_status.payment_status_name,
                            setup_cost = li.setup_cost,

                        }).ToList();
            return Json(data);

        }
        #endregion

        #region COMPANY HOLIDAY
        public ActionResult IsHolidayDateExist(string id)
        {
            string[] info = id.Split(',');
            string companyholidayDate = id;
            int companyholidayId = 0;
            DateTime dt = new DateTime();
            if (info.Length == 2)
            {
                companyholidayDate = info[0];
                String stringdate = DateTime.ParseExact(companyholidayDate, "mm/dd/yyyy", null).ToString("dd/mm/yyyy");
                dt = Convert.ToDateTime(stringdate);
                companyholidayId = info[1] != "" ? Convert.ToInt32(info[1]) : companyholidayId;
            }
            return Json(new CompanyUtil().GetHolidayDateByHolidayId(companyholidayId, dt) == null ? false : true);
        }
        #endregion

        #region Role
        public ActionResult IsRoleNameExist(string id)
        {
            string[] info = id.Split(',');
            string role_name = id;
            int role_id = 0;
            int company_id = 0;
            if (info.Length == 3)
            {
                role_name = info[0];
                role_id = info[1] != "" ? Convert.ToInt32(info[1]) : role_id;
                company_id = info[2] != "" ? Convert.ToInt32(info[2]) : company_id;
            }
            return Json(new CompanyUtil().GetRoleNameByCompanyId(role_id, role_name, company_id) == null ? false : true);
        }
        #endregion

        #region Admin Setting
        public ActionResult IsLeminationTypeNameExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string leminationTypeName = id;
            int leminationTypeId = 0;
            if (info.Length == 2)
            {
                leminationTypeName = info[0];
                leminationTypeId = info[1] != "" ? Convert.ToInt32(info[1]) : leminationTypeId;
            }
            return Json(leminationTypeUtill.GetTypeDetailsById(leminationTypeId, leminationTypeName) == null ? false : true);
        }
        public ActionResult IsJobTypeNameExist(string id)
        {
            string[] info = id.Split(',');
            string JobTypeName = id;
            int jobTypeId = 0;
            if (info.Length == 2)
            {
                JobTypeName = info[0];
                jobTypeId = info[1] != "" ? Convert.ToInt32(info[1]) : jobTypeId;
            }
            return Json(jobTypeUtill.GetJobTypeDetailsById(jobTypeId, JobTypeName) == null ? false : true);
        }
        public ActionResult IsJobStatusNameExist(string id)
        {
            string[] info = id.Split(',');
            string JobStatusName = id;
            int JobStatusId = 0;
            if (info.Length == 2)
            {
                JobStatusName = info[0];
                JobStatusId = info[1] != "" ? Convert.ToInt32(info[1]) : JobStatusId;
            }
            return Json(jobStatusUtill.GetJobStatusDetailsById(JobStatusId, JobStatusName) == null ? false : true);
        }
        public ActionResult IsPaperTypeNameExist(string id)
        {
            string[] info = id.Split(',');
            string PaperTypeName = id;
            int PaperTypeId = 0;
            if (info.Length == 2)
            {
                PaperTypeName = info[0];
                PaperTypeId = info[1] != "" ? Convert.ToInt32(info[1]) : PaperTypeId;
            }
            return Json(paperTypeUtill.GetPaperTypeDetailsById(PaperTypeId, PaperTypeName) == null ? false : true);
        }
        public ActionResult IsPackageTypeNameExist(string id)
        {
            string[] info = id.Split(',');
            string PackageTypeName = id;
            int PackageTypeId = 0;
            if (info.Length == 2)
            {
                PackageTypeName = info[0];
                PackageTypeId = info[1] != "" ? Convert.ToInt32(info[1]) : PackageTypeId;
            }
            return Json(packageTypeUtill.GetPackageTypeDetailsById(PackageTypeId, PackageTypeName) == null ? false : true);
        }
        public ActionResult IsPaperSubTypeNameExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string PapperSubTypeName = id;
            int PaperSubTypeId = 0;
            if (info.Length == 2)
            {
                PapperSubTypeName = info[0];
                PaperSubTypeId = info[1] != "" ? Convert.ToInt32(info[1]) : PaperSubTypeId;
            }
            return Json(paperSubTypeUtill.GetPaperSubTypeDetailsById(PaperSubTypeId, PapperSubTypeName) == null ? false : true);
        }
        public ActionResult IsPaymentNameExist(string id)
        {
            //id="country_name,country_id"
            string[] info = id.Split(',');
            string PaymentStatusName = id;
            int PaymentStatusId = 0;
            if (info.Length == 2)
            {
                PaymentStatusName = info[0];
                PaymentStatusId = info[1] != "" ? Convert.ToInt32(info[1]) : PaymentStatusId;
            }
            return Json(paymentStatusUtill.GetPaymentStatusDetailsById(PaymentStatusId, PaymentStatusName) == null ? false : true);
        }
        #endregion
    }
}
