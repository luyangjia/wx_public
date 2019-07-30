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
    public static class SceneModeExtension
    {
        #region SceneMode
        public static SceneModeData ToViewData(this SceneMode node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new SceneModeData()
            {
                Id = node.Id,
                ConfigTypeId = node.ConfigTypeId,
                Name = node.Name,
                Description = node.Description,

            };
        }

        public static IList<SceneModeData> ToViewList(this IQueryable<SceneMode> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new SceneModeData()
            {
                Id = node.Id,
                ConfigTypeId = node.ConfigTypeId,
                Name = node.Name,
                Description = node.Description,

            }).ToList();
            return results;
        }

        public static SceneMode ToModel(this SceneModeData node)
        {
            return new SceneMode()
            {
                Id = node.Id,
                ConfigTypeId = node.ConfigTypeId,
                Name = node.Name,
                Description = node.Description,

            };
        }
        #endregion

    }
}
