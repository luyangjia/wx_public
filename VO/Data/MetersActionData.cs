namespace WxPay2017.API.VO
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

   
    public partial class MetersActionData
    {

        public int Id { get; set; }

        public int MeterId { get; set; }

        public bool IsOk { get; set; }

        public bool? IsPowerOffByMoney { get; set; }

        public bool? IsPowerOffByTime { get; set; }

        public int ActionId { get; set; }

        public DateTime AddTime { get; set; }

        public DateTime? ActionTime { get; set; }

        public DateTime? AnswerTIme { get; set; }

        public string AnswerValue { get; set; }

        public int? SendTimes { get; set; }

        public int? Priority { get; set; }

        public string GroupNum { get; set; }

        public int? ParentId { get; set; }

        public string Description { get; set; }
        public string SettingValue { get; set; }
        public int? CommandStatus { get; set; }
        public string MeterMacAddress { get; set; }
        public string MeterAddress { get; set; }
        public string MeterName { get; set; }
        public string MeterTypeName { get; set; }
        public string InBuildingName { get; set; }
        public string CommandStatusName { get; set; }
        public string ActionName { get; set; }
    }

    public partial class MetersActionRecordData
    {
        public DateTime? ActionTime { get; set; }
        public DateTime? AnswerTIme { get; set; }
        public string AnswerValue { get; set; }
        public string MeterMacAddress { get; set; }
        public string MeterAddress { get; set; }
        public string MeterName { get; set; }
        public string MeterTypeName { get; set; }
        public string InBuildingName { get; set; }
        public string CommandStatusName { get; set; }
        public string ActionName { get; set; }
    }
}

