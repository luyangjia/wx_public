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
    public static class ConfigCycleSettingExtension
    {
        #region ConfigCycleSetting
        public static ConfigCycleSettingData ToViewData(this ConfigCycleSetting node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new ConfigCycleSettingData()
            {
                Id = node.Id,
                ConfigId = node.ConfigId,
                BeginTime = node.BeginTime,
                EndTime = node.EndTime,
                Description = node.Description,


            };
        }

        public static IList<ConfigCycleSettingData> ToViewList(this IQueryable<ConfigCycleSetting> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new ConfigCycleSettingData()
            {
                Id = node.Id,
                ConfigId = node.ConfigId,
                BeginTime = node.BeginTime,
                EndTime = node.EndTime,
                Description = node.Description,
            }).ToList();
            return results;
        }

        public static ConfigCycleSetting ToModel(this ConfigCycleSettingData node)
        {
            return new ConfigCycleSetting()
            {
                Id = node.Id,
                ConfigId = node.ConfigId,
                BeginTime = node.BeginTime,
                EndTime = node.EndTime,
                Description = node.Description,
            };
        }
        #endregion

    }
}
