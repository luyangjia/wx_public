using WxPay2017.API.VO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class MonitorMeta
    {
        public MonitorMeta()
        {
            Params = new Dictionary<string, decimal>();
        }
        //监测对象
        public int TargetId { get; set; }

        //监测对象类型： 建筑，设备
        public CategoryDictionary TargetType { get; set; }

        //实时类型
        public CategoryDictionary Category { get; set; }

        public Dictionary<string, decimal> Params { get; set; }

        public int State { get; set; }

        public bool Infinite { get; set; }
    }
}
