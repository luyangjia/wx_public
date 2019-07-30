namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.MomentaryValue")]
    public partial class MomentaryValue
    {
        public int Id { get; set; }

        public int MeterId { get; set; }

        public int ParameterId { get; set; }

        public decimal Value { get; set; }

        public DateTime Time { get; set; }

        public virtual Meter Meter { get; set; }

        public virtual Parameter Parameter { get; set; }
    }
}
