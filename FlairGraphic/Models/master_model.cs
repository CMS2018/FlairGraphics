using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlairGraphic.Base.Models;

namespace FlairGraphic.Models
{
    public enum ApprovedStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
    }
    public enum PaymentStatus
    {

        Pending = 1,
        Paid = 2,
        Cancel = 3,
        Invalid = 4

    }

    #region Country
    public class CountryUtil
    {
        private Result result;
        private BaseEntities db;
        public CountryUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }
        public Result CreateEditCountry(country countryObj)
        {

            try
            {
                result = new Result();
                if (countryObj.country_id > 0)
                {
                    country tempCountry = db.countries.Where(c => c.country_id == countryObj.country_id).SingleOrDefault();
                    tempCountry.country_name = countryObj.country_name;
                    tempCountry.is_active = countryObj.is_active;
                    db.Entry(tempCountry).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "country");

                }
                else
                {
                    db.countries.Add(countryObj);
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "country");
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;

        }

        public Result GetCountryById(int countryId)
        {
            result = new Result();
            try
            {
                if (countryId > 0)
                {
                    result.Object = db.countries.Where(c => c.country_id == countryId).SingleOrDefault();
                }
                else
                {
                    result.MessageType = MessageType.Error;
                    result.Message = "Country Not Found";
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return result;
        }
        public List<SelectListItem> GetCountrySelectList()
        {

            var list = (from c in db.countries.Where(c => c.is_active)
                        orderby c.country_name
                        select new SelectListItem
                        {
                            Text = c.country_name,
                            Value = c.country_id.ToString(),
                        }).ToList();

            return list;
        }
        public Result GetCountryList()
        {
            result = new Result();
            try
            {
                result.Object = db.countries.AsEnumerable().ToList();

            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;
        }
        public country GetCountryByCountryId(int countryId, string countryName)
        {
            return countryId > 0 ?
                db.countries.AsEnumerable().Where(x => x.country_name.ToUpper() == countryName.ToUpper() && x.country_id != countryId).FirstOrDefault()
                : db.countries.AsEnumerable().Where(x => x.country_name.ToUpper() == countryName.ToUpper()).FirstOrDefault();
        }
    }

    #endregion

    #region State
    public class StateUtil
    {
        private Result result;
        private BaseEntities db;
        public StateUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }
        public Result CreateEditState(state stateObj)
        {

            try
            {
                result = new Result();
                if (stateObj.state_id > 0)
                {
                    state tempState = db.states.Where(s => s.state_id == stateObj.state_id).SingleOrDefault();
                    tempState.state_name = stateObj.state_name;
                    tempState.is_active = stateObj.is_active;
                    db.Entry(tempState).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "state");

                }
                else
                {
                    db.states.Add(stateObj);
                    db.SaveChanges();

                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "state");
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;

        }
        public Result GetStateById(int stateId)
        {
            result = new Result();
            try
            {
                if (stateId > 0)
                {

                    result.Object = db.states.Where(s => s.state_id == stateId).SingleOrDefault();

                }
                else
                {
                    result.MessageType = MessageType.Error;
                    result.Message = "State Not Found";
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return result;
        }
        public List<SelectListItem> GetStateSelectList(int countryId)
        {

            var list = (from c in db.states.Where(c => c.is_active && c.country_id == countryId)
                        orderby c.display_order
                        select new SelectListItem
                        {
                            Text = c.state_name,
                            Value = c.state_id.ToString(),
                        }).ToList();

            return list;
        }
        public Result GetStateList()
        {
            result = new Result();
            try
            {
                result.Object = db.states.AsEnumerable().ToList();

            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;
        }
        public state GetStateByName(int StateID, string stateName, int CountryId)
        {
            return StateID > 0 ?
                db.states.AsEnumerable().Where(x => x.state_name.ToUpper() == stateName.ToUpper() && x.country_id == CountryId && x.state_id != StateID).FirstOrDefault()
                : db.states.AsEnumerable().Where(x => x.state_name.ToUpper() == stateName.ToUpper() && x.country_id == CountryId).FirstOrDefault();
        }
        public List<SelectListItem> GetStateName()
        {

            return (from s in db.states
                    select new SelectListItem
                    {
                        Value = s.state_id.ToString(),
                        Text = s.state_name
                    }).ToList();
        }

        public string GetStateNameById(int StateId)
        {
            return db.states.Find(StateId).state_name.ToString();
        }

    }

    #endregion

    #region City
    public class CityUtil
    {

        private Result result;
        private BaseEntities db;
        public CityUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }
        public Result CreateEditCity(city cityObj)
        {

            try
            {
                result = new Result();
                if (cityObj.city_id > 0)
                {
                    city tempCity = db.cities.Where(s => s.city_id == cityObj.city_id).SingleOrDefault();
                    tempCity.city_name = cityObj.city_name;
                    tempCity.is_active = cityObj.is_active;
                    db.Entry(tempCity).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "city");

                }
                else
                {
                    db.cities.Add(cityObj);
                    db.SaveChanges();

                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "city");
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;

        }

        public Result GetCityById(int cityId)
        {
            result = new Result();
            try
            {
                if (cityId > 0)
                {

                    result.Object = db.cities.Where(s => s.city_id == cityId).SingleOrDefault();

                }
                else
                {
                    result.MessageType = MessageType.Error;
                    result.Message = "City Not Found";
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return result;
        }
        public List<SelectListItem> GetCitySelectList(int stateId)
        {

            var list = (from c in db.cities.Where(c => c.is_active && c.state_id == stateId)
                        orderby c.display_order
                        select new SelectListItem
                        {
                            Text = c.city_name,
                            Value = c.city_id.ToString(),
                        }).ToList();

            return list;
        }
        public Result GetCityList()
        {
            result = new Result();
            try
            {
                result.Object = db.cities.AsEnumerable().ToList();
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;
        }
        public city GetCityByName(string cityname, int CityId, int StateID, int CountryId)
        {
            return CityId > 0 ?

            db.cities.AsEnumerable().Where(x => x.city_name.ToUpper() == cityname.ToUpper() && x.state_id == StateID && x.state.country_id == CountryId && x.city_id != CityId).FirstOrDefault()
                : db.cities.AsEnumerable().Where(x => x.city_name.ToUpper() == cityname.ToUpper() && x.state_id == StateID && x.state.country_id == CountryId).FirstOrDefault();
        }
        public List<SelectListItem> GetCityName()
        {

            return (from s in db.cities
                    select new SelectListItem
                    {
                        Value = s.city_id.ToString(),
                        Text = s.city_name
                    }).ToList();
        }
    }

    #endregion

    #region Area
    public class AreaUtil
    {
        private Result result;
        private BaseEntities db;
        public AreaUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }
        public Result CreateEditArea(area areaObj)
        {

            try
            {
                result = new Result();
                if (areaObj.area_id > 0)
                {
                    db.Entry(areaObj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Area");

                }
                else
                {
                    db.areas.Add(areaObj);
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Area");
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        public Result GetAreaById(int AreaId)
        {
            result = new Result();
            try
            {
                if (AreaId > 0)
                {

                    result.Object = db.areas.Where(s => s.area_id == AreaId).SingleOrDefault();

                }
                else
                {
                    result.MessageType = MessageType.Error;
                    result.Message = "Area Not Found";
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return result;
        }
        public List<SelectListItem> GetAreaSelectList(int cityId)
        {

            var list = (from c in db.areas.Where(c => c.is_active && c.city_id == cityId)
                        orderby c.display_order
                        select new SelectListItem
                        {
                            Text = c.area_name,
                            Value = c.area_id.ToString(),
                        }).ToList();

            return list;
        }
        public Result GetAreaList()
        {
            result = new Result();
            try
            {
                result.Object = db.areas.AsEnumerable().ToList();
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;
        }
        public area GetAreaByName(int areaId, string areaName, int countryId, int stateId, int cityId)
        {
            return areaId > 0 ?
                db.areas.AsEnumerable().Where(x => x.area_name.ToUpper() == areaName.ToUpper() && x.city.state.country_id == countryId && x.city.state_id == stateId && x.city_id == cityId && x.area_id != areaId).FirstOrDefault()
                : db.areas.AsEnumerable().Where(x => x.area_name.ToUpper() == areaName.ToUpper() && x.city.state.country_id == countryId && x.city.state_id == stateId && x.city_id == cityId).FirstOrDefault();
        }

    }

    #endregion

    #region SubArea
    public class SubAreaUtil
    {
        private Result result;
        private BaseEntities db;
        public SubAreaUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }
        public Result CreateEditSubArea(sub_area subAreaObj)
        {

            try
            {
                result = new Result();
                if (subAreaObj.sub_area_id > 0)
                {
                    //sub_area tempSubArea = db.sub_area.Where(s => s.sub_area_id == subAreaObj.sub_area_id).SingleOrDefault();
                    //tempSubArea.sub_area_name = subAreaObj.sub_area_name;

                    db.Entry(subAreaObj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Sub Area");

                }
                else
                {
                    db.sub_area.Add(subAreaObj);
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Sub Area");
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        public Result GetSubAreaById(int SubAreaId)
        {
            result = new Result();
            try
            {
                if (SubAreaId > 0)
                {

                    result.Object = db.sub_area.Where(s => s.area_id == SubAreaId).SingleOrDefault();

                }
                else
                {
                    result.MessageType = MessageType.Error;
                    result.Message = "SubArea Not Found";
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return result;
        }
        public List<SelectListItem> GetSubAreaSelectList(int areaId)
        {

            var list = (from c in db.sub_area.Where(c => c.is_active && c.area_id == areaId)
                        orderby c.display_order
                        select new SelectListItem
                        {
                            Text = c.sub_area_name,
                            Value = c.sub_area_id.ToString(),
                        }).ToList();

            return list;
        }
        public Result GetSubAreaList()
        {
            result = new Result();
            try
            {
                result.Object = db.sub_area.AsEnumerable().ToList();
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        public sub_area GetSubAreaByName(string subareaName, int areaId, int subareaId, int countryId, int stateId, int cityId)
        {
            return subareaId > 0 ?
                db.sub_area.AsEnumerable().Where(x => x.sub_area_name.ToUpper() == subareaName.ToUpper() && x.area.city.state.country_id == countryId && x.area.city.state_id == stateId && x.area.city_id == cityId && x.area_id == areaId && x.sub_area_id != subareaId).FirstOrDefault()
                : db.sub_area.AsEnumerable().Where(x => x.sub_area_name.ToUpper() == subareaName.ToUpper() && x.area.city.state.country_id == countryId && x.area.city.state_id == stateId && x.area.city_id == cityId && x.area_id == areaId).FirstOrDefault();
        }
    }

    #endregion

    #region Appointment Type
    public class AppointmentTypeUtil
    {
        private Result result;
        private BaseEntities db;
        public AppointmentTypeUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }

    }

    #endregion

    #region Currency
    public class CurrencyUtil
    {
        private Result result;
        private BaseEntities db;
        public CurrencyUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }


        public Result CreateEditCurrency(currency currencyObj)
        {

            try
            {
                result = new Result();
                if (currencyObj.currency_id > 0)
                {
                    //currency tempCurrency = db.currencies.Where(s => s.currency_id == currencyObj.currency_id).SingleOrDefault();
                    //tempCurrency.currency_name = currencyObj.currency_name;
                    //tempCurrency.currency_symbol = currencyObj.currency_symbol;
                    //tempCurrency.is_active = currencyObj.is_active;
                    db.Entry(currencyObj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Currency");

                }
                else
                {
                    db.currencies.Add(currencyObj);
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Currency");
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;

        }

        public Result GetCurrencyById(int CurrencyId)
        {


            result = new Result();
            try
            {
                if (CurrencyId > 0)
                {

                    result.Object = db.currencies.Where(s => s.currency_id == CurrencyId).SingleOrDefault();

                }
                else
                {
                    result.MessageType = MessageType.Error;
                    result.Message = "Currency Not Found";
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }


            return result;
        }
        public List<SelectListItem> GetCurrencySelectList()
        {

            var list = (from c in db.currencies.Where(c => c.is_active)

                        select new SelectListItem
                        {
                            Text = c.currency_name,
                            Value = c.currency_id.ToString(),
                        }).ToList();

            return list;
        }


        public Result GetCurrencyList()
        {
            result = new Result();
            try
            {
                result.Object = db.currencies.AsEnumerable().ToList();
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;
        }
        public List<SelectListItem> GetCurrencyListForDropdown()
        {
            var list = (from c in db.currencies.Where(c => c.is_active)
                        orderby c.currency_name
                        select new SelectListItem
                        {
                            Text = c.currency_name +"_"+ c.currency_symbol.ToString(),
                            Value = c.currency_id.ToString(),
                        }).ToList();
            return list;
        }
        public currency GetCurrencyByCurrencyId(string contactTypeName, int currencyId)
        {
            return currencyId > 0 ?
                db.currencies.AsEnumerable().Where(x => x.currency_name.ToUpper() == contactTypeName.ToUpper() && x.currency_id != currencyId).FirstOrDefault()
                : db.currencies.AsEnumerable().Where(x => x.currency_name.ToUpper() == contactTypeName.ToUpper()).FirstOrDefault();
        }

    }

    #endregion

    #region DateFormat
    public class DateFormatUtil
    {
        private Result result;
        private BaseEntities db;
        public DateFormatUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }

        public Result CreateEditDateFormat(date_format date_formatObj)
        {

            try
            {
                result = new Result();
                if (date_formatObj.date_format_id > 0)
                {
                    //date_format tempDateFormat = db.date_format.Where(s => s.date_format_id == date_formatObj.date_format_id).SingleOrDefault();
                    //tempDateFormat.date_format_name = date_formatObj.date_format_name;
                    //tempDateFormat.date_format_code_csharp = date_formatObj.date_format_code_csharp;
                    //tempDateFormat.date_format_code_js = date_formatObj.date_format_code_js;
                    //tempDateFormat.is_active = date_formatObj.is_active;

                    db.Entry(date_formatObj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "DateFormat");

                }
                else
                {
                    db.date_format.Add(date_formatObj);
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "DateFormat");
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;

        }

        public Result GetDateFormatById(int DateFormatId)
        {


            result = new Result();
            try
            {
                if (DateFormatId > 0)
                {

                    result.Object = db.date_format.Where(s => s.date_format_id == DateFormatId).SingleOrDefault();

                }
                else
                {
                    result.MessageType = MessageType.Error;
                    result.Message = "Date Format Not Found";
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }


            return result;
        }

        public Result GetDateFormatList()
        {
            result = new Result();
            try
            {
                result.Object = db.date_format.AsEnumerable().ToList();
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        public date_format GetDateFormatByDateFormatId(string dateFormatName, int dateFormatId)
        {
            return dateFormatId > 0 ?
                db.date_format.AsEnumerable().Where(x => x.date_format_name.ToUpper() == dateFormatName.ToUpper() && x.date_format_id != dateFormatId).FirstOrDefault()
                : db.date_format.AsEnumerable().Where(x => x.date_format_name.ToUpper() == dateFormatName.ToUpper()).FirstOrDefault();
        }
    }

    #endregion

    #region TimeFormat
    public class TimeFormatUtil
    {
        private Result result;
        private BaseEntities db;
        public TimeFormatUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }

        public Result CreateEditTimeFormat(time_format time_formatObj)
        {

            try
            {
                result = new Result();
                if (time_formatObj.time_format_id > 0)
                {
                    //date_format tempDateFormat = db.date_format.Where(s => s.date_format_id == date_formatObj.date_format_id).SingleOrDefault();
                    //tempDateFormat.date_format_name = date_formatObj.date_format_name;
                    //tempDateFormat.date_format_code_csharp = date_formatObj.date_format_code_csharp;
                    //tempDateFormat.date_format_code_js = date_formatObj.date_format_code_js;
                    //tempDateFormat.is_active = date_formatObj.is_active;

                    db.Entry(time_formatObj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "TimeFormat");

                }
                else
                {
                    db.time_format.Add(time_formatObj);
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "TimeFormat");
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;

        }

        public Result GetTimeFormatById(int TimeFormatId)
        {


            result = new Result();
            try
            {
                if (TimeFormatId > 0)
                {

                    result.Object = db.time_format.Where(s => s.time_format_id == TimeFormatId).SingleOrDefault();

                }
                else
                {
                    result.MessageType = MessageType.Error;
                    result.Message = "Time Format Not Found";
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }


            return result;
        }

        public Result GetTimeFormatList()
        {
            result = new Result();
            try
            {
                result.Object = db.time_format.AsEnumerable().ToList();
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        public time_format GetTimeFormatByTimeFormatId(string timeFormatName, int timeFormatId)
        {
            return timeFormatId > 0 ?
                db.time_format.AsEnumerable().Where(x => x.time_format_name.ToUpper() == timeFormatName.ToUpper() && x.time_format_id != timeFormatId).FirstOrDefault()
                : db.time_format.AsEnumerable().Where(x => x.time_format_name.ToUpper() == timeFormatName.ToUpper()).FirstOrDefault();
        }
    }
    #endregion

    #region DocumentType
    public class DocumentTypeUtil
    {
        private Result result;
        private BaseEntities db;
        public DocumentTypeUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }


        public Result CreateEditDocumentType(document_type document_typeObj)
        {

            try
            {
                result = new Result();
                if (document_typeObj.document_type_id > 0)
                {
                    //document_type tempDocumentType = db.document_type.Where(s => s.document_type_id == document_typeObj.document_type_id).SingleOrDefault();
                    //tempDocumentType.document_type_name = document_typeObj.document_type_name;
                    //tempDocumentType.is_active = document_typeObj.is_active;
                    db.Entry(document_typeObj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Document Type");

                }
                else
                {
                    db.document_type.Add(document_typeObj);
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Document Type");
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;

        }

        public Result GetDocumentTypeById(int DocumentTypeId)
        {


            result = new Result();
            try
            {
                if (DocumentTypeId > 0)
                {

                    result.Object = db.document_type.Where(s => s.document_type_id == DocumentTypeId).SingleOrDefault();

                }
                else
                {
                    result.MessageType = MessageType.Error;
                    result.Message = "Document Type Not Found";
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }


            return result;
        }
        public List<SelectListItem> GetDocumentTypeSelectList()
        {

            var list = (from c in db.document_type.Where(c => c.is_active)

                        select new SelectListItem
                        {
                            Text = c.document_type_name,
                            Value = c.document_type_id.ToString(),
                        }).ToList();

            return list;
        }


        public Result GetDocumentTypeList()
        {
            result = new Result();
            try
            {
                result.Object = db.document_type.AsEnumerable().ToList();
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        public document_type GetDocumentTypeByDocumentTypeId(string documentTypeName, int documentTypeId)
        {
            return documentTypeId > 0 ?
                db.document_type.AsEnumerable().Where(x => x.document_type_name.ToUpper() == documentTypeName.ToUpper() && x.document_type_id != documentTypeId).FirstOrDefault()
                : db.document_type.AsEnumerable().Where(x => x.document_type_name.ToUpper() == documentTypeName.ToUpper()).FirstOrDefault();
        }
    }

    #endregion

    #region Language
    public class LanguageUtil
    {
        private Result result;
        private BaseEntities db;

        public LanguageUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }

        public Result CreateEditLanguage(language languageObj)
        {

            try
            {
                result = new Result();
                if (languageObj.language_id > 0)
                {
                    //language tempLanguage = db.languages.Where(s => s.language_id == languageObj.language_id).SingleOrDefault();
                    //tempLanguage.language_name = languageObj.language_name;
                    //tempLanguage.is_active = languageObj.is_active;
                    db.Entry(languageObj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Language");

                }
                else
                {
                    db.languages.Add(languageObj);
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Language");
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;

        }

        public Result GetLanguageById(int LanguageId)
        {


            result = new Result();
            try
            {
                if (LanguageId > 0)
                {

                    result.Object = db.languages.Where(s => s.language_id == LanguageId).SingleOrDefault();

                }
                else
                {
                    result.MessageType = MessageType.Error;
                    result.Message = "Language Not Found";
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }


            return result;
        }
        public List<SelectListItem> GetLanguageSelectList()
        {

            var list = (from c in db.languages.Where(c => c.is_active)
                        orderby c.language_name
                        select new SelectListItem
                        {
                            Text = c.language_name,
                            Value = c.language_id.ToString(),
                        }).ToList();

            return list;
        }

        public Result GetLanguageList()
        {
            result = new Result();
            try
            {
                result.Object = db.languages.AsEnumerable().ToList();
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        public language GetLanguageByLanguageId(string languageName, int languageId)
        {
            return languageId > 0 ?
                db.languages.AsEnumerable().Where(x => x.language_name.ToUpper() == languageName.ToUpper() && x.language_id != languageId).FirstOrDefault()
                : db.languages.AsEnumerable().Where(x => x.language_name.ToUpper() == languageName.ToUpper()).FirstOrDefault();
        }

    }

    #endregion

    #region Nationality
    public class NationalityUtil
    {
        private Result result;
        private BaseEntities db;

        public NationalityUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }
        public Result CreateEditNationality(nationality nationalityObj)
        {
            try
            {
                result = new Result();
                if (nationalityObj.nationality_id > 0)
                {
                    //nationality tempNationality = db.nationalities.Where(s => s.nationality_id == nationalityObj.nationality_id).SingleOrDefault();
                    //tempNationality.nationality_name = nationalityObj.nationality_name;
                    //tempNationality.is_active = nationalityObj.is_active;
                    db.Entry(nationalityObj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Nationality");

                }
                else
                {
                    db.nationalities.Add(nationalityObj);
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Nationality");
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;

        }

        public Result GetNationalityById(int NationalityId)
        {


            result = new Result();
            try
            {
                if (NationalityId > 0)
                {

                    result.Object = db.nationalities.Where(s => s.nationality_id == NationalityId).SingleOrDefault();

                }
                else
                {
                    result.MessageType = MessageType.Error;
                    result.Message = "Nationality Not Found";
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }


            return result;
        }
        public List<SelectListItem> GetNationalitySelectList()
        {

            var list = (from c in db.nationalities.Where(c => c.is_active)
                        orderby c.nationality_name
                        select new SelectListItem
                        {
                            Text = c.nationality_name,
                            Value = c.nationality_id.ToString(),
                        }).ToList();

            return list;
        }
        public Result GetNationalityList()
        {
            result = new Result();
            try
            {
                result.Object = db.nationalities.AsEnumerable().ToList();
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;
        }
        public nationality GetNationalityByNationalityId(string nationalityName, int nationalityId)
        {
            return nationalityId > 0 ?
                db.nationalities.AsEnumerable().Where(x => x.nationality_name.ToUpper() == nationalityName.ToUpper() && x.nationality_id != nationalityId).FirstOrDefault()
                : db.nationalities.AsEnumerable().Where(x => x.nationality_name.ToUpper() == nationalityName.ToUpper()).FirstOrDefault();
        }
    }

    #endregion

    #region Religion
    public class ReligionUtil
    {
        private Result result;
        private BaseEntities db;

        public ReligionUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }
        public Result CreateEditReligion(religion religionObj)
        {

            try
            {
                result = new Result();
                if (religionObj.religion_id > 0)
                {
                    //religion tempReligion = db.religions.Where(s => s.religion_id == religionObj.religion_id).SingleOrDefault();
                    //tempReligion.religion_name = religionObj.religion_name;
                    //tempReligion.is_active = religionObj.is_active;
                    db.Entry(religionObj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Religion");

                }
                else
                {
                    db.religions.Add(religionObj);
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Religion");
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;

        }

        public Result GetReligionById(int ReligionId)
        {


            result = new Result();
            try
            {
                if (ReligionId > 0)
                {

                    result.Object = db.religions.Where(s => s.religion_id == ReligionId).SingleOrDefault();

                }
                else
                {
                    result.MessageType = MessageType.Error;
                    result.Message = "Religion Not Found";
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }


            return result;
        }
        public List<SelectListItem> GetReligionSelectList()
        {

            var list = (from c in db.religions.Where(c => c.is_active)
                        orderby c.religion_name
                        select new SelectListItem
                        {
                            Text = c.religion_name,
                            Value = c.religion_id.ToString(),
                        }).ToList();

            return list;
        }

        public Result GetReligionList()
        {
            result = new Result();
            try
            {
                result.Object = db.religions.AsEnumerable().ToList();
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        public religion GetReligionByReligionId(string religionName, int religionId)
        {
            return religionId > 0 ?
                db.religions.AsEnumerable().Where(x => x.religion_name.ToUpper() == religionName.ToUpper() && x.religion_id != religionId).FirstOrDefault()
                : db.religions.AsEnumerable().Where(x => x.religion_name.ToUpper() == religionName.ToUpper()).FirstOrDefault();
        }
    }

    #endregion

    #region industry_type
    public class IndustryUtil
    {
        private Result result;
        private BaseEntities db;
        public IndustryUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }
        public Result CreateEditIndustryType(industry_type industryObj)
        {

            try
            {
                result = new Result();
                if (industryObj.industry_type_id > 0)
                {
                    industry_type tempIndustry = db.industry_type.Where(c => c.industry_type_id == industryObj.industry_type_id).SingleOrDefault();
                    tempIndustry.industry_type_name = industryObj.industry_type_name;
                    tempIndustry.is_active = industryObj.is_active;
                    db.Entry(tempIndustry).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "industry");

                }
                else
                {
                    db.industry_type.Add(industryObj);
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "industry");
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;

        }


        public Result GetIndustryById(int industryid)
        {
            result = new Result();
            try
            {
                if (industryid > 0)
                {
                    result.Object = db.industry_type.Where(c => c.industry_type_id == industryid).SingleOrDefault();
                }
                else
                {
                    result.MessageType = MessageType.Error;
                    result.Message = "Industry Not Found";
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return result;
        }
        public List<SelectListItem> GetIndustrySelectList()
        {

            var list = (from c in db.industry_type.Where(c => c.is_active)
                        orderby c.industry_type_name
                        select new SelectListItem
                        {
                            Text = c.industry_type_name,
                            Value = c.industry_type_id.ToString(),
                        }).ToList();

            return list;
        }
        public Result GetIndustryList()
        {
            result = new Result();
            try
            {
                result.Object = db.industry_type.AsEnumerable().ToList();

            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;
        }
        public industry_type GetIndustryByIndustryId(int industryId, string industryName)
        {
            return industryId > 0 ?
                  db.industry_type.AsEnumerable().Where(x => x.industry_type_name.ToUpper() == industryName.ToUpper() && x.industry_type_id != industryId).FirstOrDefault()
                  : db.industry_type.AsEnumerable().Where(x => x.industry_type_name.ToUpper() == industryName.ToUpper()).FirstOrDefault();
        }
    }
    #endregion

    #region PaymentMode
    public class PaymentModeUtil
    {
        private Result result;
        private BaseEntities db;
        public PaymentModeUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }
        public Result GetPaymentModeList()
        {
            result = new Result();
            try
            {
                result.Object = db.payment_mode.AsEnumerable().ToList();

            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;
        }
        public Result CreateEditPaymentMode(payment_mode paymentmodeObj)
        {

            try
            {
                result = new Result();
                if (paymentmodeObj.payment_mode_id > 0)
                {
                    payment_mode tempPaymentMode = db.payment_mode.Where(c => c.payment_mode_id == paymentmodeObj.payment_mode_id).SingleOrDefault();
                    tempPaymentMode.payment_mode_name = paymentmodeObj.payment_mode_name;
                    tempPaymentMode.is_active = paymentmodeObj.is_active;
                    db.Entry(tempPaymentMode).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Payment Mode");

                }
                else
                {
                    db.payment_mode.Add(paymentmodeObj);
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Payment Mode");
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;

        }
        public payment_mode GetPaymentModeByPaymentId(string paymentmodename, int paymentmodeId)
        {
            return paymentmodeId > 0 ?
                  db.payment_mode.AsEnumerable().Where(x => x.payment_mode_name.ToUpper() == paymentmodename.ToUpper() && x.payment_mode_id != paymentmodeId).FirstOrDefault()
                  : db.payment_mode.AsEnumerable().Where(x => x.payment_mode_name.ToUpper() == paymentmodename.ToUpper()).FirstOrDefault();
        }


    }
    #endregion

    #region Source
    public class SourceUtil
    {
        private Result result;
        private BaseEntities db;
        public SourceUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }
    }

    #endregion

    #region ApplicationTemplatePlaceHolder
    public class ApplicationTemplatePlaceHolderUtil
    {
        private Result result;
        private BaseEntities db;
        public ApplicationTemplatePlaceHolderUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }
        public Result CreateEditApplicationTemplatePlaceHolder(application_template_placeholder application_template_placeholderObj)
        {

            try
            {
                result = new Result();

                if (application_template_placeholderObj.application_template_placeholder_id > 0)
                {
                    application_template_placeholder s = db.application_template_placeholder.Find(application_template_placeholderObj.application_template_placeholder_id);
                    s.module_name = application_template_placeholderObj.module_name;
                    s.is_active = application_template_placeholderObj.is_active;
                    s.field_name = application_template_placeholderObj.field_name;
                    s.db_table_column_name = application_template_placeholderObj.db_table_column_name;
                    db.Entry(s).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Application Template placeholder");

                }
                else
                {
                    db.application_template_placeholder.Add(application_template_placeholderObj);
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Application Template Placeholder");
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;

        }
        public Result GetApplicationTemplatePlaceHolderById(int ApplicationTemplatePlaceHolderId)
        {


            result = new Result();
            try
            {
                if (ApplicationTemplatePlaceHolderId > 0)
                {

                    result.Object = db.application_template_placeholder.Where(s => s.application_template_placeholder_id == ApplicationTemplatePlaceHolderId).SingleOrDefault();

                }
                else
                {
                    result.MessageType = MessageType.Error;
                    result.Message = "ApplicationTemplatePlaceHolder Not Found";
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }


            return result;
        }
        public List<SelectListItem> GetApplicationTemplatePlaceHoldereSelectList()
        {
            var companyID = SessionUtil.GetCompanyID();
            var list = (from c in db.application_template_placeholder.Where(c => c.is_active && c.company_id == companyID)
                        orderby c.db_table_column_name
                        select new SelectListItem
                        {
                            Text = c.db_table_column_name,
                            Value = c.application_template_placeholder_id.ToString(),
                        }).ToList();

            return list;
        }
        public application_template_placeholder GetApplicationTemplatePlaceHoldereIdByName(string dbTableColumnName, int applicationtemplateplaceholderId)
        {
            return applicationtemplateplaceholderId > 0 ?
                db.application_template_placeholder.AsEnumerable().Where(x => x.db_table_column_name.ToUpper() == dbTableColumnName.ToUpper() && x.application_template_placeholder_id != applicationtemplateplaceholderId).FirstOrDefault()
                : db.application_template_placeholder.AsEnumerable().Where(x => x.db_table_column_name.ToUpper() == dbTableColumnName.ToUpper()).FirstOrDefault();
        }
    }

    #endregion

    #region Gender
    public class GenderUtil
    {
        private Result result;
        private BaseEntities db;
        public GenderUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }


        public Result CreateEditGender(gender genderObj)
        {

            try
            {
                result = new Result();
                if (genderObj.gender_id > 0)
                {
                    gender tempGender = db.genders.Where(s => s.gender_id == genderObj.gender_id).SingleOrDefault();
                    tempGender.gender_name = genderObj.gender_name;

                    tempGender.is_active = genderObj.is_active;

                    db.Entry(tempGender).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Gender");

                }
                else
                {
                    db.genders.Add(genderObj);
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Gender");
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;

        }

        public Result GetGenderById(int GenderId)
        {


            result = new Result();
            try
            {
                if (GenderId > 0)
                {

                    result.Object = db.genders.Where(s => s.gender_id == GenderId).SingleOrDefault();

                }
                else
                {
                    result.MessageType = MessageType.Error;
                    result.Message = "Gender Not Found";
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }


            return result;
        }

        public List<SelectListItem> GetGenderSelectList()
        {

            var list = (from c in db.genders.Where(c => c.is_active)
                        orderby c.gender_name descending
                        select new SelectListItem
                        {
                            Text = c.gender_name,
                            Value = c.gender_id.ToString(),
                        }).ToList();
            return list;
        }

        public Result GetGenderList()
        {
            result = new Result();
            try
            {
                result.Object = db.genders.AsEnumerable().ToList();
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

    #region Duration Days
    public class DurationDaysUtil
    {
        private Result result;
        private BaseEntities db;
        public DurationDaysUtil()
        {
            result = new Result();
            db = new BaseEntities();
        }

        public Result CreateEditDurationDays(duration_days duration_daysObj)
        {

            try
            {
                result = new Result();
                if (duration_daysObj.duration_days_id > 0)
                {
                    //religion tempReligion = db.religions.Where(s => s.religion_id == religionObj.religion_id).SingleOrDefault();
                    //tempReligion.religion_name = religionObj.religion_name;
                    //tempReligion.is_active = religionObj.is_active;
                    db.Entry(duration_daysObj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Duration Days");

                }
                else
                {
                    db.duration_days.Add(duration_daysObj);
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Duration Days");
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;

        }

        public Result GetDurationDaysById(int DurationDaysId)
        {


            result = new Result();
            try
            {
                if (DurationDaysId > 0)
                {

                    result.Object = db.duration_days.Where(s => s.duration_days_id == DurationDaysId).SingleOrDefault();

                }
                else
                {
                    result.MessageType = MessageType.Error;
                    result.Message = "Duration Days Not Found";
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }


            return result;
        }

        public List<SelectListItem> GetDurationDaysSelectList()
        {
            var list = (from c in db.duration_days.Where(c => c.is_active)
                        orderby c.duration_days_text descending
                        select new SelectListItem
                        {
                            Text = c.duration_days_text,
                            Value = c.duration_days_id.ToString(),
                        }).ToList();
            return list;
        }

        public Result GetDurationDaysList()
        {
            result = new Result();
            try
            {
                result.Object = db.religions.AsEnumerable().ToList();
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }

            return result;
        }

        public duration_days GetDurationDaysByDurationId(string durationdaystext, string durationdaysvalue, int durationdayId)
        {
            return durationdayId > 0 ?
                db.duration_days.AsEnumerable().Where(x => x.duration_days_text.ToUpper() == durationdaystext.ToUpper() && x.duration_days_value.ToUpper() == durationdaysvalue.ToUpper() && x.duration_days_id != durationdayId).FirstOrDefault()
                : db.duration_days.AsEnumerable().Where(x => x.duration_days_text.ToUpper() == durationdaystext.ToUpper() && x.duration_days_value.ToUpper() == durationdaysvalue.ToUpper()).FirstOrDefault();
        }
        public duration_days GetDurationTextByDurationId(string durationdaystext, int durationdayId)
        {
            return durationdayId > 0 ?
                db.duration_days.AsEnumerable().Where(x => x.duration_days_text.ToUpper() == durationdaystext.ToUpper() && x.duration_days_id != durationdayId).FirstOrDefault()
                : db.duration_days.AsEnumerable().Where(x => x.duration_days_text.ToUpper() == durationdaystext.ToUpper()).FirstOrDefault();
        }
    }


    #endregion

    #region Score Column
    //public class ScoreColumnUtil
    //{
    //    private Result result;
    //    private BaseEntities db;
    //    public ScoreColumnUtil()
    //    {
    //        result = new Result();
    //        db = new BaseEntities();
    //    }

    //    public Result CreateEditDurationDays(score_column score_columnObj)
    //    {

    //        try
    //        {
    //            result = new Result();
    //            if (score_columnObj.score_column_id > 0)
    //            {
    //                //religion tempReligion = db.religions.Where(s => s.religion_id == religionObj.religion_id).SingleOrDefault();
    //                //tempReligion.religion_name = religionObj.religion_name;
    //                //tempReligion.is_active = religionObj.is_active;
    //                db.Entry(score_columnObj).State = System.Data.Entity.EntityState.Modified;
    //                db.SaveChanges();
    //                result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Score Column");

    //            }
    //            else
    //            {
    //                db.score_column.Add(score_columnObj);
    //                db.SaveChanges();
    //                result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "score column");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            result.MessageType = MessageType.Error;
    //            result.Message = ex.Message;
    //        }

    //        return result;

    //    }

    //    public Result GetScoreColumnById(int ScoreColumnId)
    //    {


    //        result = new Result();
    //        try
    //        {
    //            if (ScoreColumnId > 0)
    //            {

    //                result.Object = db.score_column.Where(s => s.score_column_id == ScoreColumnId).SingleOrDefault();

    //            }
    //            else
    //            {
    //                result.MessageType = MessageType.Error;
    //                result.Message = "Score Column Not Found";
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            result.MessageType = MessageType.Error;
    //            result.Message = ex.Message;
    //        }


    //        return result;
    //    }
    //    public List<SelectListItem> GetScoreColumnSelectList()
    //    {
    //        var list = (from c in db.score_column.Where(c => c.is_active)
    //                    orderby c.score_column_name descending
    //                    select new SelectListItem
    //                    {
    //                        Text = c.score_column_name,
    //                        Value = c.score_column_id.ToString(),
    //                    }).ToList();
    //        return list;
    //    }

    //    public Result GetScoreColumnList()
    //    {
    //        result = new Result();
    //        try
    //        {
    //            result.Object = db.score_column.AsEnumerable().ToList();
    //        }
    //        catch (Exception ex)
    //        {
    //            result.MessageType = MessageType.Error;
    //            result.Message = ex.Message;
    //        }

    //        return result;
    //    }


    //    public score_column GetScoreColumnByScoreId(string scorecolumnname, int scorecolumnid)
    //    {
    //        return scorecolumnid > 0 ?
    //            db.score_column.AsEnumerable().Where(x => x.score_column_name.ToUpper() == scorecolumnname.ToUpper() && x.score_column_id != scorecolumnid).FirstOrDefault()
    //            : db.score_column.AsEnumerable().Where(x => x.score_column_name.ToUpper() == scorecolumnname.ToUpper()).FirstOrDefault();
    //    }

    //}

    #endregion

    #region Package
    public class PackageUtil
    {
        private Result result;
        private BaseEntities db;
        public PackageUtil()
        {
            this.result = new Result();
            this.db = new BaseEntities();
        }
        public List<SelectListItem> GetPackage()
        {
            return (from pt in db.packages.AsEnumerable().Where(x => x.is_active).AsEnumerable()
                    orderby pt.package_name
                    select new SelectListItem
                    {
                        Value = pt.package_id.ToString(),
                        Text = pt.package_name
                    }).OrderBy(x => x.Text).ToList();
        }
        public Result Post_Package_Subscription(package_subscription obj)
        {
            try
            {
                if (obj.package_subscription_id > 0)
                {
                    db = new BaseEntities();
                    package_subscription package_subscription = db.package_subscription.Find(obj.package_subscription_id);
                    //package_subscription.payment_detail = obj.payment_detail;
                    //package_subscription.cancel_remark = obj.cancel_remark;
                    package_subscription.payment_status_id = obj.payment_status_id;
                    db.Entry(package_subscription).State = System.Data.Entity.EntityState.Modified;
                    result.Id = package_subscription.company_id;
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Package");
                }
                else
                {
                    db.package_subscription.Add(obj);
                    result.Id = obj.company_id;
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Package");
                }

                if (RoleUtil.IsInRole(Role.SuperAdmin))
                {
                    STUtil.SetSessionValue(UserInfo.IsCompanyAddUpdate.ToString(), "0");
                    db.SaveChanges();
                    STUtil.SetSessionValue(UserInfo.IsCompanyAddUpdate.ToString(), "1");
                }
                else
                {
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                STUtil.SetSessionValue(UserInfo.IsCompanyAddUpdate.ToString(), "1");
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return result;
        }
        public Result CreateEditPackage(package packageObj)
        {
            try
            {
                result = new Result();
                if (packageObj.package_id > 0)
                {
                    package tempCountry = db.packages.Where(c => c.package_id == packageObj.package_id).SingleOrDefault();
                    tempCountry.package_name = packageObj.package_name;
                    tempCountry.duration_days_id = packageObj.duration_days_id;
                    tempCountry.setup_cost = packageObj.setup_cost;
                    tempCountry.per_user_price = packageObj.per_user_price;
                    tempCountry.is_public = packageObj.is_public;
                    tempCountry.is_active = packageObj.is_active;
                    tempCountry.currency_id = packageObj.currency_id;
                    tempCountry.tax_display = packageObj.tax_display;
                    tempCountry.tax_percentage = packageObj.tax_percentage;
                    db.Entry(tempCountry).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "package");
                }
                else
                {
                    db.packages.Add(packageObj);
                    db.SaveChanges();
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "package");
                }
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return result;
        }
        public package GetPackageByPackageId(int packageId, string packageName)
        {
            return packageId > 0 ?
                db.packages.AsEnumerable().Where(x => x.package_name.ToUpper() == packageName.ToUpper() && x.package_id != packageId).FirstOrDefault()
                : db.packages.AsEnumerable().Where(x => x.package_name.ToUpper() == packageName.ToUpper()).FirstOrDefault();
        }
    }
    #endregion

}
