using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public partial class SceneModeData
    {
        public int Id { get; set; }

        public int ConfigTypeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
