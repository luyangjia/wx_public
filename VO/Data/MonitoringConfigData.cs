using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class MonitoringConfigData
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TargetTypeId { get; set; }

        public int TargetId { get; set; }

        public int? EnergyCategoryId { get; set; }

        public int ConfigTypeId { get; set; }

        public int? WayId { get; set; }

        public decimal? UnitValue { get; set; }

        public decimal? LowerLimit { get; set; }

        public decimal? UpperLimit { get; set; }

        public decimal? Value { get; set; }

        public DateTime? ValidStartTime { get; set; }

        public DateTime? ValidEndTime { get; set; }
        public int? ValidTypeId { get; set; }
        public int? Priority { get; set; }

        public bool Enabled { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string RegeneratorId { get; set; }

        public string RegeneratorName { get; set; }

        public DateTime? UpdatingTime { get; set; }

        public int? ParameterId { get; set; }

        public DateTime? CycleTime { get; set; }

        public int? CycleTypeId { get; set; }

        public int? AlarmLevelId { get; set; }
       
       
        public bool? IsvalidNextCycle { get; set; }
        public string ValidTypeDesc { get; set; }
        public string CycleTimeName { get; set; }
        public string CycleTimeDesc { get; set; }

        public  DictionaryData EnergyCategory { get; set; }
        public DictionaryData ValidType { get; set; }
        public DictionaryData CycleType { get; set; }
       
        public DictionaryData AlarmLevel { get; set; }

        public DictionaryData Way { get; set; }

        public DictionaryData ConfigType { get; set; }
        public DictionaryData TargetType { get; set; }
        public  ParameterData Parameter { get; set; }

        public Object Target { get; set; }

        public bool isAlowEdit { get; set; }

        public int? TemplateId { get; set; }
        public DateTime? OverTimeDate { get; set; }
        public string TemplateName { get; set; }

        public List<DictionaryData> AllowActions { get; set; }
        public string ActionName { get; set; }

        public IList<ConfigDetailData> ConfigDetails { get; set; }
        public IList<ConfigCycleSettingData> ConfigCycleSettings { get; set; }
    }
}
