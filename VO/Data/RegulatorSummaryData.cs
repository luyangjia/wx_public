using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class RegulatorSummaryData
    {
        public virtual ICollection<BalanceData> LowBalances { get; set; }
        public virtual ICollection<BalanceData> Arrearage { get; set; }
    }
}
