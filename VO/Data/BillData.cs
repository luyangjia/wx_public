using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class BillData
    {
        public decimal PayFee { get; set; }
        public decimal PeriodicEnergyConsumption { get; set; }
        public string Unit { get; set; }
        public int EnergyCategoryId { get; set; }
        public string EnergyCategoryName { get; set; }
        public string Desc { get; set; }

        public int targetId { get; set; }
    }
}
