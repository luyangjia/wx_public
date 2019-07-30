using System.Collections.Generic;
namespace WxPay2017.API.VO.Data
{
    public partial class DistributionData
    {

        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string SubstationName { get; set; }

        public string TransformerName { get; set; }

        public string TransformerType { get; set; }

        public int TransformerCapacity { get; set; }

        public string DistributionAddress { get; set; }

        public int? Coordinate3dId { get; set; }
        public bool? IsSwitchingStation { get; set; } 
        public virtual CoordinateData Coordinate3d { get; set; }

        public virtual ICollection<MeterData> MeterDatas { get; set; }

    }

    public partial class DistributionAndBuildingData
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Name { get; set; }
        public int Category { get; set; }
    }

    public partial class SubstationMonitoring
    {
        public int DistributionCount { get; set; }
        public int MeterCount { get; set; }
        public decimal MonthSupply { get; set; }
        //供能量
        public decimal LastMonthSupply { get; set; }
        //用能量
        public decimal MonthSpend { get; set; }
        public decimal LastMonthSpend { get; set; }
        public decimal APower { get; set; }
        public decimal BPower { get; set; }
        public decimal CPower { get; set; }
        public decimal TotalPower { get; set; }

    }

}
