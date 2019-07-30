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
    public static class SceneModeMeterExtension
    {
        #region SceneModeMeter
        public static SceneModeMeterData ToViewData(this SceneModeMeter node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new SceneModeMeterData()
            {
                Id = node.Id,
                SceneModeId = node.SceneModeId,
                GroupId = node.GroupId,
                BuildingId = node.BuildingId,
                MeterId = node.MeterId,
                SettingTime = node.SettingTime,
                OperatorId = node.OperatorId,
                OperatorName = node.OperatorName,

            };
        }

        public static IList<SceneModeMeterData> ToViewList(this IQueryable<SceneModeMeter> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new SceneModeMeterData()
            {
                Id = node.Id,
                SceneModeId = node.SceneModeId,
                GroupId = node.GroupId,
                BuildingId = node.BuildingId,
                MeterId = node.MeterId,
                SettingTime = node.SettingTime,
                OperatorId = node.OperatorId,
                OperatorName = node.OperatorName,

            }).ToList();
            return results;
        }

        public static SceneModeMeter ToModel(this SceneModeMeterData node)
        {
            return new SceneModeMeter()
            {
                Id = node.Id,
                SceneModeId = node.SceneModeId,
                GroupId = node.GroupId,
                BuildingId = node.BuildingId,
                MeterId = node.MeterId,
                SettingTime = node.SettingTime,
                OperatorId = node.OperatorId,
                OperatorName = node.OperatorName,

            };
        }
        #endregion

    }
}
