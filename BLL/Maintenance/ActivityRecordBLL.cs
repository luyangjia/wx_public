using WxPay2017.API.DAL.EmpModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL 
{
    public class ActivityRecordBLL : Repository<ActivityRecord>
    {
        public ActivityRecordBLL(EmpContext context = null)
            : base(context)
        { 
        }
    }
}
