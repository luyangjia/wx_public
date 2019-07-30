using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class OriginalSearchData
    {
        public int Id { get; set; }
        public IList<int> Parameters { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public int? Size { get; set; }
    }
}
