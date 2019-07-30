using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class StatisticsBaseInfo
    {
       /// <summary>
       /// 电能耗
       /// </summary>
        public decimal EnergyUsedValue { get; set; }
        /// <summary>
        /// 电产生费用
        /// </summary>
        public decimal EnergyCostValue { get; set; }
        /// <summary>
        /// 水能耗
        /// </summary>
        public decimal WaterUsedValue { get; set; }
        /// <summary>
        /// 水产生费用
        /// </summary>
        public decimal WaterCostValue { get; set; }

        /// <summary>
        /// 用户在线支付总额
        /// </summary>
        public decimal UserTotalPayMentOnLine { get; set; }
        // <summary>
        /// 用户线下现金支付总额
        /// </summary>
        public decimal UserTotalPayMentByCash { get; set; }
        ///// <summary>
        ///// 用户补贴发放总额
        ///// </summary>
        //public decimal UserTotalPayMentByCash { get; set; }
    }
}
