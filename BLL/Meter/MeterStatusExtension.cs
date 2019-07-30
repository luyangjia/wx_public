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
    public static class MeterStatusExtension
    {
        #region MeterStatus
        public static MeterStatusData ToViewData(this MeterStatus node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new MeterStatusData()
            {
                Id = node.Id,
                MeterId = node.MeterId,
                MeterMessageTypeId = node.MeterMessageTypeId,
                Enabled = node.Enabled,
                Value = node.Value,
                UpdateTime = node.UpdateTime,
                IsFluctuationData = node.IsFluctuationData,
                MonitoringConfigId = node.MonitoringConfigId,
                Description=node.Description,
                Meter = ((suffix & CategoryDictionary.Meter) == CategoryDictionary.Meter?node.Meter.ToViewData():null),
                MeterMessageType = node.MeterMessageType == null ? DictionaryCache.Get()[node.MeterMessageTypeId].ToViewData() : node.MeterMessageType.ToViewData(),
            };
           
        }

        public static IList<MeterStatusData> ToViewList(this IQueryable<MeterStatus> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new MeterStatusData()
            {
                Id = node.Id,
                MeterId = node.MeterId,
                MeterMessageTypeId = node.MeterMessageTypeId,
                Enabled = node.Enabled,
                Value = node.Value,
                UpdateTime = node.UpdateTime,
                IsFluctuationData = node.IsFluctuationData,
                Description = node.Description,
                Meter = ((suffix & CategoryDictionary.Meter) == CategoryDictionary.Meter ? node.Meter.ToViewData() : null),
                MonitoringConfigId = node.MonitoringConfigId,
               
            }).ToList();

            for (int i = 0; i < nodeList.Count(); i++)
             {
                 results[i].MeterMessageType = DictionaryCache.Get()[results[i].MeterMessageTypeId].ToViewData();
             }
             return results;
        }

        public static MeterStatus ToModel(this MeterStatusData node)
        {
            return new MeterStatus()
            {
                Id = node.Id,
                MeterId = node.MeterId,
                MeterMessageTypeId = node.MeterMessageTypeId,
                Enabled = node.Enabled,
                Value = node.Value,
                UpdateTime = node.UpdateTime,
                Description = node.Description,
                IsFluctuationData = node.IsFluctuationData,
                MonitoringConfigId = node.MonitoringConfigId,

            };
        }
        #endregion

    }
}
