using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class GisMeterData
    {
        public GisMeterData()
        {
            Children = new List<GisMeterData>();
        }
        public int Id { get; set; }

        public string TreeId { get; set; }

        public int? Rank { get; set; }

        public int Type { get; set; }

        public string TypeName { get; set; }

        public int EnergyCategoryId { get; set; }

        public string EnergyCategoryName { get; set; }

        public int BrandId { get; set; }

        public string BrandName { get; set; }

        public int BrandType { get; set; }
        
        public string Name { get; set; }
           
        public string MacAddress { get; set; }

        public string Address { get; set; }
        public int? RelayElecState { get; set; }
        public bool? PaulElecState { get; set; }

        public int State { get; set; }
          
        public List<GisMeterData> Children { get; set; }

        public virtual ICollection<MeterStatusData> Status { get; set; }
    }
}
