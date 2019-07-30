using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class MeterPayInfo
    {
        public MeterData Meter { get; set; }
        public OriginalDataForReport Report { get; set; }
        public decimal? UnitPrice { get; set; }
        public int MalignantLoadTimes { get; set; }
        public int OverLoadTimes { get; set; }

    }
}
