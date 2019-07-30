using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public partial class RatedParameterData
    {
        public int Id { get; set; }

        public int? RatedParameterTypeId { get; set; }

        public int? BrandId { get; set; }

        public int? ParameterId { get; set; }

        public string Description { get; set; }

        public decimal? MinValue { get; set; }

        public decimal? MaxValue { get; set; }

        public string Code { get; set; }

        public decimal? PPF { get; set; }

        public decimal? RPF { get; set; }

        public DateTime SettingTime { get; set; }
        public decimal? PPFMax { get; set; }
        public decimal? PPFMin { get; set; }
        public decimal? RPFMax { get; set; }
        public decimal? RPFMin { get; set; }
        public ParameterData Parameter { get; set; }
    }
}
