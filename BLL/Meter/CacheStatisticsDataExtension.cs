using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO.Data;

namespace WxPay2017.API.BLL
{
    public static class CacheStatisticsDataExtension
    {
        #region CacheStatisticsData
        public static CacheStatisticsDataData ToViewData(this CacheStatisticsData node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new CacheStatisticsDataData()
            {
                Id = node.Id,
                TargetId = node.TargetId,
                StatMode = node.StatMode,
                EnergyCategoryId = node.EnergyCategoryId,
                ParameterTypeId = node.ParameterTypeId,
                TimeUnit = node.TimeUnit,
                StartTime = node.StartTime,
                FinishTime = node.FinishTime,
                Value = node.Value,

            };
        }

        public static IList<CacheStatisticsDataData> ToViewList(this IQueryable<CacheStatisticsData> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new CacheStatisticsDataData()
            {
                Id = node.Id,
                TargetId = node.TargetId,
                StatMode = node.StatMode,
                EnergyCategoryId = node.EnergyCategoryId,
                ParameterTypeId = node.ParameterTypeId,
                TimeUnit = node.TimeUnit,
                StartTime = node.StartTime,
                FinishTime = node.FinishTime,
                Value = node.Value,

            }).ToList();
            return results;
        }

        public static CacheStatisticsData ToModel(this CacheStatisticsDataData node)
        {
            return new CacheStatisticsData()
            {
                Id = node.Id,
                TargetId = node.TargetId,
                StatMode = node.StatMode,
                EnergyCategoryId = node.EnergyCategoryId,
                ParameterTypeId = node.ParameterTypeId,
                TimeUnit = node.TimeUnit,
                StartTime = node.StartTime,
                FinishTime = node.FinishTime,
                Value = node.Value,

            };
        }
        #endregion

    }
}
