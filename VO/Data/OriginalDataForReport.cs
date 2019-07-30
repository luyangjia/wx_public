using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class OriginalDataForReport
    {
        public int ParameterId { get; set; }

        public string ParameterName { get; set; }

        public decimal? Value { get; set; }

        public DateTime Time { get; set; }
        public string Unit { get; set; }
    }
}
