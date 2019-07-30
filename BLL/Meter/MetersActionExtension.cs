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
    public static class MetersActionExtension
    {

        #region MetersAction
        public static MetersActionData ToViewData(this MetersAction node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            if (node.Meter == null)
            {
                MeterBLL meterBLL = new MeterBLL();
                node.Meter = meterBLL.Find(node.MeterId);
            }
            return new MetersActionData()
            {
                Id = node.Id,
                MeterId = node.MeterId,
                IsOk = node.IsOk,
                IsPowerOffByMoney = node.IsPowerOffByMoney,
                IsPowerOffByTime = node.IsPowerOffByTime,
                ActionId = node.ActionId,
                AddTime = node.AddTime,
                ActionTime = node.ActionTime,
                AnswerTIme = node.AnswerTIme,
                AnswerValue = node.AnswerValue,
                SendTimes = node.SendTimes,
                Priority = node.Priority,
                GroupNum = node.GroupNum,
                ParentId = node.ParentId,
                Description = node.Description,
                SettingValue = node.SettingValue,
                MeterName = node.Meter == null ? null : node.Meter.Name,  //Meter对象为空时报错(新增对象后,返回MetersAction对象时会出现，故加上判断Meter对象是否为空)
                MeterAddress = node.Meter == null ? null : node.Meter.Address,
                MeterMacAddress = node.Meter == null ? null : node.Meter.MacAddress,
                CommandStatus = node.Meter == null ? null : node.CommandStatus,
                InBuildingName = (node.Meter == null) ? null : (node.Meter.Building == null ? null : node.Meter.Building.Name),
                MeterTypeName = (node.Meter == null  ? null : node.Meter.TypeDict.ChineseName),
                CommandStatusName = (node.CommandStatusDic != null ? node.CommandStatusDic.ChineseName : null),
                //ActionName = (node.Action==null?null:node.Action.ChineseName)

                ActionName = (node.ActionId != DictionaryCache.ActionBalanceChange.Id ? DictionaryCache.Get()[node.ActionId].ChineseName : (node.SettingValue.StartsWith("-") ? "能耗消费(或扣费)" + node.SettingValue + "元" : "购电" + node.SettingValue + "元，")) + (node.Meter.Building != null ? node.Meter.Building.Name : ""),
            };

        }

        public static IEnumerable<MetersActionData> ToViewList(this IQueryable<MetersAction> node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return node.ToList().Select(x => x.ToViewData(suffix));
        }

        public static MetersAction ToModel(this MetersActionData node)
        {
            return new MetersAction()
            {
                Id = node.Id,
                MeterId = node.MeterId,
                IsOk = node.IsOk,
                IsPowerOffByMoney = node.IsPowerOffByMoney,
                IsPowerOffByTime = node.IsPowerOffByTime,
                ActionId = node.ActionId,
                AddTime = node.AddTime,
                ActionTime = node.ActionTime,
                AnswerTIme = node.AnswerTIme,
                AnswerValue = node.AnswerValue,
                SendTimes = node.SendTimes,
                Priority = node.Priority,
                GroupNum = node.GroupNum,
                ParentId = node.ParentId,
                Description = node.Description,
                SettingValue = node.SettingValue,
                CommandStatus = node.CommandStatus,


            };
        }
        #endregion

    }

}
