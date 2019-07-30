using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class StatisticalMeta
    {
        public Guid Id { get; set; }

        public int MeterId { get; set; }

        /// <summary>
        /// 统计对象名称
        /// </summary>
        public MeterData Meter { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }

        public decimal Total { get; set; }

        public string Unit { get; set; }

        public int ParameterId { get; set; }

        public string ParameterName { get; set; }

        public int Status { get; set; }

        public virtual DictionaryData StatusDict { get; set; }
    }
}
