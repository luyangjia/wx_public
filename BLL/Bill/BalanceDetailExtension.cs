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
    public static class BalanceDetailExtension
    {

        #region BalanceDetail
        public static BalanceDetailData ToViewData(this BalanceDetail node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var model = new BalanceDetailData()
            {
                Id = node.Id,
                BalanceId = node.BalanceId,
                EnergyCategory = node.EnergyCategory,
                Amount = node.Amount,
                Consumption = node.Consumption,
                Price = node.Price,
                EnergyCategoryDict = node.EnergyCategoryDict.ToViewData(),
                ConsumptionTotal = node.ConsumptionTotal,
                AmountTotal = node.AmountTotal
            };
            return model;
        }


        public static IList<BalanceDetailData> ToViewList(this IQueryable<BalanceDetail> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var result = nodes.ToList().Select(node => node.ToViewData(suffix)).ToList();
            return result;
        }

        public static BalanceDetail ToModel(this BalanceDetailData node)
        {
            return new BalanceDetail()
            {
                Id = node.Id,
                BalanceId = node.BalanceId,
                EnergyCategory = node.EnergyCategory,
                Amount = node.Amount,
                Consumption = node.Consumption,
                Price = node.Price,
                ConsumptionTotal = node.ConsumptionTotal,
                AmountTotal = node.AmountTotal
            };
        }
        #endregion
    }
}
