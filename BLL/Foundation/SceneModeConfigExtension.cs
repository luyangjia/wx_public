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
    public static class SceneModeConfigExtension
    {
        #region SceneModeConfig
        public static SceneModeConfigData ToViewData(this SceneModeConfig node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new SceneModeConfigData()
            {
                Id = node.Id,
                SceneModeId = node.SceneModeId,
                ConfigId = node.ConfigId,

            };
        }

        public static IList<SceneModeConfigData> ToViewList(this IQueryable<SceneModeConfig> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new SceneModeConfigData()
            {
                Id = node.Id,
                SceneModeId = node.SceneModeId,
                ConfigId = node.ConfigId,

            }).ToList();
            return results;
        }

        public static SceneModeConfig ToModel(this SceneModeConfigData node)
        {
            return new SceneModeConfig()
            {
                Id = node.Id,
                SceneModeId = node.SceneModeId,
                ConfigId = node.ConfigId,

            };
        }
        #endregion

    }
}
