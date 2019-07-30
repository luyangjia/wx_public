namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.MeterMonthlyResult")]
    public partial class MeterMonthlyResult : IMeterResult
    {
        public Guid Id { get; set; }

        public int MeterId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }

        public decimal Total { get; set; }

        [Required]
        [StringLength(16)]
        public string Unit { get; set; }

        public int ParameterId { get; set; }

        public int Status { get; set; }

        public virtual Dictionary StatusDict { get; set; }

        public virtual Meter Meter { get; set; }

        public virtual Parameter Parameter { get; set; }
    }
}
