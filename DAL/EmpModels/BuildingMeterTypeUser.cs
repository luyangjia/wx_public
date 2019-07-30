
namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.BuildingMeterTypeUser")]
    public partial class BuildingMeterTypeUser
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        public int MeterTypeId { get; set; }

        public int BuildingId { get; set; }

        public bool? Enable { get; set; }

        public virtual Building Building { get; set; }

        public virtual Dictionary MeterType { get; set; }

        public virtual User User { get; set; }
    }
}
