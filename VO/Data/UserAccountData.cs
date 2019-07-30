using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class UserAccountData
    {

        public int Id { get; set; }

        public string UserId { get; set; }

      
        public string Balance { get; set; }

        public int BalanceTypeId { get; set; }

        public DateTime AddTime { get; set; }

        public bool Enable { get; set; }

        public virtual string BalanceTypeName { get; set; }

        public virtual UserData User { get; set; }
    }
}
