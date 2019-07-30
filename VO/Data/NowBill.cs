using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class NowBill{

        /// <summary>
        /// 各个能耗类型账单数据
        /// </summary>
        public Dictionary<int, EnergyBill> EnergyBills { get; set; }//各个能耗类型账单数据

        /// <summary>
        /// 现金账户
        /// </summary>
        public decimal CashAccount { get; set; }//现金账户

        /// <summary>
        /// 补贴账户
        /// </summary>
        public decimal SubsidyAccount { get; set; }//补贴账户

        /// <summary>
        /// 子账户补贴和充值总金额
        /// </summary>
        public decimal ChildAccount { get; set; }//子账户补贴和充值总金额

        /// <summary>
        /// 每日结算截止时间
        /// </summary>
        public DateTime? DeadlineTime { get; set; }//每日结算截止时间

        /// <summary>
        /// 截止时间应缴费用
        /// </summary>
        public decimal NowCost { get; set; }//截止时间应缴费用

        public decimal PreCost { get; set; }//上周期应缴费用

        public string IsPowerOff { get; set; }//是否停水、断电

        public string Name { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TargetType { get; set; }
       
    }
    public class EnergyBill
    {
        /// <summary>
        /// 总能耗
        /// </summary>
        public decimal? TotalValue { get; set; }//总能耗

        /// <summary>
        /// 总消费
        /// </summary>
        public decimal? TotalCost { get; set; }//总消费

        /// <summary>
        /// 本周期能耗
        /// </summary>
        public decimal? ThisPeriodValue { get; set; }//本周期能耗

        /// <summary>
        /// 本周期消费
        /// </summary>
        public decimal? ThisPeriodCost { get; set; }//本周期消费

        /// <summary>
        /// 上周期开始时间
        /// </summary>
        public DateTime? PrePeriodBeginTime { get; set; }//本周期开始时间
        /// <summary>
        /// 上周期消费
        /// </summary>
        public decimal? PrePeriodCost { get; set; }//本周期消费
        /// <summary>
        /// 上周期消费
        /// </summary>
        public decimal? PrePeriodValue { get; set; }//本周期消费

        /// <summary>
        /// 本周期开始时间
        /// </summary>
        public DateTime? ThisPeriodBeginTime { get; set; }//本周期开始时间
        /// <summary>
        /// 本周期截至时间
        /// </summary>
        public DateTime? FinishTime { get; set; }//统计截止时间

        /// <summary>
        /// 本类补贴余额，一期只进行全类型充值，暂不使用
        /// </summary>
        public decimal? Subsidy { get; set; }//本类补贴余额

        /// <summary>
        /// 如果是收款账号，总共收到多少钱此能耗类型，一期只进行全类型充值，暂不使用
        /// </summary>
        public decimal ChildTotalPay { get; set; }//如果是收款账号，总共收到多少钱此能耗类型

    }
}
