namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.Gateway")]
    public partial class Gateway
    {
        public Gateway()
        {
            Meters = new HashSet<Meter>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(4)]
        public string GatewayNo { get; set; }

        [Required]
        [StringLength(128)]
        public string IPAddress { get; set; }

        [StringLength(256)]
        public string EnablePort { get; set; }

        [StringLength(256)]
        public string PortRule { get; set; }

        [StringLength(256)]
        public string SpeedRate { get; set; }

        [Required]
        [StringLength(128)]
        public string Address { get; set; }

        [Required]
        [StringLength(128)]
        public string NetworkingArea { get; set; }

        //public int BrandId { get; set; }

        public DateTime Date { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public virtual ICollection<Meter> Meters { get; set; }
    }
}
