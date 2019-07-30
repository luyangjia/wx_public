namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bill.HistoryBill")]
    public partial class HistoryBill
    {
        public long Id { get; set; }

        [Required]
        [StringLength(128)]
        public string ReceiverId { get; set; }

        [StringLength(128)]
        public string PayerId { get; set; }
        [StringLength(128)]
        public string OperatorId { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? EndTime { get; set; }

        public decimal Value { get; set; }
        public decimal? UsedValue { get; set; }
        public decimal? UsedEnergyValue { get; set; }
        public int BillTypeId { get; set; }

        public int? PayTypeId { get; set; }

        public bool IsPay { get; set; }

        [StringLength(200)]
        public string subject { get; set; }

        [StringLength(1000)]
        public string Body { get; set; }

        public DateTime? PayMentTime { get; set; }

        public DateTime? CreateTime { get; set; }

        public bool? IsZero { get; set; }
        public bool? IsSynchro { get; set; }
        public DateTime? ZeroTime { get; set; }

        public int? PayMethodId { get; set; }

        [StringLength(100)]
        public string TransNumber { get; set; }

        public int? EnergyCategoryId { get; set; }

        public virtual Dictionary PayMethod { get; set; }

        public virtual Dictionary BillType { get; set; }

        public virtual Dictionary PayType { get; set; }

        public virtual User Payer { get; set; }

        public virtual User Receiver { get; set; }
        public virtual User Operator { get; set; }

        [Display(Name = "能耗类型")]
        public virtual Dictionary EnergyCategory { get; set; }
    }
}
