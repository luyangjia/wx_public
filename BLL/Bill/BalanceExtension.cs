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
    public static class BalanceExtension
    {

        #region Balance
        public static BalanceData ToViewData(this Balance node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var model = new BalanceData()
            {
                Id = node.Id,
                TargetId = node.TargetId,
                TargetCategory = node.TargetCategory,
                EnergyCategory = node.EnergyCategory,
                Price = node.Price,
                EnergyConsumption = node.EnergyConsumption,
                Overplus = node.Overplus,
                Prepay = node.Prepay,
                Subsidy = node.Subsidy,
                Recharge = node.Recharge,
                CashCharge = node.CashCharge,
                CashCorrect = node.CashCorrect,
                Usage = node.Usage,
                Refund = node.Refund,
                BadDebt = node.BadDebt,
                Total = node.Total,
                AuditDate = node.AuditDate,
                CreateDate = node.CreateDate,
                OperatorId = node.OperatorId,
                TotalCashCharge=node.TotalCashCharge,
                TotalRecharge=node.TotalRecharge,
                TotalSubsidy=node.TotalSubsidy,
                BalanceDetails = node.BalanceDetails.ToList().Select(x => x.ToViewData()).ToList()
            };
            if ((suffix & CategoryDictionary.Manager) == CategoryDictionary.Manager)
            {
                var ctx_user = new UserBLL();
                var op = ctx_user.Find(model.OperatorId);
                if (op != null) model.Operator = op.ToViewData();
            }
            if ((suffix & CategoryDictionary.Building) == CategoryDictionary.Building || (suffix & CategoryDictionary.Organization) == CategoryDictionary.Organization)
            {
                if (model.TargetCategory == (int)CategoryDictionary.Building)
                {
                    var ctx_bid = new BuildingBLL();
                    var b = ctx_bid.Find(model.TargetId);
                    if (b != null) model.Target = b.ToViewData();

                }
                if (model.TargetCategory == (int)CategoryDictionary.Organization)
                {
                    var ctx_org = new OrganizationBLL();
                    var o = ctx_org.Find(model.TargetId);
                    if (o != null) model.Target = o.ToViewData();

                }
            }
            return model;
        }

        public static IEnumerable<BalanceData> ToViewList(this IQueryable<Balance> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return nodes.ToList().Select(x => x.ToViewData(suffix));
        }

        public static BalanceShortData ToShortViewData(this Balance node)
        {
            if (node == null)
                return null;
            var model = new BalanceShortData()
            {
                Id = node.Id,
                TargetId = node.TargetId,
                TargetCategory = node.TargetCategory,
                Total = node.Total,
            };

            return model;
        }
         
        public static IList<BalanceShortData> ToShortViewList(this IQueryable<Balance> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var result = nodes.ToList().Select(node => node.ToShortViewData()).ToList();
            var ids = result.Select(o => o.TargetId).ToList();
            if (nodes.Count() > 0)
            {
                Dictionary<long, string> nameDic = new Dictionary<long, string>();
                if (result[0].TargetCategory == (int)CategoryDictionary.Building)
                {
                    BuildingBLL buidingBLL = new BuildingBLL();
                    var list = buidingBLL.Filter(o => ids.Contains(o.Id)).Select(o => new
                    {
                        Id = o.Id,
                        Name = o.Name
                    }).ToList();
                    foreach (var l in list)
                        nameDic.Add(l.Id, l.Name);
                }
                else if (result[0].TargetCategory == (int)CategoryDictionary.Organization)
                {
                    OrganizationBLL orgBLL = new OrganizationBLL();
                    var list = orgBLL.Filter(o => ids.Contains(o.Id)).Select(o => new
                    {
                        Id = o.Id,
                        Name = o.Name
                    }).ToList();
                    foreach (var l in list)
                        nameDic.Add(l.Id, l.Name);
                }
                foreach (var r in result)
                {
                    r.Name = nameDic[r.TargetId];
                }
            }
            return result;
        }

        public static Balance ToModel(this BalanceData node)
        {
            var model = new Balance()
            {
                Id = node.Id,
                TargetId = node.TargetId,
                TargetCategory = node.TargetCategory,
                EnergyCategory = node.EnergyCategory,
                Price = node.Price,
                EnergyConsumption = node.EnergyConsumption,
                Overplus = node.Overplus,
                Prepay = node.Prepay,
                Subsidy = node.Subsidy,
                Recharge = node.Recharge,
                CashCharge = node.CashCharge,
                CashCorrect = node.CashCorrect,
                Usage = node.Usage,
                Refund = node.Refund,
                BadDebt = node.BadDebt,
                Total = node.Total,
                AuditDate = node.AuditDate,
                CreateDate = node.CreateDate,
                TotalCashCharge = node.TotalCashCharge,
                TotalRecharge = node.TotalRecharge,
                TotalSubsidy = node.TotalSubsidy,
                OperatorId = node.OperatorId
            };
            if (node.BalanceDetails.Count > 0)
            {
                model.BalanceDetails = node.BalanceDetails.Select(x => x.ToModel()).ToList();
            }
            return model;
        }
        #endregion


        public static Balance Balance(this Building node)
        {
            var ctx = new BalanceBLL();
            var model = ctx.Filter(x => x.TargetId == node.Id && x.TargetCategory == (int)CategoryDictionary.Building).OrderByDescending(x => x.AuditDate).FirstOrDefault();
            return model;
        }


        public static Balance Balance(this Organization node)
        {
            var ctx = new BalanceBLL();
            var model = ctx.Filter(x => x.TargetId == node.Id && x.TargetCategory == (int)CategoryDictionary.Organization).OrderByDescending(x => x.AuditDate).FirstOrDefault();
            return model;
        }
    }
}
