
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using WxPay2017.API.DAL;
using WxPay2017.API.DAL.EmpModels;

namespace WxPay2017.API.BLL
{
    public class DailyResultBLL : Repository<MeterDailyResult>
    {
         
        public DailyResultBLL(EmpContext context = null)
            : base(context)
        { 
        }
    }
}