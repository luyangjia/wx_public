using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    public static class StatisticsSubscribeHistoryExtension
    {
        #region StatisticsSubscribeHistory
        public static StatisticsSubscribeHistoryData ToViewData(this StatisticsSubscribeHistory node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new StatisticsSubscribeHistoryData()
            {
                Id = node.Id,
                StatisticsSubscribeId = node.StatisticsSubscribeId,
                SendDate = node.SendDate,

            };
        }

        public static IList<StatisticsSubscribeHistoryData> ToViewList(this IQueryable<StatisticsSubscribeHistory> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new StatisticsSubscribeHistoryData()
            {
                Id = node.Id,
                StatisticsSubscribeId = node.StatisticsSubscribeId,
                SendDate = node.SendDate,

            }).ToList();
            return results;
        }

        public static StatisticsSubscribeHistory ToModel(this StatisticsSubscribeHistoryData node)
        {
            return new StatisticsSubscribeHistory()
            {
                Id = node.Id,
                StatisticsSubscribeId = node.StatisticsSubscribeId,
                SendDate = node.SendDate,

            };
        }
        #endregion


    }
}
