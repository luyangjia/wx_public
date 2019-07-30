using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.VO;
namespace WxPay2017.API.VO.Common
{
    public static class TimeTools
    {
        /// <summary>
        /// 在当前时间添加一个周期，获得新的下个周期时间
        /// </summary>
        /// <param name="time"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static DateTime addCycleTime(DateTime time, MonitoringConfigData config)
        {
            if (config.CycleTypeId == DictionaryCache.PeriodYear.Id)
                return time.AddYears(1);
            else if (config.CycleTypeId == DictionaryCache.PeriodMonth.Id)
                return time.AddMonths(1);
            else if (config.CycleTypeId == DictionaryCache.PeriodWeek.Id)
                return time.AddDays(7);
            else if (config.CycleTypeId == DictionaryCache.PeriodDay.Id)
                return time.AddDays(1);
            else if (config.CycleTypeId == DictionaryCache.PeriodHour.Id)
                return time.AddHours(1);
            else
                return time;
        }
    }
}
