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
    public static class StatisticsSubscribeExtension
    {
        #region StatisticsSubscribe
        public static StatisticsSubscribeData ToViewData(this StatisticsSubscribe node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new StatisticsSubscribeData()
            {
                Id = node.Id,
                CategoryDictionary = node.CategoryDictionary,
                TargetId = node.TargetId,
                EnergyCategoryId = node.EnergyCategoryId,
                Emails = node.Emails,
                SubDate = node.SubDate,
                //IsPeriodMonth = node.IsPeriodMonth,
                From = node.From,
                To = node.To,
                Period = node.Period
            };
        }

        public static IList<StatisticsSubscribeData> ToViewList(this IQueryable<StatisticsSubscribe> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new StatisticsSubscribeData()
            {
                Id = node.Id,
                CategoryDictionary = node.CategoryDictionary,
                TargetId = node.TargetId,
                EnergyCategoryId = node.EnergyCategoryId,
                Emails = node.Emails,
                SubDate = node.SubDate,
                //IsPeriodMonth = node.IsPeriodMonth,
                From = node.From,
                To = node.To,
                Period = node.Period
            }).ToList();
            return results;
        }

        public static StatisticsSubscribe ToModel(this StatisticsSubscribeData node)
        {
            return new StatisticsSubscribe()
            {
                Id = node.Id, 
                CategoryDictionary = node.CategoryDictionary,
                TargetId = node.TargetId,
                EnergyCategoryId = node.EnergyCategoryId,
                Emails = node.Emails,
                SubDate = node.SubDate,
                //IsPeriodMonth = node.IsPeriodMonth,
                From = node.From,
                To = node.To,
                Period = node.Period
            };
        }
        #endregion


    }
}
