
namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.MeterGroup")]
    public partial class MeterGroup
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public int? MeterId { get; set; }

        public int? BuildingId { get; set; }

        public bool Enable { get; set; }

        public virtual Building Building { get; set; }

        public virtual Group Group { get; set; }

        public virtual Meter Meter { get; set; }
    }
}
