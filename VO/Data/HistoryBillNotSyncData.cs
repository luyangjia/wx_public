using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    /// <summary>
    /// 历史订单未同步数据
    /// </summary>
    /// <param name="macAddress">历史充值订单对应水电控设备mac地址</param>
    /// <param name="name">付款人姓名</param>
    /// <param name="value">充值金额</param>
    /// <param name="time">充值时间</param>
    /// <param name="typeId">充值能源类型</param>
    public class HistoryBillNotSyncData
    {
        public long Id { get; set; }
        public string MacAddress { get; set; }
        public int MeterId { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime? Time { get; set; }
        public int? TypeId { get; set; }
    }
}
