using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class MonitoringConfigPriceData
    {
        public ConfigDetailData Detail { get; set; }
        public List<MonitoringConfigGroupPriceData> PriceConfigs { get; set; }
    }

    public class MonitoringConfigGroupPriceData
    {
        public int StartYear { get; set; }
        public int StartMonth { get; set; }
        public int EndMonth { get; set; }
        public int EndYear { get; set; }
        public int StartDay { get; set; }
        public int EndDay { get; set; }
        public int? ValidStartHour { get; set; }
        public int? ValidEndHour { get; set; }
        public List<MonitoringConfigData> Configs{get;set;}
        
    }
}
