using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO.Param;
using WxPay2017.API.VO;
using WxPay2017.API.DAL.EmpModels;

namespace WxPay2017.API.BLL
{
    public class FeedbackBLL : Repository<Feedback>
    { 
        public FeedbackBLL(EmpContext context = null)
            : base(context)
        { 
        }
    }
}
