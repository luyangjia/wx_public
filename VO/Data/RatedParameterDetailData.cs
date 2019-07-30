using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public partial class RatedParameterDetailData
    {
        public int Id { get; set; }

        public int? RatedParameterId { get; set; }

        public int? BrandId { get; set; }

        public int? MeterId { get; set; }

        public DateTime SettingTime { get; set; }

        public bool? IsSuccess { get; set; }

        public string OperatorId { get; set; }

        public string OperatorName { get; set; }
        public BrandData Brand { get; set; }
        public MeterData Meter { get; set; }
        public UserData Operator { get; set; }
    }

    /// <summary>
    /// 下发参数设备详情数据(包含建筑信息)
    /// </summary>
    public class RatedParameterDetailMeterBuildingData
    {
        public int RatedParameterDetailId { get; set; }
        public string MeterName{get;set;}
        public int MeterId{get;set;}
        public int? MeterParentId{get;set;}
        public bool? IsSuccess{get;set;}
        public DateTime SettingTime{get;set;}
        public int? BuildingId{get;set;}
        public string BuildingName{get;set;}
    }

    /// <summary>
    /// 下发参数设备详情数据(不包含建筑信息)
    /// </summary>
    public class RatedParameterDetailMeterData
    {
        public int RatedParameterDetailId { get; set; }
        public string MeterName { get; set; }
        public int MeterId { get; set; }
        public int? MeterParentId { get; set; }
        public bool? IsSuccess { get; set; }
        public DateTime SettingTime { get; set; }

    }
}
