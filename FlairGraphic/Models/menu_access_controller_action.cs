//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FlairGraphic.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class menu_access_controller_action
    {
        public int menu_access_controller_action_id { get; set; }
        public int menu_id { get; set; }
        public string controller_name { get; set; }
        public string action_name { get; set; }
        public bool is_active { get; set; }
    
        public virtual menu menu { get; set; }
    }
}