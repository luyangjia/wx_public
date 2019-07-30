namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bill.Balance")]
    public partial class Balance
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Balance()
        {
            //Meters = new HashSet<Meter>();
            //Parameters = new HashSet<Parameter>();
            BalanceDetails = new HashSet<BalanceDetail>();
        }

        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 对象Id
        /// </summary>
        public int TargetId { get; set; }

        /// <summary>
        /// 对象类型
        /// </summary>
        public int TargetCategory { get; set; }

        /// <summary>
        /// 能耗类型
        /// </summary>
        public int EnergyCategory { get; set; }

        /// <summary>
        /// 能耗单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 能耗消耗量
        /// </summary>
        public decimal EnergyConsumption { get; set; }

        /// <summary>
        /// 上月余额
        /// </summary>
        public decimal Overplus { get; set; }

        /// <summary>
        /// 预交费
        /// </summary>
        public decimal Prepay { get; set; }

        /// <summary>
        /// 补贴
        /// </summary>
        public decimal Subsidy { get; set; }

        /// <summary>
        /// 在线\非现金充值
        /// </summary>
        public decimal Recharge { get; set; }

        /// <summary>
        /// 现金充值
        /// </summary>
        public decimal CashCharge { get; set; }

        /// <summary>
        /// 现金纠错
        /// </summary>
        public decimal CashCorrect { get; set; }

        /// <summary>
        /// 本期使用
        /// </summary>
        public decimal Usage { get; set; }

        /// <summary>
        /// 本期退还
        /// </summary>
        public decimal Refund { get; set; }

        /// <summary>
        /// 本期坏账
        /// </summary>
        public decimal BadDebt { get; set; }

        /// <summary>
        /// 本期剩余
        /// </summary>
        public decimal Total { get; set; }
        public decimal? TotalSubsidy { get; set; }
        public decimal? TotalRecharge { get; set; }
        public decimal? TotalCashCharge { get; set; }

        /// <summary>
        /// 本期结算日期
        /// </summary>
        public DateTime AuditDate { get; set; }

        /// <summary>
        /// 本单据创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 操作者id
        /// </summary>
        [Required]
        [StringLength(128)]
        public string OperatorId { get; set; }

        //public virtual Dictionary BalanceCategory { get; set; }

        public virtual ICollection<BalanceDetail> BalanceDetails { get; set; }

    }
}
