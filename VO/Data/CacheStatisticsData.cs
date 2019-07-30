using System;
using System.Collections.Generic;

namespace WxPay2017.API.VO.Data
{
    public partial class CacheStatisticsDataData
    {

        public long Id { get; set; }

        public int TargetId { get; set; }

        public string StatMode { get; set; }

        public int EnergyCategoryId { get; set; }

        public int ParameterTypeId { get; set; }

        public string TimeUnit { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }

        public decimal Value { get; set; }

    }
}
