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
    
    public partial class application_template_type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public application_template_type()
        {
            this.application_template = new HashSet<application_template>();
        }
    
        public int application_template_type_id { get; set; }
        public string application_template_type_name { get; set; }
        public bool is_active { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<application_template> application_template { get; set; }
    }
}
