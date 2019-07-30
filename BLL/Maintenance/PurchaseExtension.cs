using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO.Param;
using WxPay2017.API.VO;
using WxPay2017.API.DAL.EmpModels;

namespace WxPay2017.API.BLL 
{
    public static class PurchaseExtension
    {
        #region Purchase
        public static PurchaseData ToViewData(this Purchase node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            UserBLL userBLL = new UserBLL();
            MaintenanceBLL maintenanceBLL = new MaintenanceBLL();
            if (node == null)
                return null;
            return new PurchaseData()
            {
                Id = node.Id,
                MaintenanceId = node.MaintenanceId,
                CurrentOperatorId = node.CurrentOperatorId,
                ApproverId = node.ApproverId,
                MaterialName = node.MaterialName,
                MaterialNum = node.MaterialNum,
                MaterialPrice = node.MaterialPrice,
                Description = node.Description,
                IsAdopt = node.IsAdopt,
                CreateDate = node.CreateDate,
                ApplyDate = node.ApplyDate,
                Approver = node.ApproverId != null ? node.Approver == null ? userBLL.Find(node.ApproverId).ToViewData() : node.Approver.ToViewData() : null,
                CurrentOperator = node.CurrentOperatorId != null ? node.CurrentOperator == null ? userBLL.Find(node.CurrentOperatorId).ToViewData() : node.CurrentOperator.ToViewData() : null,
                //Maintenance = node.MaintenanceId != null ? node.Maintenance == null ? maintenanceBLL.Find(node.MaintenanceId).ToViewData() : node.Maintenance.ToViewData() : null,
            };
        }

        public static IList<PurchaseData> ToViewList(this IQueryable<Purchase> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            UserBLL userBLL = new UserBLL();
            MaintenanceBLL maintenanceBLL = new MaintenanceBLL();
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new PurchaseData()
            {
                Id = node.Id,
                MaintenanceId = node.MaintenanceId,
                CurrentOperatorId = node.CurrentOperatorId,
                ApproverId = node.ApproverId,
                MaterialName = node.MaterialName,
                MaterialNum = node.MaterialNum,
                MaterialPrice = node.MaterialPrice,
                Description = node.Description,
                IsAdopt = node.IsAdopt,
                CreateDate = node.CreateDate,
                ApplyDate = node.ApplyDate,
                Approver = node.ApproverId != null ? node.Approver == null ? userBLL.Find(node.ApproverId).ToViewData() : node.Approver.ToViewData() : null,
                CurrentOperator = node.CurrentOperatorId != null ? node.CurrentOperator == null ? userBLL.Find(node.CurrentOperatorId).ToViewData() : node.CurrentOperator.ToViewData() : null,
                //Maintenance = node.MaintenanceId != null ? node.Maintenance == null ? maintenanceBLL.Find(node.MaintenanceId).ToViewData() : node.Maintenance.ToViewData() : null,

            }).ToList();
            return results;
        }

        public static Purchase ToModel(this PurchaseData node)
        {
            return new Purchase()
            {
                Id = node.Id,
                MaintenanceId = node.MaintenanceId,
                CurrentOperatorId = node.CurrentOperatorId,
                ApproverId = node.ApproverId,
                MaterialName = node.MaterialName,
                MaterialNum = node.MaterialNum,
                MaterialPrice = node.MaterialPrice,
                Description = node.Description,
                IsAdopt = node.IsAdopt,
                CreateDate = node.CreateDate,
                ApplyDate = node.ApplyDate,


            };
        }
        #endregion

    }
}
