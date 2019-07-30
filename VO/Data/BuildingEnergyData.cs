using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class BuildingEnergyData
    {
        public MeterData Meter { get; set; }
        public decimal BeginTimeData { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime Endtime { get; set; }
        public decimal EndTimeData { get; set; }
        public string Unit { get; set; }
        public decimal Value { get; set; }
       
    }
    public class BuildingEnergyDataList
    {
        public BuildingData Building { get; set; }
        public List<BuildingEnergyData> BuildingEnergyDatas { get; set; }
    }
}
