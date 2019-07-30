using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    public static class MomentaryExtension
    {
        public static MomentaryValueData ToViewData(this MomentaryValue node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            //var RatioParameterList = new List<int>() { 60004, 60005, 60006, 60007, 60008, 60009, 60010, 60011, 60012, 60019, 60020, 60021, 60022 };
            var RatioParameterList = DictionaryCache.RatioParameterList;
            var model = new MomentaryValueData()
            {
                Id = node.Id,
                MeterId = node.MeterId,
                ParameterId = node.ParameterId,
                ParameterName = node.Parameter == null ? "" : node.Parameter.Type.ChineseName,
                ParameterType = node.Parameter == null ? 0 : node.Parameter.TypeId,
                Value = node.Meter == null ? node.Value : (RatioParameterList.Contains(node.Parameter == null ? 0 : node.Parameter.TypeId) ? node.Value * node.Meter.Rate : node.Value),
                Time = node.Time,
                Unit = node.Parameter == null ? "" : node.Parameter.Unit,
                OriginalValue = node.Value
                //Parameter = node.Parameter.ToViewData()
            };
            if ((suffix & CategoryDictionary.Parameter) == CategoryDictionary.Parameter)
            {
                model.Parameter = node.Parameter.ToViewData();
            }
            if ((suffix & CategoryDictionary.Meter) == CategoryDictionary.Meter)
            {
                model.Meter = node.Meter.ToViewData();
            }

            return model;

        }

        public static IEnumerable<MomentaryValueData> ToViewList(this IQueryable<MomentaryValue> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return nodes.ToList().Select(x => x.ToViewData(suffix));
        }

        public static MomentaryValue ToModel(this MomentaryValueData node)
        {
            return new MomentaryValue()
            {
                Id = node.Id,
                MeterId = node.MeterId,
                ParameterId = node.ParameterId,
                Value = node.Value,
                Time = node.Time
            };
        }
    }
}
