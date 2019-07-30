namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.VirtualMeters")]
    public partial class VirtualMeters
    {
        public int Id { get; set; }
        public int TargetMeterId { get; set; }
        public string SourceMeterId { get; set; }
        public string AddMetersId { get; set; }
        public Nullable<decimal> AddPer { get; set; }
        public string SubMetersId { get; set; }
        public Nullable<decimal> SubPer { get; set; }
        public virtual Meter Meter { get; set; }
    }
}
