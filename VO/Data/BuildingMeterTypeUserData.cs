using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public partial class BuildingMeterTypeUserData
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int MeterTypeId { get; set; }

        public int BuildingId { get; set; }

        public bool? Enable { get; set; }
        public UserData User { get; set; }
        //public DictionaryData MeterType { get; set; }
        public string MeterTypeName { get; set; }
        public BuildingData Building { get; set; }

    }
}
