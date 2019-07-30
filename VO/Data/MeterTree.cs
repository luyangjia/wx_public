using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class MeterTree
    {
        public string TreeId { get; set; }

        public int? Id { get; set; }

        public int? ParentId { get; set; }

        public int? BuildingId { get; set; }

        public int? EnergyCategoryId { get; set; }

        public int? BrandId { get; set; }

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

        public int? Type { get; set; }

        public string Access { get; set; }

        public int? Sequence { get; set; }

        public string Address { get; set; }

        public string MacAddress { get; set; }

        public int? Status { get; set; }

        public string StatusNote { get; set; }

        public int? GetInterval { get; set; }

        public string BranchName { get; set; }

        public bool? BranchEnable { get; set; }

        public decimal? Rate { get; set; }

        public string Description { get; set; }

        public int? Rank { get; set; }

    }
}
