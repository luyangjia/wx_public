using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class MeterDailySumResult
    {
        public decimal Sum { get; set; }
        public string UserId { get; set; }
        public int MeterId { get; set; }
    }
}
