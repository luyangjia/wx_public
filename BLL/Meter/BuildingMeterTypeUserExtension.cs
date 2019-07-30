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
    public static class BuildingMeterTypeUserExtension
    {
        #region BuildingMeterTypeUser
        public static BuildingMeterTypeUserData ToViewData(this BuildingMeterTypeUser node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            UserBLL userBLL = new UserBLL();
            BuildingBLL buildingBLL = new BuildingBLL();
            if (node == null)
                return null;
            return new BuildingMeterTypeUserData()
            {
                Id = node.Id,
                UserId = node.UserId,
                MeterTypeId = node.MeterTypeId,
                BuildingId = node.BuildingId,
                Enable = node.Enable,
                User = (suffix & CategoryDictionary.User) == CategoryDictionary.User ? (node.User == null ? userBLL.Find(node.UserId).ToViewData() : node.User.ToViewData()) : null,
                MeterTypeName = node.MeterType == null ? DictionaryCache.Get()[node.MeterTypeId].ChineseName : node.MeterType.ChineseName,
                //MeterType = node.MeterType == null ? DictionaryCache.Get()[node.MeterTypeId].ToViewData() : node.MeterType.ToViewData(),
                Building = (suffix & CategoryDictionary.Building) == CategoryDictionary.Building ? (node.Building == null ? buildingBLL.Find(node.BuildingId).ToViewData() : node.Building.ToViewData()) : null
            };
        }

        public static IList<BuildingMeterTypeUserData> ToViewList(this IQueryable<BuildingMeterTypeUser> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            UserBLL userBLL = new UserBLL();
            BuildingBLL buildingBLL = new BuildingBLL();
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new BuildingMeterTypeUserData()
            {
                Id = node.Id,
                UserId = node.UserId,
                MeterTypeId = node.MeterTypeId,
                BuildingId = node.BuildingId,
                Enable = node.Enable,
                User = (suffix & CategoryDictionary.User) == CategoryDictionary.User ? (node.User == null ? userBLL.Find(node.UserId).ToViewData() : node.User.ToViewData()) : null,
                MeterTypeName = node.MeterType == null ? DictionaryCache.Get()[node.MeterTypeId].ChineseName : node.MeterType.ChineseName,
                Building = (suffix & CategoryDictionary.Building) == CategoryDictionary.Building ? (node.Building == null ? buildingBLL.Find(node.BuildingId).ToViewData() : node.Building.ToViewData()) : null

            }).ToList();
            return results;
        }

        public static BuildingMeterTypeUser ToModel(this BuildingMeterTypeUserData node)
        {
            return new BuildingMeterTypeUser()
            {
                Id = node.Id,
                UserId = node.UserId,
                MeterTypeId = node.MeterTypeId,
                BuildingId = node.BuildingId,
                Enable = node.Enable,

            };
        }
        #endregion

    }
}
