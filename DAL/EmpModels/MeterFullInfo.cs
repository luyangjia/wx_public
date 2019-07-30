namespace WxPay2017.API.DAL.EmpModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Meter.MeterFullInfo")]
    public partial class MeterFullInfo
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(128)]
        public string BuildingName { get; set; }

        [StringLength(128)]
        public string brandName { get; set; }

        [StringLength(128)]
        public string TypeName { get; set; }

        [StringLength(128)]
        public string EnergyCategoryName { get; set; }

        public int? HasChildren { get; set; }

        public int? PID { get; set; }

        public int? ParentId { get; set; }

        [StringLength(128)]
        public string ParentName { get; set; }

        [StringLength(128)]
        public string TreeId { get; set; }

        public int? Rank { get; set; }

        public int? BuildingId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EnergyCategoryId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BrandId { get; set; }

        public int? CoordinateMapId { get; set; }

        public int? Coordinate3dId { get; set; }

        public int? Coordinate2dId { get; set; }

        [StringLength(32)]
        public string Code { get; set; }

        [StringLength(16)]
        public string GbCode { get; set; }

        [Key]
        [Column(Order = 3)]
        public string Name { get; set; }

        [StringLength(128)]
        public string AliasName { get; set; }

        [StringLength(16)]
        public string Initial { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Type { get; set; }

        [StringLength(128)]
        public string Access { get; set; }

        public int? Sequence { get; set; }

        [StringLength(256)]
        public string Address { get; set; }

        [StringLength(256)]
        public string MacAddress { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool Enable { get; set; }

        public string StatusNote { get; set; }

        public int? GetInterval { get; set; }

        [StringLength(10)]
        public string BranchName { get; set; }

        public bool? BranchEnable { get; set; }

        [Key]
        [Column(Order = 6)]
        public decimal Rate { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        public int? OrganizationId { get; set; }

        public int? BuildingCategoryId { get; set; }

        [StringLength(16)]
        public string BuildingGBCode { get; set; }

        public string Producer { get; set; }
        public string Model { get; set; }
        public string BrandDescription { get; set; }

        public bool IsTurnOn { get; set; }
    }
}
