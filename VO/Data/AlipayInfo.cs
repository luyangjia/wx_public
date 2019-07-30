using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class AlipayInfo
    {
        public VO.Common.CategoryDictionary category { get; set; }
        public int targetId { get; set; }
        public string ParterId { get; set; }
        public string AccountInfo { get; set; }
        public string PrivateKeyUrl { get; set; }
        public string PublicKeyUrl { get; set; }
    }
}
