namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.Meter")]
    public partial class Meter 
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Meter()
        {
            Children = new HashSet<Meter>();
            MeterDailyResults = new HashSet<MeterDailyResult>();
            MeterHourlyResults = new HashSet<MeterHourlyResult>();
            MeterMonthlyResults = new HashSet<MeterMonthlyResult>();
            MomentaryValues = new HashSet<MomentaryValue>();
            OriginalDatas = new HashSet<OriginalData>();
            MeterStatus = new HashSet<MeterStatus>();

            LogInfos = new HashSet<LogInfo>();
            SceneModeMeters = new HashSet<SceneModeMeter>();
            MeterGroups = new HashSet<MeterGroup>();
            RatedParameterDetails = new HashSet<RatedParameterDetail>();

            ConfigDetails = new HashSet<ConfigDetail>();
            MetersActions = new HashSet<MetersAction>();
        }

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [StringLength(128)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string TreeId { get; set; }

        public int? Rank { get; set; }

        public int? BuildingId { get; set; }
        public bool? IsControlByHand { get; set; }
        public int EnergyCategoryId { get; set; }

        public int BrandId { get; set; }

        public int? CoordinateMapId { get; set; }
        public decimal? EffectiveBasePrice { get; set; }
        public int? EffectiveModel { get; set; }
        public int? RelayElecState { get; set; }
        public bool? PaulElecState { get; set; }
        public int? Coordinate3dId { get; set; }

        public int? Coordinate2dId { get; set; }

        [StringLength(16)]
        public string Code { get; set; }


        [StringLength(16)]
        public string GbCode { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [StringLength(128)]
        public string AliasName { get; set; }

        [StringLength(16)]
        public string Initial { get; set; }

        public int Type { get; set; }

        [StringLength(128)]
        public string Access { get; set; }

        public int? Sequence { get; set; }

        [StringLength(256)]
        public string Address { get; set; }

        [StringLength(256)]
        public string MacAddress { get; set; }

        public bool Enable { get; set; }

        public string StatusNote { get; set; }

        public int? GetInterval { get; set; }

        [StringLength(10)]
        public string BranchName { get; set; }
        public DateTime? FinalSettlementTime { get; set; }
        public bool? BranchEnable { get; set; }

        public decimal Rate { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public bool IsTurnOn { get; set; }

        public int? SetupMode { get; set; }

        public int? PortNumber { get; set; }

        public string Rs485Address { get; set; }

        public decimal? ActivePrecise { get; set; }

        public decimal? ReactivePrecise { get; set; }

        [StringLength(50)]
        public string BasicCurrent { get; set; }

        [StringLength(50)]
        public string ComProtocol { get; set; }

        public decimal? SpeedRate { get; set; }

        public bool? IsHarmonic { get; set; }

        public int? Precision { get; set; }

        public decimal? PtRate { get; set; }

        public decimal? RangeRatio { get; set; }

        public int? Caliber { get; set; }

        public decimal? Hydraulic { get; set; }

        public decimal? Flow { get; set; }

        //[StringLength(256)]
        //public string SetupPosition { get; set; }

        [StringLength(256)]
        public string SupplyRegion { get; set; }

        [StringLength(256)]
        public string Manufactor { get; set; }

        [Column(TypeName = "date")]
        public DateTime? SetupDate { get; set; }

        public decimal? InitialValue { get; set; }

        [StringLength(256)]
        public string PortDescription { get; set; }



        public virtual Dictionary DictionarySetupMode { get; set; }

        public virtual Building Building { get; set; }

        public virtual Coordinate Coordinate2d { get; set; }

        public virtual Coordinate Coordinate3d { get; set; }

        public virtual Coordinate CoordinateMap { get; set; }

        public virtual Dictionary EnergyCategoryDict { get; set; }

        public virtual Dictionary TypeDict { get; set; }

        public virtual Brand Brand { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "子对象")]
        public virtual ICollection<Meter> Children { get; set; }

        public virtual Meter Parent { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "天统计")]
        public virtual ICollection<MeterDailyResult> MeterDailyResults { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "小时统计")]
        public virtual ICollection<MeterHourlyResult> MeterHourlyResults { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "月统计")]
        public virtual ICollection<MeterMonthlyResult> MeterMonthlyResults { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "实时")]
        public virtual ICollection<MomentaryValue> MomentaryValues { get; set; }

        [Display(Name = "原始")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OriginalData> OriginalDatas { get; set; }
        [Display(Name = "状态")]
        public virtual ICollection<MeterStatus> MeterStatus { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MeterGroup> MeterGroups { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RatedParameterDetail> RatedParameterDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LogInfo> LogInfos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SceneModeMeter> SceneModeMeters { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ConfigDetail> ConfigDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MetersAction> MetersActions { get; set; }

    }
}
