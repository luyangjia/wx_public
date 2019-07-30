using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    /// <summary>
    /// 建筑价格配置参数
    /// </summary>
    public class ConfigSettingData
    {
        public int Id { get; set; }
        public int? EnergyCategoryId{ get; set; }
        public int? HolidayTimeControlTemplateId { get; set; }
        public bool HolidayTimeControlEnable { get; set; }
         public int? VacationTimeControlTemplateId { get; set; }
         public bool VacationTimeControlEnable { get; set; }
         public int? WeekEndTimeControlTemplateId { get; set; }
         public bool WeekEndTimeControlEnable { get; set; }
         public int? PeacetimeTimeControlTemplateId { get; set; }
         public bool PeacetimeTimeControlEnable { get; set; }
         public decimal? MinThresholdForMalignantLoad { get; set; }
         public decimal? MinThresholdForOverLoad { get; set; }
         public bool? IsOpenMalignantLoadAlert { get; set; }
         public bool? IsOpenOverLoadAlert { get; set; }
         public bool? IsControlByAccount { get; set; }
         public bool? IsControlByTime { get; set; }
         public bool? IsControlPower { get; set; }
        public bool? IsControlWater { get; set; }
        public bool? IsControlWaterByPower { get; set; }
        public int? TemplateId { get; set; }

    }
}
