using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    /// <summary>
    /// 水电控设备充值传入数据
    /// </summary>
    /// <param name="MacAddress">mac地址</param>
    /// <param name="Name">充值人姓名</param>
    /// <param name="Value">充值金额</param>
    /// <param name="Time">充值时间</param>
    /// <param name="TypeId">充值能源类型</param>
    public class MacRechargeData
    {
        public string MacAddress { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime? Time { get; set; }
        public int? TypeId { get; set; }
        public string UserKey { get; set; }
    }	
}
