namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.Distribution")]
    public partial class Distribution
    {
        public Distribution()
        {
            Meters = new HashSet<Meter>();
            this.EnergyStatistics = new HashSet<EnergyStatistics>();
            Children = new HashSet<Distribution>();
            IsSwitchingStation = false;

        }

        public int Id { get; set; }

        public int? ParentId { get; set; }


        [Required]
        [StringLength(128)]
        public string SubstationName { get; set; }

        [Required]
        [StringLength(128)]
        public string TransformerName { get; set; }

        
        [StringLength(128)]
        public string TransformerType { get; set; }

        public int TransformerCapacity { get; set; }
        public bool? IsSwitchingStation { get; set; } 

        [Required]
        [StringLength(128)]
        public string DistributionAddress { get; set; }

        public int? Coordinate3dId { get; set; }

        public virtual Coordinate Coordinate { get; set; }

        public virtual ICollection<Meter> Meters { get; set; }

        public virtual ICollection<Distribution> Children { get; set; }

        public virtual Distribution Parent { get; set; }
        public virtual ICollection<EnergyStatistics> EnergyStatistics { get; set; }
    }
}
