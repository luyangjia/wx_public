using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.DAL.EmpModels
{
    [Table("Meter.StatisticalRelation")]
    public class StatisticalRelation
    {
        public string OrganizationTreeId { get; set; }
        public int OrganizationId { get; set; }
        public int? OrganizationParentId { get; set; }
        public int OrganizationTypeId { get; set; }
        public string OrganizationName { get; set; }
        public bool OrganizationEnable { get; set; }
        public int OrganizationRank { get; set; }
        public string BuildingTreeId { get; set; }
        public int BuildingId { get; set; }
        public int? BuildingParentId { get; set; }
        public int BuildingCategoryId { get; set; }
        public string BuildingName { get; set; }
        public int BuildingTypeId { get; set; }
        public string BuildingDistrict { get; set; }
        public bool BuildingEnable { get; set; }
        public int BuildingRank { get; set; }
        public string MeterTreeId { get; set; }
        [Key]
        public int MeterId { get; set; }
        public int? MeterParentId { get; set; }
        public int EnergyCategoryId { get; set; }
        public string MeterName { get; set; }
        public int MeterTypeId { get; set; }
        public bool MeterEnable { get; set; }
        public int MeterRank { get; set; }
    }
}
