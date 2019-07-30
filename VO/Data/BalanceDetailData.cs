namespace WxPay2017.API.VO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BalanceDetailData
    {
        public long Id { get; set; }

        public long BalanceId { get; set; }

        /// <summary>
        /// 能耗类型
        /// </summary>
        public int EnergyCategory { get; set; }

        /// <summary>
        /// 能耗使用量
        /// </summary>
        public decimal Consumption { get; set; }

        /// <summary>
        /// 能耗单价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 能耗费用
        /// </summary>
        public decimal Amount { get; set; }

        public decimal? ConsumptionTotal { get; set; }

        public decimal? AmountTotal { get; set; }

        /// <summary>
        /// 能耗类型字典
        /// </summary>
        public virtual DictionaryData EnergyCategoryDict { get; set; }
    }
}
