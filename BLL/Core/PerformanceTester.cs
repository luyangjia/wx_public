using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL.Core
{
    public static class PerformanceTester
    {
        public static DateTime Start { get; set; }
        public static DateTime Finish { get; set; }
        public static TimeSpan Duration { get { return PerformanceTester.Finish - PerformanceTester.Start; } }
    }
}
