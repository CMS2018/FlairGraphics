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
    
    public partial class application_template_placeholder
    {
        public int application_template_placeholder_id { get; set; }
        public string module_name { get; set; }
        public string field_name { get; set; }
        public string db_table_column_name { get; set; }
        public int company_id { get; set; }
        public System.DateTime created_date { get; set; }
        public int created_by { get; set; }
        public Nullable<System.DateTime> updated_date { get; set; }
        public Nullable<int> updated_by { get; set; }
        public bool is_active { get; set; }
    
        public virtual company company { get; set; }
        public virtual user user { get; set; }
        public virtual user user1 { get; set; }
    }
}