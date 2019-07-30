namespace WxPay2017.API.VO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using WxPay2017.API.DAL.EmpModels;
    using System.Linq;

    public partial class BuildingDiagram
    {
        public BuildingDiagram()
        {
            Children = new HashSet<BuildingDiagram>();
        }

        public int Id { get; set; }

        public int? ParentId { get; set; }

        public int? OrganizationId { get; set; }

        public int? BuildingCategoryId { get; set; }

        public int? CoordinateMapId { get; set; }

        public int? Coordinate3dId { get; set; }

        public int? Coordinate2dId { get; set; }


        public string GbCode { get; set; }

        public string Name { get; set; }

        public string AliasName { get; set; }

        public string Initial { get; set; }

        public int Type { get; set; }

        public int? ManagerCount { get; set; }

        public int? CustomerCount { get; set; }

        public decimal? TotalArea { get; set; }

        public decimal? WorkingArea { get; set; }

        public decimal? LivingArea { get; set; }

        public decimal? ReceptionArea { get; set; }

        public bool Enable { get; set; }

        public string Description { get; set; }

        public int? Year { get; set; }

        public int? UpFloor { get; set; }

        public int Sort { get; set; }

        public virtual BuildingDiagram Parent { get; set; }

        public virtual OrganizationDiagram Organization { get; set; }

        public virtual IEnumerable<BuildingDiagram> Children { get; set; }

        public virtual ICollection<ExtensionFieldData> ExtensionFields { get; set; }

    }
}
