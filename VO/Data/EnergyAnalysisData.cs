using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.VO.Common;

namespace WxPay2017.API.VO
{
    public class TargetData
    {
        public CategoryDictionary Category { get; set; }
        public int TargetId { get; set; }
        public string Name { get; set; }
        public DateTime beginTime { get; set; }
        public int Period { get; set; }

    }

    //public class TargetContrastData
    //{
    //    /// <summary>
    //    /// 当前能耗量
    //    /// </summary>
    //    public decimal ValueNow { get; set; }
    //    /// <summary>
    //    /// 上一周期能耗量
    //    /// </summary>
    //    public decimal ValueLast { get; set; }
    //    /// <summary>
    //    /// 同比增量
    //    /// </summary>
    //    public decimal AnValueAdd { get; set; }
    //    /// <summary>
    //    /// 同比
    //    /// </summary>
    //    public string AnValue { get; set; }
    //    /// <summary>
    //    /// 环比增量
    //    /// </summary>
    //    public decimal? MomValueAdd { get; set; }
    //    /// <summary>
    //    /// 环比量
    //    /// </summary>
    //    public string MomValue { get; set; }
    //    /// <summary>
    //    /// 时间
    //    /// </summary>
    //    public DateTime Time { get; set; }

    //}

    public class EnergyAnalysisFullData
    {
        public string tabulatingPrefix { get; set; }
        public string title { get; set; }
        public string timeRange { get; set; }
        public string timeNow { get; set; }
        public List<List<string>> data { get; set; }
    }

    public class EnergyAnalysisData
    {
        public string no { get; set; }
        /// <summary>
        /// 当前能耗量
        /// </summary>
        public string valueNow { get; set; }
        /// <summary>
        /// 上一周期能耗量
        /// </summary>
        public string valueLast { get; set; }
        /// <summary>
        /// 同比增量
        /// </summary>
        public string anValueAdd { get; set; }
        /// <summary>
        /// 同比
        /// </summary>
        public string anValue { get; set; }
        /// <summary>
        /// 环比增量
        /// </summary>
        public string momValueAdd { get; set; }
        /// <summary>
        /// 环比量
        /// </summary>
        public string momValue { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public string time { get; set; }
    }
}
