namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bill.EnergyStatistics")]
    public partial class EnergyStatistics
    {
        public long Id { get; set; }
        public Nullable<int> BuildingId { get; set; }
        public Nullable<int> OrganizationId { get; set; }
        public Nullable<int> DistributionId { get; set; }
        public Nullable<int> PumpRoomId { get; set; }
        public int EnergyCategoryId { get; set; }
        public decimal Value { get; set; }
        public decimal Total { get; set; }
        public System.DateTime AuditDate { get; set; }

        public virtual Building Building { get; set; }
        public virtual Distribution Distribution { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual PumpRoom PumpRoom { get; set; }
        public virtual Dictionary EnergyCategoryDic { get; set; }
    }
}
