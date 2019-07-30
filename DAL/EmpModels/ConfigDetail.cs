using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Spatial;

namespace WxPay2017.API.DAL.EmpModels
{
    [Table("Foundation.ConfigDetail")]
    public partial class ConfigDetail
    {
        public int Id { get; set; }

        public int? TemplateId { get; set; }

        public int? MeterId { get; set; }

        public int? BuildingId { get; set; }

        public int? OrganizationId { get; set; }

        public bool Enabled { get; set; }

        [StringLength(128)]
        public string OperatorId { get; set; }

        [StringLength(256)]
        public string OperatorName { get; set; }

        public DateTime CreateTime { get; set; }

        public virtual Building Building { get; set; }

        public virtual Meter Meter { get; set; }

        public virtual Organization Organization { get; set; }

        public virtual MonitoringConfig Template { get; set; }

        public virtual User OperatorUser { get; set; }

        public int? BuildingCategoryId { get; set; }

        public int? EnergyCategoryId { get; set; }

        public bool? IsOpenOverLoadAlert { get; set; }

        public bool? IsOpenMalignantLoadAlert { get; set; }

        public bool? IsControlPower { get; set; }

        public bool? IsControlWater { get; set; }

        public bool? IsControlWaterByPower { get; set; }

        public int? VacationTimeControlTemplateId { get; set; }

        public int? HolidayTimeControlTemplateId { get; set; }

        public int? WeekEndTimeControlTemplateId { get; set; }

        public int? PeacetimeTimeControlTemplateId { get; set; }

        public bool? IsControlByAccount { get; set; }

        public bool? IsControlByTime { get; set; }
        public decimal? MinThresholdForMalignantLoad { get; set; }
        public decimal? MinThresholdForOverLoad { get; set; }
        public virtual Dictionary BuildingCategory { get; set; }

        public virtual Dictionary EnergyCategory { get; set; }

        public virtual MonitoringConfig HolidayTimeControlTemplate { get; set; }

        public virtual MonitoringConfig PeacetimeTimeControlTemplate { get; set; }

       

        public virtual MonitoringConfig MonitoringConfigTemplate { get; set; }

        public virtual MonitoringConfig VacationTimeControlTemplate { get; set; }

      

        public virtual MonitoringConfig WeekEndTimeControlTemplate { get; set; }
    }
}
