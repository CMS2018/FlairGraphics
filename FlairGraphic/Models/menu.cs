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
    
    public partial class menu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public menu()
        {
            this.menu_access_controller_action = new HashSet<menu_access_controller_action>();
            this.menu1 = new HashSet<menu>();
            this.role_menu = new HashSet<role_menu>();
        }
    
        public int menu_id { get; set; }
        public Nullable<int> menu_parent_id { get; set; }
        public string menu_name { get; set; }
        public string controller_name { get; set; }
        public string action_name { get; set; }
        public string param_id { get; set; }
        public string icon { get; set; }
        public Nullable<int> sequence_order { get; set; }
        public string title_name { get; set; }
        public string sludge_name { get; set; }
        public bool is_for_super_admin { get; set; }
        public bool is_for_admin { get; set; }
        public bool is_for_normal_user { get; set; }
        public bool is_active { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<menu_access_controller_action> menu_access_controller_action { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<menu> menu1 { get; set; }
        public virtual menu menu2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<role_menu> role_menu { get; set; }
    }
}
