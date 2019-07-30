using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class AliPayBaseInfo
    {
        public string AlipayId { get; set; }
        public string Seller { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public string AppKey { get; set; }
    }
}
