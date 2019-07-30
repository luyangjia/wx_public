using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace WxPay2017.API.BLL
{
    public static class MeterResultExtension
    {
        public static StatisticalMeta ToViewData(this IMeterResult node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new StatisticalMeta
            {
                Id = node.Id,
                MeterId = node.MeterId,
                Meter = node.Meter.ToViewData(),
                StartTime = node.StartTime,
                FinishTime = node.FinishTime,
                Total = node.Total,
                Unit = node.Unit,
                ParameterId = node.ParameterId,
                Status = node.Status,
                StatusDict = node.StatusDict.ToViewData()
            };
        }


        public static IEnumerable<StatisticalMeta> ToViewList(this IQueryable<IMeterResult> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return nodes.ToList().Select(x => x.ToViewData(suffix));
        }
    }
}
