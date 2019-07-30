namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.PumpRoom")]
    public partial class PumpRoom
    {
        public PumpRoom()
        {
            Meters = new HashSet<Meter>();
            Children = new HashSet<PumpRoom>();

            this.EnergyStatistics = new HashSet<EnergyStatistics>();
        }

        public int Id { get; set; }

        public int? ParentId { get; set; }


        [Required]
        [StringLength(128)]
        public string PumpName { get; set; }

        [Required]
        [StringLength(128)]
        public string PumpAddress { get; set; }

        public int? Coordinate3dId { get; set; }

        public virtual Coordinate Coordinate { get; set; }

        public virtual ICollection<Meter> Meters { get; set; }

        public virtual ICollection<PumpRoom> Children { get; set; }

        public virtual PumpRoom Parent { get; set; }
        public virtual ICollection<EnergyStatistics> EnergyStatistics { get; set; }
    }
}
