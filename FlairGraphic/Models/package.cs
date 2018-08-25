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
    
    public partial class package
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public package()
        {
            this.package_subscription = new HashSet<package_subscription>();
        }
    
        public int package_id { get; set; }
        public string package_name { get; set; }
        public int duration_days_id { get; set; }
        public decimal setup_cost { get; set; }
        public decimal per_user_price { get; set; }
        public string tax_display { get; set; }
        public decimal tax_percentage { get; set; }
        public bool is_public { get; set; }
        public int currency_id { get; set; }
        public System.DateTime created_date { get; set; }
        public int created_by { get; set; }
        public Nullable<System.DateTime> updated_date { get; set; }
        public Nullable<int> updated_by { get; set; }
        public bool is_active { get; set; }
    
        public virtual currency currency { get; set; }
        public virtual duration_days duration_days { get; set; }
        public virtual user user { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<package_subscription> package_subscription { get; set; }
        public virtual user user1 { get; set; }
    }
}
