using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class PayForBuilding
    {
        public decimal PaymentAmount { get; set; }
        public List<int> Ids { get; set; }
    }
}
