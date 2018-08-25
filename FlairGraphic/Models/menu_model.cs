using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlairGraphic.Base.Models;

namespace FlairGraphic.Models
{
    #region Business
    #region Menu
    public class MenuUtil
    {
        private Result result;
        private BaseEntities db;
        public MenuUtil()
        {
            this.result = new Result();
            this.db = new BaseEntities();
        }
        public List<SelectListItem> GetMenu(bool isSearch)
        {
            if (isSearch)
            {
                return (from m in db.menus.AsEnumerable().Where(x => x.is_active)
                        where m.menu_parent_id == null //controller_name == null && m.action_name == null && m.param_id == null
                        orderby m.sequence_order
                        select new SelectListItem
                        {
                            Value = m.menu_id.ToString(),
                            Text = m.menu_name + " - " + m.sequence_order
                        }).ToList();
            }
            else
            {
                return (from m in db.menus.AsEnumerable().Where(x => x.is_active)
                        orderby m.menu_name
                        select new SelectListItem
                        {
                            Value = m.menu_id.ToString(),
                            Text = m.menu_name
                        }).ToList();

            }

        }
        public List<SelectListItem> GetController()
        {
            var list = (from m in STUtil.GetControllerNames()

                        select new SelectListItem
                        {
                            Value = m.Replace("Controller", ""),
                            Text = m.Replace("Controller", "")
                        }).ToList();

            return list;
        }
        public Result ListMenu(string searchString)
        {
            try
            {
                var menuList = (from m in db.menus.AsEnumerable()
                                orderby m.sequence_order
                                select new
                                {
                                    //menu_parent_name = m.menu_parent_id != null ? m.menu2.menu_name : "",
                                    menu_name = m.menu_name,
                                    controller_name = m.controller_name,
                                    action_name = m.action_name,
                                    icon = m.icon,
                                    sequence_order = m.sequence_order,
                                    menu_parent_id = m.menu_parent_id,
                                    menu_id = m.menu_id,
                                    is_active = m.is_active
                                }).ToList();
                if (!string.IsNullOrEmpty(searchString))
                {

                    var info = searchString.Split(',');
                    int menu_ddl_id = info[0] != "" ? Convert.ToInt32(info[0]) : 0;
                    var txtController = info[1];
                    var txtActionName = info[2];

                    menuList = menu_ddl_id > 0 ? menuList.Where(x => x.menu_id == menu_ddl_id || x.menu_parent_id == menu_ddl_id).OrderByDescending(x => x.menu_id).ToList() : menuList;
                    if (txtController != "")
                    {
                        menuList = menuList.Where(x => x.controller_name != null && x.controller_name.ToUpper().Trim().Contains(txtController.ToUpper().Trim())).ToList();
                    }
                    if (txtActionName != "")
                    {
                        menuList = menuList.Where(x => x.action_name != null && x.action_name.ToUpper().Trim().Contains(txtActionName.ToUpper().Trim())).ToList();

                    }
                }
                result.Object = menuList;
            }
            catch (Exception ex)
            {
                result.MessageType = MessageType.Error;
                result.Message = ex.Message;
            }
            return result;
        }
        public Result PostMenuCreate(menu menu)
        {
            try
            {
                db = new BaseEntities();
                int menu_id = Convert.ToInt32(menu.menu_id);
                if (menu_id > 0)
                {
                    //menu.is_active = menu.is_active;
                    db.Entry(menu).State = System.Data.Entity.EntityState.Modified;
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_UPDATE, "Menu");
                }
                else
                {
                    db.menus.Add(menu);
                    result.Message = string.Format(BaseConst.MSG_SUCCESS_CREATE, "Menu");
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
    }
    #endregion
    #endregion
}