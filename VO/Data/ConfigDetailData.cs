using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public partial class ConfigDetailData
    {

        public int Id { get; set; }

        public int? TemplateId { get; set; }

        public int? MeterId { get; set; }

        public int? BuildingId { get; set; }

        public int? OrganizationId { get; set; }

        public bool Enabled { get; set; }

        public string OperatorId { get; set; }

        public string OperatorName { get; set; }

        public DateTime CreateTime { get; set; }

        public int? BuildingCategoryId { get; set; }

        public int? EnergyCategoryId { get; set; }

        public bool? IsOpenOverLoadAlert { get; set; }

        public bool? IsOpenMalignantLoadAlert { get; set; }

        public bool? IsControlPower { get; set; }

        public bool? IsControlWater { get; set; }

        public bool? IsControlWaterByPower { get; set; }

        public int? VacationTimeControlTemplateId { get; set; }
        public virtual MonitoringConfigData VacationTimeControlTemplate { get; set; }
        public int? HolidayTimeControlTemplateId { get; set; }
        public virtual MonitoringConfigData HolidayTimeControlTemplate { get; set; }
        public int? WeekEndTimeControlTemplateId { get; set; }
        public virtual MonitoringConfigData WeekEndTimeControlTemplate { get; set; }
        public int? PeacetimeTimeControlTemplateId { get; set; }
        public virtual MonitoringConfigData PeacetimeTimeControlTemplate { get; set; }
        public bool? IsControlByAccount { get; set; }

        public bool? IsControlByTime { get; set; }
        public decimal? MinThresholdForMalignantLoad { get; set; }
        public decimal? MinThresholdForOverLoad { get; set; }
        public virtual DictionaryData BuildingCategory { get; set; }

        public virtual DictionaryData EnergyCategory { get; set; }
        public virtual MonitoringConfigData Template { get; set; }

    }
}
