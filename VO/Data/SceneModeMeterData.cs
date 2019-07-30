using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public partial class SceneModeMeterData
    {
        public int Id { get; set; }

        public int SceneModeId { get; set; }

        public int? GroupId { get; set; }

        public int? BuildingId { get; set; }

        public int? MeterId { get; set; }

        public DateTime SettingTime { get; set; }

        public string OperatorId { get; set; }

        public string OperatorName { get; set; }
    }
}
