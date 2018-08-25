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
    public class MastersController : BaseController
    {
        private Result result;
        private CountryUtil countryUtil;
        private StateUtil stateUtil;
        private CityUtil cityUtil;
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
       // private ScoreColumnUtil scoreColumnUtil;
        private DurationDaysUtil durationDaysUtil;
        private PackageUtil packageUtil;

        private IndustryUtil industryUtil;
        private PaymentModeUtil paymentModeUtil;
        private ApplicationTemplatePlaceHolderUtil applicationTemplatePlaceHolderUtil;

        public MastersController()
        {
            result = new Result();
            countryUtil = new CountryUtil();
            stateUtil = new StateUtil();
            cityUtil = new CityUtil();
            areaUtil = new AreaUtil();
            subareaUtil = new SubAreaUtil();
            appointmentTypeUtil = new AppointmentTypeUtil();
            currencyUtil = new CurrencyUtil();
            dateFormatUtil = new DateFormatUtil();
            documentTypeUtil = new DocumentTypeUtil();
            languageUtil = new LanguageUtil();
            religionUtil = new ReligionUtil();
            nationalityUtil = new NationalityUtil();
            sourceUtil = new SourceUtil();
            durationDaysUtil = new DurationDaysUtil();
            packageUtil = new PackageUtil();

           // scoreColumnUtil = new ScoreColumnUtil();
            timeFormatUtil = new TimeFormatUtil();
            industryUtil = new IndustryUtil();
            paymentModeUtil = new PaymentModeUtil();
            applicationTemplatePlaceHolderUtil = new ApplicationTemplatePlaceHolderUtil();

        }
        public ActionResult ApplicationSetup()
        {
            ViewBag.Title = "Application Setup";

            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
        #region #REGIONS
        #region COUNTRY
        public ActionResult CountryIndex(string id)
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Country");
            country c = new country();
            if (id != null && id != "")
            {

                c = db.countries.Find(Convert.ToInt32(id));
            }

            return View(c);
        }
        public ActionResult CountryList(int id)
        {
            var list = db.countries.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            country_name = li.country_name,
                            country_id = li.country_id,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        // GET: /Country/Create
        public ActionResult CountryEdit(string id)
        {
            var data = (from c in db.countries.AsEnumerable()
                        where c.country_id == Convert.ToInt32(id)
                        select new
                        {
                            country_id = c.country_id,
                            country_name = c.country_name,
                            is_active = c.is_active,
                        }).FirstOrDefault();

            return Json(data);
        }
        // POST: /Country/Create
        [HttpPost]
        public ActionResult CountryCreateEdit(country country)
        {
            result = countryUtil.CreateEditCountry(country);
            ViewBag.Title = country == null ? "Country Create" : "Country Edit";
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            return Json(result);
        }

        #endregion

        #region STATE
        public ActionResult StateIndex(string id)
        {
            state s = new state();
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "State");
            ViewBag.country_id = new SelectList(countryUtil.GetCountrySelectList(), "Value", "Text");
            return View(s);
        }
        public ActionResult StateList(int id)
        {
            var list = db.states.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            country_name = li.country.country_name,
                            state_id = li.state_id,
                            state_name = li.state_name,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        // GET: /Country/Create
        public ActionResult StateEdit(string id)
        {
            var data = (from c in db.states.AsEnumerable()
                        where c.state_id == Convert.ToInt32(id)
                        select new
                        {
                            country_id = c.country_id,
                            state_name = c.state_name,
                            state_id = c.state_id,
                            is_active = c.is_active,
                        }).FirstOrDefault();

            return Json(data);
        }
        // POST: /Country/Create
        [HttpPost]
        public ActionResult StateCreateEdit(state state)
        {
            result = stateUtil.CreateEditState(state);
            return Json(result);
        }

        #endregion

        #region CITY
        public ActionResult CityIndex(string id)
        {
            city s = new city();
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "City");
            IList<SelectListItem> list = new List<SelectListItem>();
            ViewBag.country_id = new SelectList(countryUtil.GetCountrySelectList(), "Value", "Text");
            ViewBag.state_id = list;
            return View(s);
        }
        public ActionResult CityList(int id)
        {
            var list = db.cities.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            country_name = li.state.country.country_name,
                            state_id = li.state_id,
                            state_name = li.state.state_name,
                            city_name = li.city_name,
                            city_id = li.city_id,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        // GET: /Country/Create
        public ActionResult CityEdit(string id)
        {

            var data = (from c in db.cities.AsEnumerable()
                        where c.city_id == Convert.ToInt32(id)
                        select new
                        {
                            country_id = c.state.country_id,
                            state_idList = stateUtil.GetStateSelectList(c.state.country_id),
                            city_name = c.city_name,
                            state_id = c.state_id,
                            city_id = c.city_id,
                            is_active = c.is_active,
                        }).FirstOrDefault();

            return Json(data);
        }
        // POST: /Country/Create
        [HttpPost]
        public ActionResult CityCreateEdit(city city)
        {
            result = cityUtil.CreateEditCity(city);
            return Json(result);
        }
        #endregion

        #region Area
        public ActionResult AreaIndex(string id)
        {
            area s = new area();
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Area");
            IList<SelectListItem> list = new List<SelectListItem>();
            ViewBag.country_id = new SelectList(countryUtil.GetCountrySelectList(), "Value", "Text");
            ViewBag.state_id = list;
            ViewBag.city_id = list;
            return View(s);
        }
        public ActionResult AreaList(int id)
        {
            var list = db.areas.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            country_name = li.city.state.country.country_name,
                            state_name = li.city.state.state_name,
                            city_name = li.city.city_name,
                            area_id = li.area_id,
                            area_name = li.area_name,
                            display_order = li.display_order,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        // GET: /Area/Create
        public ActionResult AreaEdit(string id)
        {
            var data = (from c in db.areas.AsEnumerable()
                        where c.area_id == Convert.ToInt32(id)
                        select new
                        {
                            country_idList = countryUtil.GetCountrySelectList(),
                            country_id = c.city.state.country_id,

                            state_idList = stateUtil.GetStateSelectList(c.city.state.country_id),
                            state_id = c.city.state.state_id,

                            city_idList = cityUtil.GetCitySelectList(c.city.state.state_id),
                            city_id = c.city.city_id,

                            area_id = c.area_id,
                            area_name = c.area_name,
                            display_order = c.display_order,
                            is_active = c.is_active,
                        }).FirstOrDefault();

            return Json(data);
        }
        // POST: /Area/Create
        [HttpPost]
        public ActionResult AreaCreateEdit(area area)
        {
            result = areaUtil.CreateEditArea(area);
            return Json(result);
        }
        #endregion

        #region SubArea
        public ActionResult SubareaIndex(string id)
        {
            sub_area s = new sub_area();
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "SubArea");
            IList<SelectListItem> list = new List<SelectListItem>();
            ViewBag.country_id = new SelectList(countryUtil.GetCountrySelectList(), "Value", "Text");
            ViewBag.state_id = list;
            ViewBag.city_id = list;
            ViewBag.area_id = list;
            return View(s);
        }
        public ActionResult SubareaList(int id)
        {
            var list = db.sub_area.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            country_name = li.area.city.state.country.country_name,
                            state_name = li.area.city.state.state_name,
                            city_name = li.area.city.city_name,
                            area_name = li.area.area_name,
                            sub_area_id = li.sub_area_id,
                            sub_area_name = li.sub_area_name,
                            display_order = li.display_order,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        // GET: /Subarea/Create
        public ActionResult SubareaEdit(string id)
        {
            var data = (from c in db.sub_area.AsEnumerable()
                        where c.sub_area_id == Convert.ToInt32(id)
                        select new
                        {
                            country_idList = countryUtil.GetCountrySelectList(),
                            country_id = c.area.city.state.country_id,

                            state_idList = stateUtil.GetStateSelectList(c.area.city.state.country_id),
                            state_id = c.area.city.state.state_id,

                            city_idList = cityUtil.GetCitySelectList(c.area.city.state.state_id),
                            city_id = c.area.city.city_id,

                            area_idList = areaUtil.GetAreaSelectList(c.area.city.city_id),
                            area_id = c.area.area_id,

                            sub_area_id = c.sub_area_id,
                            sub_area_name = c.sub_area_name,
                            display_order = c.display_order,
                            is_active = c.is_active,
                        }).FirstOrDefault();

            return Json(data);
        }
        // POST: /Subarea/Create
        [HttpPost]
        public ActionResult SubareaCreateEdit(sub_area sub_area)
        {
            result = subareaUtil.CreateEditSubArea(sub_area);
            return Json(result);
        }
        #endregion

        //#region Contact Title
        //public ActionResult ContactTitleIndex(string id)
        //{
        //    ViewBag.Title = "Contact Title List";
        //    contact_title c = new contact_title();
        //    if (id != null && id != "")
        //    {
        //        c = db.contact_title.Find(Convert.ToInt32(id));
        //    }

        //    return View(c);
        //}
        //public ActionResult ContactTitleList(int id)
        //{
        //    var list = db.contact_title.AsEnumerable().ToList();
        //    var data = (from li in list
        //                select new
        //                {
        //                    contact_title_name = li.contact_title_name,
        //                    contact_title_id = li.contact_title_id,
        //                    is_active = li.is_active,
        //                }).ToList();
        //    return Json(data);
        //}
        //// GET: /Country/Create
        //public ActionResult ContactTitleEdit(string id)
        //{
        //    var data = (from c in db.contact_title.AsEnumerable()
        //                where c.contact_title_id == Convert.ToInt32(id)
        //                select new
        //                {
        //                    contact_title_id = c.contact_title_id,
        //                    contact_title_name = c.contact_title_name,
        //                    is_active = c.is_active,
        //                }).FirstOrDefault();

        //    return Json(data);
        //}
        //// POST: /Country/Create
        //[HttpPost]
        //public ActionResult ContactTitleCreateEdit(contact_title contact_title)
        //{
        //    result = contactTitleUtil.CreateEditContactTitle(contact_title);
        //    ViewBag.Title = contact_title == null ? "ContactTitle Create" : "ContactTitle Edit";
        //    ViewBag.action_name = STUtil.GetListAllActionByController("");
        //    return Json(result);
        //}
        //#endregion

        #region Currency
        public ActionResult CurrencyIndex(string id)
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Currency");
            currency c = new currency();
            if (id != null && id != "")
            {
                c = db.currencies.Find(Convert.ToInt32(id));
            }

            return View(c);
        }
        public ActionResult CurrencyList(int id)
        {
            var list = db.currencies.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            currency_name = li.currency_name,
                            currency_symbol = li.currency_symbol,
                            currency_id = li.currency_id,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        // GET: /Currency/Create
        public ActionResult CurrencyEdit(string id)
        {
            var data = (from c in db.currencies.AsEnumerable()
                        where c.currency_id == Convert.ToInt32(id)
                        select new
                        {
                            currency_id = c.currency_id,
                            currency_name = c.currency_name,
                            currency_symbol = c.currency_symbol,
                            is_active = c.is_active,
                        }).FirstOrDefault();

            return Json(data);
        }
        // POST: /Currency/Create
        [HttpPost]
        public ActionResult CurrencyCreateEdit(currency currency)
        {
            result = currencyUtil.CreateEditCurrency(currency);
            ViewBag.Title = currency == null ? "Currency Create" : "Currency Edit";
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            return Json(result);
        }
        #endregion

        #region Date Format
        public ActionResult DateFormatIndex(string id)
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Date Format");
            date_format c = new date_format();
            if (id != null && id != "")
            {
                c = db.date_format.Find(Convert.ToInt32(id));
            }

            return View(c);
        }
        public ActionResult DateFormatList(int id)
        {
            var list = db.date_format.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            date_format_name = li.date_format_name,
                            date_format_code_csharp = li.date_format_code_csharp,
                            date_format_code_js = li.date_format_code_js,
                            date_format_id = li.date_format_id,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        // GET: /DateFormat/Create
        public ActionResult DateFormatEdit(string id)
        {
            var data = (from c in db.date_format.AsEnumerable()
                        where c.date_format_id == Convert.ToInt32(id)
                        select new
                        {
                            date_format_id = c.date_format_id,
                            date_format_name = c.date_format_name,
                            date_format_code_csharp = c.date_format_code_csharp,
                            date_format_code_js = c.date_format_code_js,
                            is_active = c.is_active,
                        }).FirstOrDefault();

            return Json(data);
        }
        // POST: /DateFormat/Create
        [HttpPost]
        public ActionResult DateFormatCreateEdit(date_format date_format)
        {
            result = dateFormatUtil.CreateEditDateFormat(date_format);
            ViewBag.Title = date_format == null ? "DateFormat Create" : "DateFormat Edit";
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            return Json(result);
        }
        #endregion

        #region Time Format
        public ActionResult TimeFormatIndex(string id)
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Time Format");
            time_format c = new time_format();
            if (id != null && id != "")
            {
                c = db.time_format.Find(Convert.ToInt32(id));
            }

            return View(c);
        }
        public ActionResult TimeFormatList(int id)
        {
            var list = db.time_format.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            time_format_name = li.time_format_name,
                            time_format_code_csharp = li.time_format_code_csharp,
                            time_format_code_js = li.time_format_code_js,
                            time_format_id = li.time_format_id,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        // GET: /TimeFormat/Create
        public ActionResult TimeFormatEdit(string id)
        {
            var data = (from c in db.time_format.AsEnumerable()
                        where c.time_format_id == Convert.ToInt32(id)
                        select new
                        {
                            time_format_id = c.time_format_id,
                            time_format_name = c.time_format_name,
                            time_format_code_csharp = c.time_format_code_csharp,
                            time_format_code_js = c.time_format_code_js,
                            is_active = c.is_active,
                        }).FirstOrDefault();

            return Json(data);
        }
        // POST: /TimeFormat/Create
        [HttpPost]
        public ActionResult TimeFormatCreateEdit(time_format time_format)
        {
            result = timeFormatUtil.CreateEditTimeFormat(time_format);
            ViewBag.Title = time_format == null ? "TimeFormat Create" : "TimeFormat Edit";
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            return Json(result);
        }
        #endregion

        #region Document Type
        public ActionResult DocumentTypeIndex(string id)
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Document Type");
            document_type c = new document_type();
            if (id != null && id != "")
            {
                c = db.document_type.Find(Convert.ToInt32(id));
            }

            return View(c);
        }
        public ActionResult DocumentTypeList(int id)
        {
            var list = db.document_type.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            document_type_name = li.document_type_name,
                            document_type_id = li.document_type_id,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        // GET: /DocumentType/Create
        public ActionResult DocumentTypeEdit(string id)
        {
            var data = (from c in db.document_type.AsEnumerable()
                        where c.document_type_id == Convert.ToInt32(id)
                        select new
                        {
                            document_type_id = c.document_type_id,
                            document_type_name = c.document_type_name,
                            is_active = c.is_active,
                        }).FirstOrDefault();

            return Json(data);
        }
        // POST: /DocumentType/Create
        [HttpPost]
        public ActionResult DocumentTypeCreateEdit(document_type document_type)
        {
            result = documentTypeUtil.CreateEditDocumentType(document_type);
            ViewBag.Title = document_type == null ? "DocumentType Create" : "DocumentType Edit";
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            return Json(result);
        }
        #endregion

        #region Language
        public ActionResult LanguageIndex(string id)
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Language");
            language c = new language();
            if (id != null && id != "")
            {
                c = db.languages.Find(Convert.ToInt32(id));
            }

            return View(c);
        }
        public ActionResult LanguageList(int id)
        {
            var list = db.languages.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            language_code = li.language_code,
                            language_name = li.language_name,
                            language_id = li.language_id,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        // GET: /Language/Create
        public ActionResult LanguageEdit(string id)
        {
            var data = (from c in db.languages.AsEnumerable()
                        where c.language_id == Convert.ToInt32(id)
                        select new
                        {
                            language_id = c.language_id,
                            language_code = c.language_code,
                            language_name = c.language_name,
                            is_active = c.is_active,
                        }).FirstOrDefault();

            return Json(data);
        }
        // POST: /Language/Create
        [HttpPost]
        public ActionResult LanguageCreateEdit(language language)
        {
            result = languageUtil.CreateEditLanguage(language);
            ViewBag.Title = language == null ? "Language Create" : "Language Edit";
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            return Json(result);
        }
        #endregion

        #region Religion
        public ActionResult ReligionIndex(string id)
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Religion");
            religion c = new religion();
            if (id != null && id != "")
            {
                c = db.religions.Find(Convert.ToInt32(id));
            }

            return View(c);
        }
        public ActionResult ReligionList(int id)
        {
            var list = db.religions.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            religion_name = li.religion_name,
                            religion_id = li.religion_id,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        // GET: /Religion/Create
        public ActionResult ReligionEdit(string id)
        {
            var data = (from c in db.religions.AsEnumerable()
                        where c.religion_id == Convert.ToInt32(id)
                        select new
                        {
                            religion_id = c.religion_id,
                            religion_name = c.religion_name,
                            is_active = c.is_active,
                        }).FirstOrDefault();

            return Json(data);
        }
        // POST: /Religion/Create
        [HttpPost]
        public ActionResult ReligionCreateEdit(religion religion)
        {
            result = religionUtil.CreateEditReligion(religion);
            ViewBag.Title = religion == null ? "Religion Create" : "Religion Edit";
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            return Json(result);
        }
        #endregion

        #region Nationality
        public ActionResult NationalityIndex(string id)
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Nationality");
            nationality c = new nationality();
            if (id != null && id != "")
            {
                c = db.nationalities.Find(Convert.ToInt32(id));
            }

            return View(c);
        }
        public ActionResult NationalityList(int id)
        {
            var list = db.nationalities.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            nationality_name = li.nationality_name,
                            nationality_id = li.nationality_id,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        // GET: /Nationality/Create
        public ActionResult NationalityEdit(string id)
        {
            var data = (from c in db.nationalities.AsEnumerable()
                        where c.nationality_id == Convert.ToInt32(id)
                        select new
                        {
                            nationality_id = c.nationality_id,
                            nationality_name = c.nationality_name,
                            is_active = c.is_active,
                        }).FirstOrDefault();

            return Json(data);
        }
        // POST: /Nationality/Create
        [HttpPost]
        public ActionResult NationalityCreateEdit(nationality nationality)
        {
            result = nationalityUtil.CreateEditNationality(nationality);
            ViewBag.Title = nationality == null ? "Nationality Create" : "Nationality Edit";
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            return Json(result);
        }
        #endregion

        #endregion

        #region #TYPE
        #region Industry Type
        public ActionResult IndustryTypeIndex(string id)
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Industry Type");
            industry_type c = new industry_type();
            if (id != null && id != "")
            {

                c = db.industry_type.Find(Convert.ToInt32(id));
            }

            return View(c);
        }
        public ActionResult IndustryTypeList(int id)
        {
            var list = db.industry_type.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            industry_type_name = li.industry_type_name,
                            industry_type_id = li.industry_type_id,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        // GET: /Industry/Create
        public ActionResult IndustryEdit(string id)
        {
            var data = (from c in db.industry_type.AsEnumerable()
                        where c.industry_type_id == Convert.ToInt32(id)
                        select new
                        {
                            industry_type_id = c.industry_type_id,
                            industry_type_name = c.industry_type_name,
                            is_active = c.is_active,
                        }).FirstOrDefault();

            return Json(data);
        }
        [HttpPost]
        public ActionResult IndustryCreateEdit(industry_type industry_type)
        {
            result = industryUtil.CreateEditIndustryType(industry_type);
            ViewBag.Title = industry_type == null ? "Country Create" : "Country Edit";
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            return Json(result);
        }
        #endregion


        #region PAYMENT MODE
        public ActionResult PaymentModeIndex(string id)
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Payment Mode");
            payment_mode c = new payment_mode();
            if (id != null && id != "")
            {

                c = db.payment_mode.Find(Convert.ToInt32(id));
            }

            return View(c);
        }
        public ActionResult PaymentModeList(int id)
        {
            var list = db.payment_mode.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            payment_mode_id = li.payment_mode_id,
                            payment_mode_name = li.payment_mode_name,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        // GET: /Country/Create
        public ActionResult PaymentModeEdit(string id)
        {
            var data = (from c in db.payment_mode.AsEnumerable()
                        where c.payment_mode_id == Convert.ToInt32(id)
                        select new
                        {
                            payment_mode_id = c.payment_mode_id,
                            payment_mode_name = c.payment_mode_name,
                            is_active = c.is_active,
                        }).FirstOrDefault();

            return Json(data);
        }
        // POST: /Country/Create
        [HttpPost]
        public ActionResult PaymentModeCreateEdit(payment_mode paymentmode)
        {
            result = paymentModeUtil.CreateEditPaymentMode(paymentmode);
            ViewBag.Title = paymentmode == null ? "Payment Mode Create" : "Payment Mode Edit";
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            return Json(result);
        }
        #endregion

        #region Duration Days
        public ActionResult DurationDaysIndex(string id)
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Duration Days");
            duration_days c = new duration_days();
            if (id != null && id != "")
            {
                c = db.duration_days.Find(Convert.ToInt32(id));
            }

            return View(c);
        }
        public ActionResult DurationDaysList(int id)
        {
            var list = db.duration_days.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            duration_days_id = li.duration_days_id,
                            duration_days_text = li.duration_days_text,
                            duration_days_value = li.duration_days_value,
                            is_active = li.is_active,
                        }).ToList();
            return Json(data);
        }
        // GET: /Nationality/Create
        public ActionResult DurationDaysEdit(string id)
        {
            var data = (from c in db.duration_days.AsEnumerable()
                        where c.duration_days_id == Convert.ToInt32(id)
                        select new
                        {
                            duration_days_id = c.duration_days_id,
                            duration_days_text = c.duration_days_text,
                            duration_days_value = c.duration_days_value,
                            is_active = c.is_active,
                        }).FirstOrDefault();

            return Json(data);
        }
        // POST: /Nationality/Create
        [HttpPost]
        public ActionResult DurationDaysCreateEdit(duration_days duration_days)
        {
            result = durationDaysUtil.CreateEditDurationDays(duration_days);
            ViewBag.Title = duration_days == null ? "Duration Days Create" : "Duration Days Edit";
            ViewBag.action_name = STUtil.GetListAllActionByController("");
            return Json(result);
        }
        #endregion

        #endregion
        #region Package
        public ActionResult PackageIndex()
        {
            STUtil.SetSessionValue(UserInfo.pageTitle.ToString(), "Package");
            package package = new package();
            ViewBag.duration_days_id = durationDaysUtil.GetDurationDaysSelectList();
            ViewBag.package_id = packageUtil.GetPackage();
            ViewBag.Currency = currencyUtil.GetCurrencyListForDropdown();
            return View(package);
        }
        public ActionResult PackageList(int id)
        {
             
            var list = db.packages.AsEnumerable().ToList();
            var data = (from li in list
                        select new
                        {
                            package_id = li.package_id,
                            package_name = li.package_name,
                            duration_days_text = li.duration_days.duration_days_text,
                            setup_cost = li.setup_cost,
                            per_user_price = li.per_user_price,
                            is_public = li.is_public,
                            is_active = li.is_active,
                            tax_display = li.tax_display,
                            tax_percentage =li.tax_percentage
                        }).ToList();
            return Json(data);
        }
        public ActionResult PackageCreateEdit(package package)
        {
            result = packageUtil.CreateEditPackage(package);
            ViewBag.Title = package == null ? "Package Create" : "Package Edit";
            return Json(result);
        }
        public ActionResult PackageEdit(string id)
        {
            var data = (from c in db.packages.AsEnumerable()
                        where c.package_id == Convert.ToInt32(id)
                        select new
                        {
                            package_id = c.package_id,
                            package_name = c.package_name,
                            duration_days_id = c.duration_days_id,
                            setup_cost = c.setup_cost,
                            per_user_price = c.per_user_price,
                            currency_id = c.currency_id,
                            is_public = c.is_public,
                            is_active = c.is_active,
                            tax_display = c.tax_display,
                            tax_percentage = c.tax_percentage
                        }).FirstOrDefault();
            return Json(data);
        }
        #endregion

    }
}



