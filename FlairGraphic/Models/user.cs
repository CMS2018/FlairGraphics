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
    
    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            this.app_error_log = new HashSet<app_error_log>();
            this.application_template = new HashSet<application_template>();
            this.application_template1 = new HashSet<application_template>();
            this.application_template_placeholder = new HashSet<application_template_placeholder>();
            this.application_template_placeholder1 = new HashSet<application_template_placeholder>();
            this.categories = new HashSet<category>();
            this.categories1 = new HashSet<category>();
            this.companies = new HashSet<company>();
            this.company_holiday = new HashSet<company_holiday>();
            this.company_holiday1 = new HashSet<company_holiday>();
            this.company_work_days = new HashSet<company_work_days>();
            this.company_work_days1 = new HashSet<company_work_days>();
            this.jobs = new HashSet<job>();
            this.job_assignment = new HashSet<job_assignment>();
            this.job_attachment = new HashSet<job_attachment>();
            this.packages = new HashSet<package>();
            this.packages1 = new HashSet<package>();
            this.package_subscription = new HashSet<package_subscription>();
            this.package_subscription1 = new HashSet<package_subscription>();
            this.package_subscription2 = new HashSet<package_subscription>();
            this.package_subscription3 = new HashSet<package_subscription>();
            this.users1 = new HashSet<user>();
            this.users11 = new HashSet<user>();
            this.users12 = new HashSet<user>();
        }
    
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string login_id { get; set; }
        public string email_id { get; set; }
        public string mobile { get; set; }
        public byte[] password { get; set; }
        public string gender { get; set; }
        public Nullable<int> report_to { get; set; }
        public string user_photo { get; set; }
        public Nullable<System.DateTime> last_login_date { get; set; }
        public string activation_link { get; set; }
        public string reset_password_link { get; set; }
        public Nullable<System.DateTime> reset_password_link_expire_date_time { get; set; }
        public string activation_reset_key { get; set; }
        public Nullable<int> parent_user_id { get; set; }
        public int role_id { get; set; }
        public int role_bit { get; set; }
        public bool is_account_locked { get; set; }
        public int password_failed_attempt { get; set; }
        public string imei_number { get; set; }
        public string token_number { get; set; }
        public string os_version { get; set; }
        public string user_signature { get; set; }
        public int create_work_order_access_id { get; set; }
        public int view_work_order_access_id { get; set; }
        public bool is_change_requester { get; set; }
        public bool is_service_provider { get; set; }
        public Nullable<int> theme_color_id { get; set; }
        public int company_id { get; set; }
        public System.DateTime created_date { get; set; }
        public int created_by { get; set; }
        public Nullable<System.DateTime> updated_date { get; set; }
        public Nullable<int> updated_by { get; set; }
        public bool is_active { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<app_error_log> app_error_log { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<application_template> application_template { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<application_template> application_template1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<application_template_placeholder> application_template_placeholder { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<application_template_placeholder> application_template_placeholder1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<category> categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<category> categories1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<company> companies { get; set; }
        public virtual company company { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<company_holiday> company_holiday { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<company_holiday> company_holiday1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<company_work_days> company_work_days { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<company_work_days> company_work_days1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<job> jobs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<job_assignment> job_assignment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<job_attachment> job_attachment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<package> packages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<package> packages1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<package_subscription> package_subscription { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<package_subscription> package_subscription1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<package_subscription> package_subscription2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<package_subscription> package_subscription3 { get; set; }
        public virtual role role { get; set; }
        public virtual theme_color theme_color { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user> users1 { get; set; }
        public virtual user user1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user> users11 { get; set; }
        public virtual user user2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user> users12 { get; set; }
        public virtual user user3 { get; set; }
    }
}
