using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
   public  class GuaranteePowerData
    {
       public bool isActiveInWeekEnd { get; set; }
       public List<ConfigCycleSettingData> ConfigCycleSetting { get; set; }

       public bool isEnable { get; set; }

    }
}
