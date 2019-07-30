namespace WxPay2017.API.VO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using WxPay2017.API.DAL.EmpModels;
    using System.Linq;

    public partial class MeterData
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }


        public string TreeId { get; set; }

        public int? Rank { get; set; }

        public int? BuildingId { get; set; }

        public int EnergyCategoryId { get; set; }
        public string BuildingName { get; set; }
        public string BrandName { get; set; }

        public int BrandId { get; set; }
        public bool? IsControlByHand { get; set; }

        public int? CoordinateMapId { get; set; }

        public int? Coordinate3dId { get; set; }
        public int? RelayElecState { get; set; }
        public bool? PaulElecState { get; set; }
        public int? Coordinate2dId { get; set; }

        /// <summary>
        /// 柜号
        /// </summary>
        public string Code { get; set; }

        public DateTime? FinalSettlementTime { get; set; }
        public string GbCode { get; set; }


        public string Name { get; set; }


        public string AliasName { get; set; }

        public string Initial { get; set; }

        public int Type { get; set; }

        public string Access { get; set; }

        public int? Sequence { get; set; }

        public string Address { get; set; }


        public string MacAddress { get; set; }


        public bool Enable { get; set; }

        public string StatusNote { get; set; }

        public int? GetInterval { get; set; }

        public string BranchName { get; set; }

        public bool? BranchEnable { get; set; }

        public decimal Rate { get; set; }

        public bool IsTurnOn { get; set; }

        public string Description { get; set; }

        public int? SetupMode { get; set; }

        public int? PortNumber { get; set; }

        public string Rs485Address { get; set; }

        public decimal? ActivePrecise { get; set; }

        public decimal? ReactivePrecise { get; set; }

        public string BasicCurrent { get; set; }

        public string ComProtocol { get; set; }

        public decimal? SpeedRate { get; set; }

        public bool? IsHarmonic { get; set; }

        public int? Precision { get; set; }

        public decimal? PtRate { get; set; }

        public decimal? RangeRatio { get; set; }

        public int? Caliber { get; set; }

        public decimal? Hydraulic { get; set; }

        public decimal? Flow { get; set; }

        //public string SetupPosition { get; set; }

        public string SupplyRegion { get; set; }
        public decimal? EffectiveBasePrice { get; set; }
        public int? EffectiveModel { get; set; }
        public string Manufactor { get; set; }

        public DateTime? SetupDate { get; set; }

        public decimal? InitialValue { get; set; }

        public string PortDescription { get; set; }


        public virtual MeterData Parent { get; set; }

        public virtual MeterData Gateway { get; set; }

        public int HasChildren { get; set; }

        public virtual BuildingData Building { get; set; }

        public virtual DictionaryData EnergyCategory { get; set; }

        public virtual BrandData Brand { get; set; }

        public virtual DictionaryData TypeDict { get; set; }

        public virtual OrganizationData Organization { get; set; }

        public virtual ICollection<MomentaryValueData> MomentaryValues { get; set; }

        public virtual ICollection<ExtensionFieldData> ExtensionFields { get; set; }

        public virtual ICollection<MeterStatusData> Status { get; set; }

        public virtual ICollection<MeterData> Children { get; set; }

        public virtual ICollection<MeterData> Siblings { get; set; }

        public virtual ICollection<MeterData> Descendants { get; set; }

        public virtual ICollection<MeterData> Ancestors { get; set; }
    }

    public class MeterShortData
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public int? RelayElecState { get; set; }
        public bool? PaulElecState { get; set; }
        public string TreeId { get; set; }

        public int? Rank { get; set; }

        public int? BuildingId { get; set; }

        public int EnergyCategoryId { get; set; }


        public int BrandId { get; set; }


        public int? CoordinateMapId { get; set; }

        public int? Coordinate3dId { get; set; }

        public int? Coordinate2dId { get; set; }

        /// <summary>
        /// 柜号
        /// </summary>
        public string Code { get; set; }


        public string GbCode { get; set; }


        public string Name { get; set; }


        public string AliasName { get; set; }

        public string Initial { get; set; }

        public int Type { get; set; }

        public string Access { get; set; }

        public int? Sequence { get; set; }

        public string Address { get; set; }


        public string MacAddress { get; set; }


        public bool Enable { get; set; }

        public string StatusNote { get; set; }

        public int? GetInterval { get; set; }

        public string BranchName { get; set; }

        public bool? BranchEnable { get; set; }

        public decimal Rate { get; set; }

        public bool IsTurnOn { get; set; }

        public string Description { get; set; }

        public int? SetupMode { get; set; }

        public int? PortNumber { get; set; }
    }

    /// <summary>
    /// 网关设备数据
    /// </summary>
    public partial class GatewayMeterData
    {
        /// <summary>
        /// 网关名称
        /// </summary>
        public string gatewayName { get; set; }
        /// <summary>
        /// 品牌型号
        /// </summary>
        public string brandName { get; set; }
        /// <summary>
        /// ip地址
        /// </summary>
        public string access { get; set; }
        /// <summary>
        /// 网关编号
        /// </summary>
        public string gbcode { get; set; }
        /// <summary>
        /// 安装位置
        /// </summary>
        public string setupAddress { get; set; }
        /// <summary>
        /// 网关型号
        /// </summary>
        //public string gatewayBrand { get; set; }
        /// <summary>
        /// 端口数量
        /// </summary>
        public int? portNumber { get; set; }
        /// <summary>
        /// 联网区域
        /// </summary>
        public string supplyRegion { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string manufactor { get; set; }


        ///// <summary>
        ///// 建筑名
        ///// </summary>
        //public string BuildingName { get; set; }
        ///// <summary>
        ///// 网关名称
        ///// </summary>
        //public string MeterName { get; set; }
        ///// <summary>
        ///// 网关型号
        ///// </summary>
        //public string BrandName { get; set; }
        ///// <summary>
        ///// Ip地址
        ///// </summary>
        //public string Access { get; set; }
        ///// <summary>
        ///// 端口详情
        ///// </summary>
        //public string PortDescription { get; set; }
        ///// <summary>
        ///// 安装位置
        ///// </summary>
        //public string Address { get; set; }
        ///// <summary>
        ///// 联网区域
        ///// </summary>
        //public string SupplyRegion { get; set; }
        ///// <summary>
        ///// 生产厂家
        ///// </summary>
        //public string Manufactor { get; set; }
        ///// <summary>
        ///// 安装日期
        ///// </summary>
        //public DateTime? SetupDate { get; set; }

    }

    public partial class ElecMeterData 
    {
        public string energyCategory { get; set; }
        public string floor { get; set; }
        public string room { get; set; }
        public string electricName { get; set; }
        public string setupMode { get; set; }
        public int? electricPrecision { get; set; }
        public decimal? ptVoltageRatio { get; set; }
        public string basicCurrent { get; set; }
        public decimal? turndownRatio { get; set; }
        public string communicationProtocol { get; set; }
        public decimal? communicationSpeed { get; set; }
        public string brandName { get; set; }
        public string gbCode { get; set; }
        public string macAddress { get; set; }
        public string communicationPort { get; set; }
        public string ctVoltageRatio { get; set; }
        public int? port { get; set; }
        public string supplyRegion { get; set; }
        public string manufactory { get; set; }
        public decimal? initialValue { get; set; }
        public decimal baudRate { get; set; }
        public string protocol { get; set; }
        public string ipAddress { get; set; }
        public DateTime? setupDate { get; set; }
        public string setupAddress { get; set; }
    }



    /// <summary>
    /// 水表设备数据
    /// </summary>
    public partial class WaterMeterData
    {
        public string energyCategory { get; set; }
        public string floor { get; set; }
        public string room { get; set; }
        public string waterName { get; set; }
        public string setupMode { get; set; }
        public int? waterPrecision { get; set; }
        public decimal? caliber { get; set; }
        public decimal? waterGage { get; set; }
        public decimal? flowRate { get; set; }
        public string communicationProtocol { get; set; }
        public decimal? communicationSpeed { get; set; }
        public string brandName { get; set; }
        public string gbCode { get; set; }
        public string macAddress { get; set; }
        public string communicationPort { get; set; }
        public decimal? meterOverride { get; set; }
        public int? port { get; set; }
        public string supplyRegion { get; set; }
        public string manufactory { get; set; }
        public decimal? initialValue { get; set; }
        public decimal baudRate { get; set; }
        public string protocol { get; set; }
        public string ipAddress { get; set; }
        public DateTime? setupDate { get; set; }
        public string setupAddress { get; set; }


    }


}
