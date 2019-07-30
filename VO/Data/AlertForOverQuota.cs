using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.VO.Common;
namespace WxPay2017.API.VO
{
    public class AlertForOverQuota
    {
        public string MessageInfo { get; set; }
        public decimal OriginalValue { get; set; }
        public decimal NowValue { get; set; }
        public StatisticalModes Mode { get; set; }
        public int TargetId { get; set; }
        public MonitoringConfigData Config { get; set; }
    }
}
