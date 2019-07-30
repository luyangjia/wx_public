using System;
using System.Collections.Generic;


namespace WxPay2017.API.VO.Data
{
    public partial class GatewayData
    {

        public int Id { get; set; }

        public string GatewayNo { get; set; }

        public string IPAddress { get; set; }

        public string EnablePort { get; set; }

        public string PortRule { get; set; }

        public string SpeedRate { get; set; }

        public string Address { get; set; }

        public string NetworkingArea { get; set; }

        //public int BrandId { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public virtual ICollection<MeterData> MeterDatas { get; set; }

    }

}
