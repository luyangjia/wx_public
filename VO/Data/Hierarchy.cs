using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO.Data
{
    public class GateWayHierarchy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<MeterHierarchy> Meters { get;set; }
    }
    public class MeterHierarchy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? GatewayId { get; set; }
        public int? BuildingId { get; set; }
        public int? OrganizationId { get; set; }
        public int? DistributionId { get; set; }
        public int? PumpRoomId { get; set; }
        public int? EnergyCategoryId { get; set; }
        public string EnergyCategoryName { get; set; }
        public int? EnergyCategoryFirstValue { get; set; }
        public string TreeId { get; set; }
        public int? ParentId { get; set; }
        public List<MeterHierarchy> Children { get; set; }
        public string BuildingTreeId { get; set; }
    }
}
