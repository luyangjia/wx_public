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
    public static class RequestForOvertimeExtension
    {
        #region RequestForOvertime
        public static RequestForOvertimeData ToViewData(this RequestForOvertime node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new RequestForOvertimeData()
            {
                Id = node.Id,
                ApplicantId = node.ApplicantId,
                BuildingId = node.BuildingId,
                BeginTime = node.BeginTime,
                EndTime = node.EndTime,
                BeginDate = node.BeginDate,
                EndDate = node.EndDate,
                Reason = node.Reason,
                ApproverId = node.ApproverId,
                ApproverDesc = node.ApproverDesc,
                IsOk = node.IsOk,
                CreateTime = node.CreateTime,
                CheckTIme = node.CheckTIme,

            };
        }

        public static IList<RequestForOvertimeData> ToViewList(this IQueryable<RequestForOvertime> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new RequestForOvertimeData()
            {
                Id = node.Id,
                ApplicantId = node.ApplicantId,
                BuildingId = node.BuildingId,
                BeginTime = node.BeginTime,
                EndTime = node.EndTime,
                BeginDate = node.BeginDate,
                EndDate = node.EndDate,
                Reason = node.Reason,
                ApproverId = node.ApproverId,
                ApproverDesc = node.ApproverDesc,
                IsOk = node.IsOk,
                CreateTime = node.CreateTime,
                CheckTIme = node.CheckTIme,

            }).ToList();
            return results;
        }

        public static RequestForOvertime ToModel(this RequestForOvertimeData node)
        {
            return new RequestForOvertime()
            {
                Id = node.Id,
                ApplicantId = node.ApplicantId,
                BuildingId = node.BuildingId,
                BeginTime = node.BeginTime,
                EndTime = node.EndTime,
                BeginDate = node.BeginDate,
                EndDate = node.EndDate,
                Reason = node.Reason,
                ApproverId = node.ApproverId,
                ApproverDesc = node.ApproverDesc,
                IsOk = node.IsOk,
                CreateTime = node.CreateTime,
                CheckTIme = node.CheckTIme,

            };
        }
        #endregion

    }
}
