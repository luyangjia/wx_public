using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO.Common
{
    public enum TimeUnits
    {
        [Display(Name = "小时")]
        Hourly = 10,
        [Display(Name = "最近24小时")]
        H24 = 11,
        [Display(Name = "最近48小时")]
        H48 = 12,
        [Display(Name = "最近72小时")]
        H72 = 13,
        [Display(Name = "日")]
        Daily = 20,
        [Display(Name = "日间")]
        Daytime = 21,
        [Display(Name = "夜间")]
        Nighttime = 22,
        [Display(Name = "半夜")]
        Midnight = 23,
        [Display(Name = "月")]
        Monthly = 30,
        [Display(Name = "季度")]
        Quarterly = 31,
        [Display(Name = "年")]
        Yearly = 40
    }
}
