using WxPay2017.API.DAL.EmpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO.Param;
using WxPay2017.API.VO;

namespace WxPay2017.API.BLL
{
    public class BrandBLL : Repository<Brand>
    {


        public BrandBLL(EmpContext context = null)
            : base(context)
        {
        }


    }
}
