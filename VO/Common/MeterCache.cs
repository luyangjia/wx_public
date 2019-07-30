using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO;

namespace WxPay2017.API.VO.Common
{
    public static class MeterCache
    {
        public static void Init()
        {
            MetersUnderBuiding = new Dictionary<string, StatisticalTransfer>();
            MetersUnderOrg = new Dictionary<string, StatisticalTransfer>();
            MetersByEnergy = new Dictionary<string, IList<StatisticalTransfer>>();
 
        }
        // 通过建筑获取关联的一级设备清单
        //key为建筑id+_+能耗id+_+way
        public static Dictionary<string, StatisticalTransfer> MetersUnderBuiding = new Dictionary<string, StatisticalTransfer>();

        // 通过机构获取关联的一级设备清单
        //key为机构id+_+能耗id+_+way
        public static Dictionary<string, StatisticalTransfer> MetersUnderOrg = new Dictionary<string, StatisticalTransfer>();


        //通过建筑获取能耗类型关联的一级设备清单
        //key为机构id+_+能耗ids+_+way
        public static Dictionary<string, IList<StatisticalTransfer>> MetersByEnergy = new Dictionary<string, IList<StatisticalTransfer>>();
    }
}
