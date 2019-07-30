using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    /// <summary>
    /// 缴费统计数据[缴费人Id、缴费人姓名、缴费总额]
    /// </summary>
    public class HistoryBillPayData
    {
        public string PayerId { get; set; }//缴费人Id
        public string PayerName { get; set; }//缴费人姓名
        public decimal? PayValue { get; set; }//缴费总额
    }	
}
