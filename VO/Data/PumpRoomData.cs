using System;
using System.Collections.Generic;


namespace WxPay2017.API.VO.Data
{
    public partial class PumpRoomData
    {

        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string PumpName { get; set; }

        public string PumpAddress { get; set; }

        public int? Coordinate3dId { get; set; }

        public virtual ICollection<MeterData> MeterDatas { get; set; }

    }
}