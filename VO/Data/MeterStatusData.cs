using System.Collections.Generic;
using System;
using WxPay2017.API.VO.Param;
namespace WxPay2017.API.VO
{
    public  class MeterStatusData
    {
        public int Id { get; set; }

        public int MeterId { get; set; }

        public int MeterMessageTypeId { get; set; }

        public bool Enabled { get; set; }

        public decimal Value { get; set; }

        public DateTime UpdateTime { get; set; }

        public bool? IsFluctuationData { get; set; }

        public int? MonitoringConfigId { get; set; }
        public string Description { get; set; }
       

        public  DictionaryData MeterMessageType { get; set; }

        //public MonitoringConfigData MonitoringConfig { get; set; }

        public  MeterData Meter { get; set; }
    }

    /// <summary>
    /// 故障设备信息
    /// </summary>
    public class MalfunctionMeterData {
        /// <summary>
        /// 设备信息
        /// </summary>
        public Object Meter { get; set; }
        //public MeterShortData Meter { get; set; }
        /// <summary>
        /// 设备名
        /// </summary>
        public string MeterName { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 设备mac地址
        /// </summary>
        public string MacAddress { get; set; }

        /// <summary>
        /// 故障信息名(字典)
        /// </summary>
        public string MalfunctionName { get; set; }
        /// <summary>
        /// 故障更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 故障设备地址
        /// </summary>
        public string MeterAddress { get; set; }
        /// <summary>
        /// 所在建筑Id
        /// </summary>
        public int? BuidingId { get; set; }
        /// <summary>
        /// 所在楼层Id
        /// </summary>
        public int? FloorId { get; set; }
        /// <summary>
        /// 所在房间Id
        /// </summary>
        public int? RoomId { get; set; }
        /// <summary>
        /// 建筑名
        /// </summary>
        public string BuildingName { get; set; }
        /// <summary>
        /// 楼层名
        /// </summary>
        public string FloorName { get; set; }
        /// <summary>
        /// 房间名
        /// </summary>
        public string RoomName { get; set; }
        /// <summary>
        /// 故障类型Id（见字典表MessageType）
        /// </summary>
        public int MeterMessageTypeId { get; set; }

    }

    /// <summary>
    /// 故障信息结果集
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IMalfunctionMeterData<T>
    {
        /// <summary>
        /// 故障设备信息
        /// </summary>
        public ICollection<T> MalfunctionMeterDatas { get; set; }
        
        /// <summary>
        /// 分页信息 
        /// </summary>
        public Pagination Pagination { get; set; }
    }
}
