using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class HistoryBillStatisticsData
    {
        public DateTime Time;//统计开始时间
        public decimal? UseEnergyValue;//能耗量
        public decimal Value;//消费
    }

    public class HistoryBillStatisticsReturnData
    {
        public List<HistoryBillStatisticsData> HistoryBillStatisticList { get; set; }//统计数据集合
        public int HistoryBillNum { get; set; }//统计数据总数量
        public bool IsNextPage { get; set; }//是否有下一页
    }
}
