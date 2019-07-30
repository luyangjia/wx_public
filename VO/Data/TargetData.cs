using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIR2017.API.VO.Common;

namespace AIR2017.API.VO.Data
{
    public class TargetData
    {
        public CategoryDictionary Category { get; set; }
        public int TargetId { get; set; }
        public string Name { get; set; }
        public DateTime beginTime { get; set; }
        public int Period { get; set; }

    }

    public class TargetContrastData
    {
        /// <summary>
        /// 当前能耗量
        /// </summary>
        public decimal ValueNow { get; set; }
        /// <summary>
        /// 上一周期能耗量
        /// </summary>
        public decimal ValueLast { get; set; }
        /// <summary>
        /// 同比增量
        /// </summary>
        public decimal AnValueAdd { get; set; }
        /// <summary>
        /// 同比
        /// </summary>
        public string AnValue { get; set; }
        /// <summary>
        /// 环比增量
        /// </summary>
        public decimal? MomValueAdd { get; set; }
        /// <summary>
        /// 环比量
        /// </summary>
        public string MomValue { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }

    }
}
