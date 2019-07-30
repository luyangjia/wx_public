namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Foundation.Dictionary")]
    public partial class Dictionary
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Dictionary()
        {
            OrganizationByType = new HashSet<Organization>();
            BuildingsByCategory = new HashSet<Building>();
            BuildingsByType = new HashSet<Building>();
            Coordinates = new HashSet<Coordinate>();
            Brands = new HashSet<Brand>();
            MeterHourlyResults = new HashSet<MeterHourlyResult>();
            MeterDailyResults = new HashSet<MeterDailyResult>();
            MeterMonthlyResults = new HashSet<MeterMonthlyResult>();
            MetersByEnergyCategory = new HashSet<Meter>();
            MetersByType = new HashSet<Meter>();
            Parameters = new HashSet<Parameter>();
            AttachmentsByFormat = new HashSet<Attachment>();
            AttachmentsByType = new HashSet<Attachment>();
            MessageBySourceType = new HashSet<Message>();
            MessagesByType = new HashSet<Message>();
            MeterStatusByMessageType = new HashSet<MeterStatus>();
            SubscribesByMessageType = new HashSet<Subscribe>();
            SubscribesByTargetType = new HashSet<Subscribe>();
            SubscribesByType = new HashSet<Subscribe>();
            MonitoringConfigsByAlarmLevel = new HashSet<MonitoringConfig>();
            MonitoringConfigsByConfigType= new HashSet<MonitoringConfig>();
            MonitoringConfigsByCycleType = new HashSet<MonitoringConfig>();
            MonitoringConfigsByEnergyCategory = new HashSet<MonitoringConfig>();
            MonitoringConfigsByWay = new HashSet<MonitoringConfig>();
            MonitoringConfigsByValidType = new HashSet<MonitoringConfig>();
            MonitoringConfigsByTargetType = new HashSet<MonitoringConfig>();
            SubscribesByReceivingMode = new HashSet<Subscribe>();
            PayMethod = new HashSet<HistoryBill>();
            BillType = new HashSet<HistoryBill>();
            PayType = new HashSet<HistoryBill>();
            BalanceType = new HashSet<UserAccount>();
            FeedbackType = new HashSet<Feedback>();
            FeedbackState = new HashSet<Feedback>();
            BalanceDetailsByCategory = new HashSet<BalanceDetail>();
            //TemplatesByCategory = new HashSet<Template>();

            //ActivityRecordType = new HashSet<ActivityRecord>();
            //ActivityRecordState = new HashSet<ActivityRecord>();
            //MaintenanceCategory = new HashSet<Maintenance>();
            //MaintenanceState = new HashSet<Maintenance>();
            //MaintenanceObjectCategory = new HashSet<Maintenance>();

            ActivityRecords = new HashSet<ActivityRecord>();
            MaintenanceMaintenanceCategories = new HashSet<Maintenance>();
            MaintenanceStates = new HashSet<Maintenance>();

            Groups = new HashSet<Group>();
            LogInfos = new HashSet<LogInfo>();
            RatedParameters = new HashSet<RatedParameter>();
            SceneModes = new HashSet<SceneMode>();
            BuildingMeterTypeUsers = new HashSet<BuildingMeterTypeUser>();

            UserExtensions = new HashSet<UserExtension>();
            ConfigDetailBuildingCategories = new HashSet<ConfigDetail>();

            ConfigDetailEngeryCategories = new HashSet<ConfigDetail>();
            Actions = new HashSet<MetersAction>();
            CommandStatusDic = new HashSet<MetersAction>();


            HitoryBillsByEnergyCategory = new HashSet<HistoryBill>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(128)]
        public string TreeId { get; set; }

        [Required]
        [StringLength(32)]
        public string Code { get; set; }

        [StringLength(12)]
        public string GbCode { get; set; }

        public int? FirstValue { get; set; }

        public int? SecondValue { get; set; }

        public int? ThirdValue { get; set; }

        public int? FourthValue { get; set; }
        public decimal? EquValue { get; set; }
        [StringLength(128)]
        public string EquText { get; set; }
        public int? FifthValue { get; set; }

        [Required]
        [StringLength(128)]
        public string ChineseName { get; set; }

        [StringLength(128)]
        public string EnglishName { get; set; }

        public bool Enable { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "机构类型")]
        public virtual ICollection<Organization> OrganizationByType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "建筑分类")]
        public virtual ICollection<Building> BuildingsByCategory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "建筑种类")]
        public virtual ICollection<Building> BuildingsByType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "坐标")]
        public virtual ICollection<Coordinate> Coordinates { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "品牌")]
        public virtual ICollection<Brand> Brands { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "能耗设备")]
        public virtual ICollection<Meter> MetersByEnergyCategory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "小时统计")]
        public virtual ICollection<MeterHourlyResult> MeterHourlyResults { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "天统计")]
        public virtual ICollection<MeterDailyResult> MeterDailyResults { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "月统计")]
        public virtual ICollection<MeterMonthlyResult> MeterMonthlyResults { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "设备分类")]
        public virtual ICollection<Meter> MetersByType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name="读参")]
        public virtual ICollection<Parameter> Parameters { get; set; }
   [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "附件")]
        public virtual ICollection<Attachment> AttachmentsByType { get; set; }
           [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "附件")]
        public virtual ICollection<Attachment> AttachmentsByFormat { get; set; }
           [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
         [Display(Name = "消息")]
        public virtual ICollection<Message> MessagesByType { get; set; }
           [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
         [Display(Name = "消息")]
        public virtual ICollection<Message> MessageBySourceType { get; set; }
           [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

         [Display(Name = "设备状态")]
        public virtual ICollection<MeterStatus> MeterStatusByMessageType { get; set; }
           [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
         [Display(Name = "配置")]
        public virtual ICollection<MonitoringConfig> MonitoringConfigsByEnergyCategory { get; set; }
           [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "配置")]
        public virtual ICollection<MonitoringConfig> MonitoringConfigsByCycleType { get; set; }
           [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "配置")]
        public virtual ICollection<MonitoringConfig> MonitoringConfigsByAlarmLevel { get; set; }
           [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "配置")]
        public virtual ICollection<MonitoringConfig> MonitoringConfigsByWay { get; set; }
           [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "配置")]
        public virtual ICollection<MonitoringConfig> MonitoringConfigsByConfigType { get; set; }
           [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        [Display(Name = "订阅")]
        public virtual ICollection<Subscribe> SubscribesByType { get; set; }
           [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "订阅")]
        public virtual ICollection<Subscribe> SubscribesByTargetType { get; set; }
           [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "订阅")]
        public virtual ICollection<Subscribe> SubscribesByMessageType { get; set; }
         [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
         [Display(Name = "发送方式")]
         public virtual ICollection<Subscribe> SubscribesByReceivingMode { get; set; }
         [Display(Name = "配置类型")]
         public virtual ICollection<MonitoringConfig> MonitoringConfigsByTargetType { get; set; }
         [Display(Name = "告警循环检测周期")]
         public virtual ICollection<MonitoringConfig> MonitoringConfigsByValidType { get; set; }
         [Display(Name = "账单类型")]
         public virtual ICollection<HistoryBill> BillType { get; set; }
         [Display(Name = "支付方式")]
         public virtual ICollection<HistoryBill> PayMethod { get; set; }
         [Display(Name = "支付类型")]
         public virtual ICollection<HistoryBill> PayType { get; set; }
         [Display(Name = "余额分类")]
         public virtual ICollection<UserAccount> BalanceType { get; set; }

        [Display(Name = "反馈类型")]
         [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
         public virtual ICollection<Feedback> FeedbackType { get; set; }
        [Display(Name="反馈状态")]
         [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
         public virtual ICollection<Feedback> FeedbackState { get; set; }


        //[Display(Name = "账户类型")]
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Balance> BalanceByCategory { get; set; }


        [Display(Name = "账户类型")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BalanceDetail> BalanceDetailsByCategory { get; set; }


        //Message.Templete外键引用Dictionary
        //[Display(Name = "模板")]
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Template> TemplatesByCategory { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ActivityRecord> ActivityRecordType { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<ActivityRecord> ActivityRecordState { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Maintenance> MaintenanceCategory { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Maintenance> MaintenanceState { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Maintenance> MaintenanceObjectCategory { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ActivityRecord> ActivityRecords { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Maintenance> MaintenanceMaintenanceCategories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Maintenance> MaintenanceStates { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Group> Groups { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogInfo> LogInfos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RatedParameter> RatedParameters { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SceneMode> SceneModes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuildingMeterTypeUser> BuildingMeterTypeUsers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserExtension> UserExtensions { get; set; }
        public virtual ICollection<ConfigDetail> ConfigDetailBuildingCategories { get; set; }

        public virtual ICollection<ConfigDetail> ConfigDetailEngeryCategories { get; set; }
        public virtual ICollection<MetersAction> Actions { get; set; }

        [Display(Name = "能耗类型")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoryBill> HitoryBillsByEnergyCategory { get; set; }

        [Display(Name = "能耗用户类型")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> UserType { get; set; }

        [Display(Name = "安装模式")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Meter> MeterSetupMode { get; set; }

        [Display(Name = "用途")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Building> BuildingPurpose { get; set; }

        [Display(Name = "命令状态")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MetersAction> CommandStatusDic { get; set; }
    }
}
