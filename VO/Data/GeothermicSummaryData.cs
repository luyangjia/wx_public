using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.VO.Common;

namespace WxPay2017.API.VO
{
    public class GeothermicSummaryData
    {

        public int Target { get; set; }

        public CategoryDictionary Category { get; set; }

        /// <summary>
        /// 冷负荷 | 地源热泵用户侧制冷（制热）量
        /// </summary>
        public decimal? Qsy { get; set; }

        /// <summary>
        /// 热负荷 | 地源热泵热源侧制冷（制热）量
        /// </summary>
        public decimal? Qsr { get; set; }

        /// <summary>
        /// 供暖季燃煤锅炉供暖系统的年耗能量
        /// </summary>
        public decimal Wr { get; set; }

        /// <summary>
        /// 供冷季系统的年耗电量
        /// </summary>
        public decimal Wl { get; set; }

        /// <summary>
        /// 常规系统年耗能量
        /// </summary>
        public decimal W { get; set; }

        /// <summary>
        /// 地源热泵系统年耗能量
        /// </summary>
        public decimal N { get; set; }

        /// <summary>
        /// 节能量
        /// </summary>
        public decimal SavedQuantity { get; set; }

        /// <summary>
        /// 节能率
        /// </summary>
        public decimal SavedRatio { get; set; }

        /// <summary>
        /// CO2 减排量
        /// </summary>
        public decimal CO2EmissionReduction { get; set; }

        /// <summary>
        /// SO2 减排量
        /// </summary>
        public decimal SO2EmissionReduction { get; set; }

        /// <summary>
        /// 粉尘减排量
        /// </summary>
        public decimal DustEmissionReduction { get; set; }

        public string NoteW { get; set; }

        /// <summary>
        /// 锅炉房供暖系统循环水泵、风机等用电设备的耗电量近似认为与地源热泵系统冷冻水泵耗电量
        /// </summary>
        public decimal Wls { get; set; }



        public int Year { get; set; }

        public int MeterId { get; set; }

        public int ParameterId { get; set; }

        public int ParameterType { get; set; }

    }
}