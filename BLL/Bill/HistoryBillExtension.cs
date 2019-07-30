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
    public static  class HistoryBillExtension
    {
        #region HistoryBill
        public static HistoryBillData ToViewData(this HistoryBill node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            UserBLL userBLL = new UserBLL();
            return new HistoryBillData()
            {
                Id = node.Id,
                ReceiverId = node.ReceiverId,
                PayerId = node.PayerId,
                BeginTime = node.BeginTime,
                EndTime = node.EndTime,
                Value = node.Value,
                UsedValue=node.UsedValue,
                UsedEnergyValue=node.UsedEnergyValue,
                BillTypeId = node.BillTypeId,
                PayTypeId = node.PayTypeId,
                OperatorId = node.OperatorId,
                IsPay = node.IsPay,
                IsSynchro=node.IsSynchro,
                subject = node.subject,
                Body = node.Body,
                PayMentTime = node.PayMentTime,
                CreateTime = node.CreateTime,
                IsZero = node.IsZero,
                ZeroTime = node.ZeroTime,
                PayMethodId = node.PayMethodId,
                TransNumber = node.TransNumber,
                EnergyCategoryId = node.EnergyCategoryId,
                //------以下注释代码，Delete时报错--------
                //BillTypeName = node.BillType == null ? DictionaryCache.Get()[node.BillTypeId].ChineseName : node.BillType.ChineseName,
                //PayTypeName = node.PayType == null ? DictionaryCache.Get()[(int)node.PayTypeId].ChineseName : node.PayType.ChineseName,
                //PayMethodName = node.PayMethod == null ?node.PayMethodId==null?null: DictionaryCache.Get()[(int)node.PayMethodId].ChineseName : node.PayMethod.ChineseName,
                Receiver = (suffix & CategoryDictionary.User) == CategoryDictionary.User ?(node.Receiver==null?userBLL.Find(node.ReceiverId).ToViewData():node.Receiver.ToViewData()) : null,
                Payer = (suffix & CategoryDictionary.User) == CategoryDictionary.User ?(node.Payer==null?userBLL.Find(node.PayerId).ToViewData():node.Payer.ToViewData()) : null,
                Operator = (suffix & CategoryDictionary.User) == CategoryDictionary.User ? (node.Operator == null ? userBLL.Find(node.OperatorId).ToViewData() : node.Operator.ToViewData()) : null
            };
        }

        public static IList<HistoryBillData> ToViewList(this IQueryable<HistoryBill> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            UserBLL userBLL = new UserBLL();
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new HistoryBillData()
            {
                Id = node.Id,
                ReceiverId = node.ReceiverId,
                PayerId = node.PayerId,
                BeginTime = node.BeginTime,
                EndTime = node.EndTime,
                Value = node.Value,
                IsSynchro = node.IsSynchro,
                BillTypeId = node.BillTypeId,
                UsedValue = node.UsedValue,
                UsedEnergyValue = node.UsedEnergyValue,
                PayTypeId = node.PayTypeId,
                IsPay = node.IsPay,
                subject = node.subject,
                Body = node.Body,
                OperatorId = node.OperatorId,
                PayMentTime = node.PayMentTime,
                CreateTime = node.CreateTime,
                IsZero = node.IsZero,
                ZeroTime = node.ZeroTime,
                PayMethodId = node.PayMethodId,
                TransNumber = node.TransNumber,
                EnergyCategoryId = node.EnergyCategoryId,
                BillTypeName = node.BillType == null ? DictionaryCache.Get()[node.BillTypeId].ChineseName : node.BillType.ChineseName,
                PayTypeName = node.PayType == null ? DictionaryCache.Get()[(int)node.PayTypeId].ChineseName : node.PayType.ChineseName,
                PayMethodName = node.PayMethodId != null ? DictionaryCache.Get()[(int)node.PayMethodId].ChineseName : null,
                Receiver = (node.ReceiverId != null &&(suffix & CategoryDictionary.User) == CategoryDictionary.User)? (node.Receiver == null ? userBLL.Find(node.ReceiverId).ToViewData(CategoryDictionary.Building | CategoryDictionary.Org) : node.Receiver.ToViewData(CategoryDictionary.Building | CategoryDictionary.Org)) : null,
                Payer = (node.PayerId != null &&(suffix & CategoryDictionary.User) == CategoryDictionary.User) ? (node.Payer == null ? userBLL.Find(node.PayerId).ToViewData() : node.Payer.ToViewData()) : null,
                Operator = (node.OperatorId != null && (suffix & CategoryDictionary.User) == CategoryDictionary.User) ? (node.Operator == null ? userBLL.Find(node.OperatorId).ToViewData() : node.Operator.ToViewData()) : null,
            }).ToList();
            return results;
        }

        public static IList<HistoryBillData> ToShortViewList(this IQueryable<HistoryBill> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            UserBLL userBLL = new UserBLL();
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new HistoryBillData()
            {
                Id = node.Id,
                ReceiverId = node.ReceiverId,
                PayerId = node.PayerId,
                BeginTime = node.BeginTime,
                EndTime = node.EndTime,
                Value = node.Value,
                BillTypeId = node.BillTypeId,
                IsSynchro = node.IsSynchro,
                UsedValue = node.UsedValue,
                OperatorId = node.OperatorId,
                UsedEnergyValue = node.UsedEnergyValue,
                PayTypeId = node.PayTypeId,
                IsPay = node.IsPay,
                subject = node.subject,
                Body = node.Body,
                PayMentTime = node.PayMentTime,
                CreateTime = node.CreateTime,
                IsZero = node.IsZero,
                ZeroTime = node.ZeroTime,
                PayMethodId = node.PayMethodId,
                TransNumber = node.TransNumber,
                EnergyCategoryId = node.EnergyCategoryId,
                BillTypeName = node.BillType == null ? DictionaryCache.Get()[node.BillTypeId].ChineseName : node.BillType.ChineseName,
                PayTypeName = node.PayType == null ? DictionaryCache.Get()[(int)node.PayTypeId].ChineseName : node.PayType.ChineseName,

            }).ToList();
            return results;
        }

        public static HistoryBill ToModel(this HistoryBillData node)
        {
            return new HistoryBill()
            {
                Id = node.Id,
                ReceiverId = node.ReceiverId,
                PayerId = node.PayerId,
                BeginTime = node.BeginTime,
                IsSynchro = node.IsSynchro,
                EndTime = node.EndTime,
                Value = node.Value,
                BillTypeId = node.BillTypeId,
                PayTypeId = node.PayTypeId,
                UsedEnergyValue = node.UsedEnergyValue,
                IsPay = node.IsPay,
                UsedValue = node.UsedValue,
                subject = node.subject,
                OperatorId = node.OperatorId,
                Body = node.Body,
                PayMentTime = node.PayMentTime,
                CreateTime = node.CreateTime,
                IsZero = node.IsZero,
                ZeroTime = node.ZeroTime,
                PayMethodId = node.PayMethodId,
                TransNumber = node.TransNumber,
                EnergyCategoryId = node.EnergyCategoryId,

            };
        }
        #endregion

    }
}
