using System.Collections.Generic;
namespace WxPay2017.API.VO
{
    public partial class BrandData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int MeterType { get; set; }

        public string Producer { get; set; }

        public string Model { get; set; }

        public string Description { get; set; }

        public string MeterTypeName { get; set; }

        public bool IsControllable { get; set; }
        public bool IsFJNewcapSystem { get; set; }

    }
}
