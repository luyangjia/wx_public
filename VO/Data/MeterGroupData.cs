using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public partial class MeterGroupData
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public int? MeterId { get; set; }

        public int? BuildingId { get; set; }

        public bool Enable { get; set; }
        public GroupData Group { get; set; }
        public MeterData Meter { get; set; }
        public BuildingData Building { get; set; }
    }
}
