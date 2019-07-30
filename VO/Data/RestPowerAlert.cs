using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class RestPowerAlert
    {
        public string OrgName { get; set; }
        public int? OrgId { get; set; }
        public string BuildingName { get; set; }
        public int? BuildingId { get; set; }
        public int? MeterId { get; set; }
        public string MeterName { get; set; }
        public string AlertName { get; set; }
        public int? AlertTypeId { get; set; }
        public string EnergyTypeName { get; set; }
        public int? EnergyTypeId { get; set; }
        public decimal? Value { get; set; }
        public bool? IsSendedMessageThisTure { get; set; }

        public string Unit { get; set; }


    }
}
