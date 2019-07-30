using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class GisData
    {
        /// <summary>
        /// 对象ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 对象名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 节点类型： "building", "meter"
        /// </summary>
        public string Category { get; set; }

        public int Type { get; set; }

        public string TypeName { get; set; }

        public int? CoordinateId { get; set; }

        public decimal? X { get; set; }

        public decimal? Y { get; set; }

        public string Points { get; set; }

        public int State { get; set; }

        public string Icon { get; set; }

        public string Color { get; set; }

        public int InWarning { get; set; }
        public int? CityId { get; set; }
    }
}
