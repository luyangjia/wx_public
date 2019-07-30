using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;

namespace WxPay2017.API.BLL
{
    public static class LogInfoExtension
    {
        #region LogInfo
        public static LogInfoData ToViewData(this LogInfo node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new LogInfoData()
            {
                Id = node.Id,
                MeterId = node.MeterId,
                GroupId = node.GroupId,
                ActionId = node.ActionId,
                UpdatingTime = node.UpdatingTime,
                IsSuccess = node.IsSuccess,
                OperatorId = node.OperatorId,
                OperatorName = node.OperatorName,
                ConfigName = node.ConfigName,

            };
        }

        public static IList<LogInfoData> ToViewList(this IQueryable<LogInfo> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new LogInfoData()
            {
                Id = node.Id,
                MeterId = node.MeterId,
                GroupId = node.GroupId,
                ActionId = node.ActionId,
                UpdatingTime = node.UpdatingTime,
                IsSuccess = node.IsSuccess,
                OperatorId = node.OperatorId,
                OperatorName = node.OperatorName,
                ConfigName = node.ConfigName,

            }).ToList();
            return results;
        }

        public static LogInfo ToModel(this LogInfoData node)
        {
            return new LogInfo()
            {
                Id = node.Id,
                MeterId = node.MeterId,
                GroupId = node.GroupId,
                ActionId = node.ActionId,
                UpdatingTime = node.UpdatingTime,
                IsSuccess = node.IsSuccess,
                OperatorId = node.OperatorId,
                OperatorName = node.OperatorName,
                ConfigName = node.ConfigName,

            };
        }
        #endregion

    }
}
