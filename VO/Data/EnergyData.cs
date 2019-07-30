using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class EnergyData
    {
        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }

        public decimal? Value { get; set; }
    }
}
