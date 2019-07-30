namespace WxPay2017.API.VO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using WxPay2017.API.DAL.EmpModels;
    using WxPay2017.API.VO;
    using System.Linq;

    public partial class BuildingData
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public int? OrganizationId { get; set; }

        public int BuildingCategoryId { get; set; }

        public int? CoordinateMapId { get; set; }

        public int? Coordinate3dId { get; set; }

        public int? Coordinate2dId { get; set; }


        public string GbCode { get; set; }

        public string Name { get; set; }

        public string AliasName { get; set; }

        public string Initial { get; set; }
         
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }

        public int Type { get; set; }

        public int? ManagerCount { get; set; }

        public int? CustomerCount { get; set; }

        public decimal? TotalArea { get; set; }

        public decimal? WorkingArea { get; set; }

        public decimal? LivingArea { get; set; }

        public decimal? ReceptionArea { get; set; }
        public string TreeId { get; set; }
        public bool Enable { get; set; }

        public string Description { get; set; }

        public int? Year { get; set; }

        public int? UpFloor { get; set; }

        public int Sort { get; set; }

        public BuildingData Parent { get; set; }


        public int HasChildren { get; set; }

        public int? MaxCustomerCount { get; set; }

        public int? Purpose { get; set; }


        public virtual ICollection<BuildingData> Children { get; set; }

        public virtual ICollection<BuildingData> Descendants { get; set; }
        public virtual ICollection<BuildingData> Ancestors { get; set; }


        public virtual DictionaryData TypeDict { get; set; }

        public virtual DictionaryData BuildingCategory { get; set; }

        public virtual CoordinateData Coordinate2d { get; set; }

        public virtual CoordinateData Coordinate3d { get; set; }

        public virtual CoordinateData CoordinateMap { get; set; }


        public virtual OrganizationData Organization { get; set; }

        public virtual ICollection<MeterData> Meters { get; set; }

        public virtual ICollection<ExtensionFieldData> ExtensionFields { get; set; }

        public virtual ProvinceData Province { get; set; }

        public virtual CityData City { get; set; }

        public virtual DistrictData District { get; set; }
    }

    public class BuildingShortData
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public int? OrganizationId { get; set; }

        public int BuildingCategoryId { get; set; }

        public int? CoordinateMapId { get; set; }

        public int? Coordinate3dId { get; set; }

        public int? Coordinate2dId { get; set; }


        public string GbCode { get; set; }

        public string Name { get; set; }

        public string AliasName { get; set; }

        public string Initial { get; set; }

        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }

        public int Type { get; set; }

        public int? ManagerCount { get; set; }

        public int? CustomerCount { get; set; }

        public decimal? TotalArea { get; set; }

        public decimal? WorkingArea { get; set; }

        public decimal? LivingArea { get; set; }

        public decimal? ReceptionArea { get; set; }
        public string TreeId { get; set; }
        public bool Enable { get; set; }

        public string Description { get; set; }

        public int? Year { get; set; }

        public int? UpFloor { get; set; }

        public int Sort { get; set; }

        public int? MaxCustomerCount { get; set; }

        public int? Purpose { get; set; }
        public int HasChildren { get; set; }

    }

    /// <summary>
    /// 批量生成建筑信息
    /// </summary>
    public class BuildingBatchData {
        /// <summary>
        /// 基础建筑信息
        /// </summary>
        public BuildingData buildingData { get; set; }
        /// <summary>
        /// 楼层开始层数
        /// </summary>
        public int floorNumStart { get; set; }
        /// <summary>
        /// 楼层总数
        /// </summary>
        public int floorNum { get; set; }
        /// <summary>
        /// 房间数量/每层
        /// </summary>
        public int roomNum { get; set; }

        /// <summary>
        /// 房间床位数
        /// </summary>
        public int maxCustomerCount { get; set; }

    }

    /// <summary>
    /// 批量生成楼层信息
    /// </summary>
    public class FloorBatchData
    {
        /// <summary>
        /// 基础楼层信息
        /// </summary>
        public BuildingData buildingData { get; set; }

        /// <summary>
        /// 房间数量/每层
        /// </summary>
        public int roomNum { get; set; }

        /// <summary>
        /// 房间床位数
        /// </summary>
        public int maxCustomerCount { get; set; }

        /// <summary>
        /// 当前楼层序号(用于房间命名)
        /// </summary>
        public int floorNum { get; set; }

    }

    /// <summary>
    /// 批量生成建筑数据
    /// </summary>
    public class BatchData
    {
        /// <summary>
        /// 建筑、楼层数据
        /// </summary>
        public BuildingData build { get; set; }
        /// <summary>
        /// 建筑、楼层id
        /// </summary>
        public int? bid { get; set; }
        /// <summary>
        /// 楼层起始序号
        /// </summary>
        public int? from { get; set; }
        /// <summary>
        /// 楼层结束序号
        /// </summary>
        public int? to { get; set; }

        /// <summary>
        /// 起始房间号
        /// </summary>
        public int? roomFrom { get; set; }

        /// <summary>
        /// 房间数/每层
        /// </summary>
        public int room { get; set; }
        /// <summary>
        /// 床位数
        /// </summary>
        public int? bed { get; set; }

    }
    public class BuildingMiniInfo
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public String Name { get; set; }
        public String FloorName { get; set; }
        public String BuildingName { get; set; }
    }
}
