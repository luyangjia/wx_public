using WxPay2017.API.DAL.EmpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL 
{
    public class UserExtensionBLL : Repository<WxPay2017.API.DAL.EmpModels.UserExtension>
    {

        public UserExtensionBLL(EmpContext context = null)
            : base(context)
        {
        }

    }


}
