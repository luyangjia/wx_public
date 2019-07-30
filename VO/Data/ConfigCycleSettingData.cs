using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public partial class ConfigCycleSettingData
    {
        public int Id { get; set; }

        public int ConfigId { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
        public string Description { get; set; }
    }
}
