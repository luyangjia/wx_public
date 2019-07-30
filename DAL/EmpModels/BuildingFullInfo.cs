
namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Building.BuildingFullInfo")]
    public partial class BuildingFullInfo
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? ParentId { get; set; }

        [StringLength(128)]
        public string TreeId { get; set; }

        public int? Rank { get; set; }

        public int? OrganizationId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BuildingCategoryId { get; set; }

        public int? CoordinateMapId { get; set; }

        public int? Coordinate3dId { get; set; }

        public int? Coordinate2dId { get; set; }

        [StringLength(16)]
        public string GbCode { get; set; }

        [Key]
        [Column(Order = 2)]
        public string Name { get; set; }

        [StringLength(128)]
        public string AliasName { get; set; }

        [StringLength(16)]
        public string Initial { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Type { get; set; }

        [StringLength(128)]
        public string District { get; set; }

        public int? ManagerCount { get; set; }

        public int? CustomerCount { get; set; }

        public decimal? TotalArea { get; set; }

        public decimal? WorkingArea { get; set; }

        public decimal? LivingArea { get; set; }

        public decimal? ReceptionArea { get; set; }

        public bool BuildingEnable { get; set; }

        [StringLength(256)]
        public string Description { get; set; }


        public int? Year { get; set; }

        public int? UpFloor { get; set; }

        public int Sort { get; set; }

        public int? HasChildren { get; set; }

        [StringLength(128)]
        public string TypeName { get; set; }

        [StringLength(128)]
        public string BuildingCategoryName { get; set; }

        public int? OrganizationParentId { get; set; }

        [StringLength(128)]
        public string OrganizationName { get; set; }

        [StringLength(128)]
        public string OrganizationAliasName { get; set; }

        [StringLength(128)]
        public string OrganizationTreeId { get; set; }

        public bool? OrganizationEnable { get; set; }

        public int? ParentParentId { get; set; }

        [StringLength(128)]
        public string ParentTreeId { get; set; }

        public int? ParentOrganizationId { get; set; }

        public int? ParentBuildingCategoryId { get; set; }

        [StringLength(16)]
        public string ParentGbCode { get; set; }

        [StringLength(128)]
        public string ParentName { get; set; }

        [StringLength(128)]
        public string ParentAliasName { get; set; }

        public int? ParentType { get; set; }

        public int? MaxCustomerCount { get; set; }

        public int? Purpose { get; set; }
    }
}
