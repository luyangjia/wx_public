
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
using WxPay2017.API.VO;
using WxPay2017.API.VO.Common;
using System.Text;
using WxPay2017.API.VO.Param;

namespace WxPay2017.API.BLL
{
    public class ViewBuildingFullInfoBLL : Repository<BuildingFullInfo>
    { 
        public ViewBuildingFullInfoBLL(EmpContext context = null)
            : base(context)
        {
        }


        public override BuildingFullInfo Find(params object[] keys)
        {
            try
            {
                int id = Convert.ToInt32(keys[0]);
                var fullInfos = db.BuildingFullInfo.Where(o => o.Id == id).ToList();
                if (fullInfos.Count() == 0)
                {
                    return null;
                }

                return fullInfos[0];
            }
            catch
            {
                return null;
            }
        }

    }
}