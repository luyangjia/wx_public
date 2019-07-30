using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO;

namespace WxPay2017.API.VO.Common
{
    public  static class UserCache
    {
        public static void Init()
        {
            UsersByTarget = new Dictionary<string, User>();
            TargetsByUser = new Dictionary<string, TargetData>();
        }

        //key:category+targetid
        public static Dictionary<string, User> UsersByTarget = new Dictionary<string, User>();
        //key:用户id
        public static Dictionary<string, TargetData> TargetsByUser = new Dictionary<string, TargetData>();

    }
}
