namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.CacheStatisticsData")]
    public partial class CacheStatisticsData
    {
        public long Id { get; set; }

        public int TargetId { get; set; }

        [Required]
        [StringLength(50)]
        public string StatMode { get; set; }

        public int EnergyCategoryId { get; set; }

        public int ParameterTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string TimeUnit { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }

        public decimal Value { get; set; }

        public virtual Dictionary DictionaryEnergy { get; set; }

        public virtual Dictionary DictionaryParameter { get; set; }
    }
}
