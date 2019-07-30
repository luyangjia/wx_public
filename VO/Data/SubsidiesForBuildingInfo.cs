using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.VO
{
    public class SubsidiesForBuildingInfo
    {
        
        public int BuildingId { get; set; }
        public string ReceiverId { get; set; }
        public IEnumerable<UsersForSubsidies> UserInfos { get; set; }
        public string BeforeAccount { get; set; }
        public string EndAccount { get; set; }
        public decimal Value { get; set; }
    }
    public class UsersForSubsidies
    {
        public string UserId { get; set; }
        public string UserFullName { get; set; }
    }
}
