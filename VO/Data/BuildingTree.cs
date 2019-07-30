using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class BuildingTree
    {
        public string TreeId { get; set; }

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

        public int? Type { get; set; }

        public string District { get; set; }

        public int? ManagerCount { get; set; }

        public int? CustomerCount { get; set; }

        public int? TotalArea { get; set; }

        public int? WorkingArea { get; set; }

        public int? LivingArea { get; set; }

        public int? ReceptionArea { get; set; }

        public string Description { get; set; }

        public int Rank { get; set; }

    }
}
