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
    
    public partial class lemination_type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public lemination_type()
        {
            this.jobs = new HashSet<job>();
        }
    
        public int lemination_type_id { get; set; }
        public string lemination_type_name { get; set; }
        public int company_id { get; set; }
        public bool is_active { get; set; }
    
        public virtual company company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<job> jobs { get; set; }
        public virtual lemination_type lemination_type1 { get; set; }
        public virtual lemination_type lemination_type2 { get; set; }
    }
}