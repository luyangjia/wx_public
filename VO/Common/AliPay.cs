using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMP2016.API.VO.Common
{
    public class AliPay
    {
        public static string sign(string orderInfo,string path)
        {
            return Aop.Api.Util.RSAUtil.RSASign(orderInfo, path, null);
        }
    }
}
