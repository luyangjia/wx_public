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
        /// �ܺ�����
        /// </summary>
        public int EnergyCategory { get; set; }

        /// <summary>
        /// �ܺ�ʹ����
        /// </summary>
        public decimal Consumption { get; set; }

        /// <summary>
        /// �ܺĵ���
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// �ܺķ���
        /// </summary>
        public decimal Amount { get; set; }

        public decimal? ConsumptionTotal { get; set; }

        public decimal? AmountTotal { get; set; }

        /// <summary>
        /// �ܺ������ֵ�
        /// </summary>
        public virtual DictionaryData EnergyCategoryDict { get; set; }
    }
}
