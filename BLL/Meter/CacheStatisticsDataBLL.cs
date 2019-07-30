using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.DAL.EmpModels;

namespace WxPay2017.API.BLL
{
    public class CacheStatisticsDataBLL : Repository<CacheStatisticsData>
    {
        public CacheStatisticsDataBLL(EmpContext context = null)
            : base(context)
        {
        }

    }
}
