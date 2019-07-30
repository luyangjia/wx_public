namespace WxPay2017.API.VO
{
    using WxPay2017.API.VO.Common;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;


    public class StatisticalData
    {
        public int StatisticalId { get; set; }
        public int? StatisticalParentId { get; set; }
        public string StatisticalTreeId { get; set; }
        public string StatisticalName { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public StatisticalWay StatisticalWay { get; set; }

        public string WayName { get; set; }

        public int EnergyCategoryId { get; set; }

        public string EnergyCategoryName { get; set; }
        public decimal? StandardCoalCoefficient { get; set; }
        public string Formula { get; set; }
        //[JsonIgnore]
        public decimal? FormulaParam1 { get; set; }

        public int ParameterTypeId { get; set; }

        public string ParameterTypeName { get; set; }

        public string Unit { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TimeUnits TimeUnit { get; set; }

        public string TimeUnitName { get; set; }

        public IEnumerable<EnergyData> Result { get; set; }
        public decimal? Bill { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
    }
    /// <summary>
    /// ���ز������(����)
    /// </summary>
    public class StatisticsDifference
    {
        public decimal ParentStatistics { get; set; }//��ͳ�ƶ����ܶ�
        public decimal ChildrenStatistics { get; set; }//��ͳ�ƶ����ܶ�
        public decimal Difference { get; set; }//���
    }
}
