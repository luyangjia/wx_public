namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bill.BalanceDetail")]
    public partial class BalanceDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BalanceDetail()
        {
            //Meters = new HashSet<Meter>();
            //Parameters = new HashSet<Parameter>();
        }
         
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        public long BalanceId { get; set; }

        public int EnergyCategory { get; set; }
        /// <summary>
        /// 能耗消费
        /// </summary>
        public decimal Consumption { get; set; }

        public decimal Price { get; set; }
        /// <summary>
        /// 能耗总量
        /// </summary>
        public decimal Amount { get; set; }

        public decimal? ConsumptionTotal { get; set; }

        public decimal? AmountTotal { get; set; }

        public virtual Dictionary EnergyCategoryDict { get; set; }

        public virtual Balance Balance { get; set; }

    }
}
