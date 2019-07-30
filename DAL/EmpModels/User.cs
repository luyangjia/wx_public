namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User.User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            UserClaims = new HashSet<UserClaim>();
            UserLogins = new HashSet<UserLogin>();
            Buildings = new HashSet<Building>();
            Organizations = new HashSet<Organization>();
            Roles = new HashSet<Role>();
            MonitoringConfigs = new HashSet<MonitoringConfig>();
            MessageRecords = new HashSet<MessageRecord>();
            Payer = new HashSet<HistoryBill>();
            Receiver = new HashSet<HistoryBill>();
            UserAccount = new HashSet<UserAccount>();
            FeedbackUser = new HashSet<Feedback>();
            FeedbackHandleUser = new HashSet<Feedback>();
            BillOperators = new HashSet<HistoryBill>();

            //ActivityRecordCurrentOperator = new HashSet<ActivityRecord>();
            //MaintenanceUser = new HashSet<Maintenance>();
            //PurchaseCurrentOperator = new HashSet<Purchase>();
            //PurchaseApprover = new HashSet<Purchase>();

            ActivityRecords = new HashSet<ActivityRecord>();
            MaintenanceUsers = new HashSet<Maintenance>();
            MaintenanceApprovers = new HashSet<Maintenance>();
            MaintenanceOperators = new HashSet<Maintenance>();
            PurchaseCurrentOperators = new HashSet<Purchase>();
            PurchaseApprovers = new HashSet<Purchase>();

            LogInfos = new HashSet<LogInfo>();
            SceneModeMeters = new HashSet<SceneModeMeter>();
            Groups = new HashSet<Group>();
            RatedParameterDetails = new HashSet<RatedParameterDetail>();

            RequestForOvertimeApplicantors = new HashSet<RequestForOvertime>();
            RequestForOvertimeApprovers = new HashSet<RequestForOvertime>();
            BuildingMeterTypeUsers = new HashSet<BuildingMeterTypeUser>();

            UserExtensions = new HashSet<UserExtension>();
            ConfigDetails = new HashSet<ConfigDetail>();
        }

        public string Id { get; set; }

        public int? OrganizationId { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [StringLength(256)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        public bool IsResignOrGraduate { get; set; }

        public string ForeignId { get; set; }
        public bool? IsRightInfo { get; set; }
        public string IdentityNo { get; set; }

        [Display(Name = "入学时间， 入职时间")]
        public DateTime EnrollDate { get; set; }

        public string StaffNo { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        [StringLength(256)]
        public string FullName { get; set; }

        [StringLength(32)]
        public string QQ { get; set; }

        [StringLength(32)]
        public string WeChat { get; set; }

        public int? UserType { get; set; }

        public bool? Gender { get; set; }

        public virtual Dictionary EnergyUserType { get; set; }

        [Display(Name = "反馈用户")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> FeedbackUser { get; set; }

        [Display(Name = "反馈处理用户")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> FeedbackHandleUser { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserClaim> UserClaims { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserLogin> UserLogins { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Building> Buildings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Organization> Organizations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<MonitoringConfig> MonitoringConfigs { get; set; }

        public virtual ICollection<MessageRecord> MessageRecords{ get; set; }

        public virtual ICollection<HistoryBill> Payer { get; set; }

        public virtual ICollection<HistoryBill> Receiver { get; set; }

        public virtual ICollection<UserAccount> UserAccount { get; set; }

        public virtual ICollection<HistoryBill> BillOperators { get; set; }


        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ActivityRecord> ActivityRecordCurrentOperator { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Maintenance> MaintenanceUser { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Purchase> PurchaseCurrentOperator { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Purchase> PurchaseApprover { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ActivityRecord> ActivityRecords { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Maintenance> MaintenanceUsers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Maintenance> MaintenanceApprovers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Maintenance> MaintenanceOperators { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Purchase> PurchaseCurrentOperators { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Purchase> PurchaseApprovers { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SceneModeMeter> SceneModeMeters { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Group> Groups { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RatedParameterDetail> RatedParameterDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogInfo> LogInfos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestForOvertime> RequestForOvertimeApplicantors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestForOvertime> RequestForOvertimeApprovers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuildingMeterTypeUser> BuildingMeterTypeUsers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserExtension> UserExtensions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConfigDetail> ConfigDetails { get; set; }
    }
}
