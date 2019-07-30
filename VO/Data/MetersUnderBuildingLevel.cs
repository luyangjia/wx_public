using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class MetersBaseInfo
    {
        public int EnergyCategoryId { get; set; }
        public int Count { get; set; }
        public int BuildingCount { get; set; }
        public int AbnormalCostMeterCount { get; set; }
        public int AbnormalEquipmentMeterCount { get; set; }
        public int RechargeFailureMeterCount { get; set; }
        public int BatteryShortageMeterCount { get; set; }
        public int SettingFailureMeterCount { get; set; }
    }

     public class MetersRealTimeInfo
     {
         public RealTimeMeter Meter { get; set; }
         public List<MeterStatusData> MeterStatus { get; set; }
         public bool IsAbnormalEquipment = false;
         public bool IsAbnormalCost = false;
     }
    public class MetersUnderBuildingLevel
    {
        public int BuildingId { get; set; }
        public bool isEmergencySwitchOn { get; set; }
        public int RoomNum{get;set;}
        public int stuNum{get;set;}
        public List<MetersRealTimeInfo> PowerMeters = new List<MetersRealTimeInfo>();
        public List<MetersRealTimeInfo> WaterMeters = new List<MetersRealTimeInfo>();
    }


    public class RealTimeMeter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? BuildingId { get; set; }
        public string BuildingName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public bool IsSwitchOn { get; set; }
        public int? SetupMode { get; set; }
        public string SetupModelName { get; set; }
        public bool IsTurnOn { get; set; }
        public bool IsControlable { get; set; }
        public string Status { get; set; }
        public decimal? RealValue { get; set; }
        public string RealValueUnit { get; set; }
        public decimal? ChargeBalance { get; set; }
        public int OverLoadTimes { get; set; }
        public int MalignantLoadTimes { get; set; }
        public string ControlModelName { get; set; }
       

    }

    public class PowerMeterMonitoringData
    {
        public string BuildingName { get; set; }
        public string BrandName { get; set; }
        public decimal? RealValue { get; set; }
        public decimal? LeftValue { get; set; }
        public string ControlModelName { get; set; }
        public string Status { get; set; }
        public int OverLoadTimes { get; set; }
        public int MalignantLoadTimes { get; set; }
        public bool IsRemoteControl { get; set; }



    }
}
