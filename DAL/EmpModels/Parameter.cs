namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.Parameter")]
    public partial class Parameter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Parameter()
        {
            MomentaryValues = new HashSet<MomentaryValue>();
            MonitoringConfigs = new HashSet<MonitoringConfig>();
            RatedParameters = new HashSet<RatedParameter>();

        }

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int BrandId { get; set; }

        public int TypeId { get; set; }

        [StringLength(16)]
        public string Unit { get; set; }

        public virtual Dictionary Type { get; set; }

        public virtual Brand Brand { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "实时")]
        public virtual ICollection<MomentaryValue> MomentaryValues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "小时统计")]
        public virtual ICollection<MeterHourlyResult> MeterHourlyResult { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name = "天统计")]
        public virtual ICollection<MeterDailyResult> MeterDailyResult { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        [Display(Name="月统计")]
        public virtual ICollection<MeterMonthlyResult> MeterMonthlyResult { get; set; }

        [Display(Name = "订阅")]
        public virtual ICollection<MonitoringConfig> MonitoringConfigs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RatedParameter> RatedParameters { get; set; }
    }
}
