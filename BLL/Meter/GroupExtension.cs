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
    public static class GroupExtension
    {
        #region Group
        public static GroupData ToViewData(this Group node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            UserBLL userBLL = new UserBLL();
            if (node == null)
                return null;
            return new GroupData()
            {
                Id = node.Id,
                GroupTypeId = node.GroupTypeId,
                Name = node.Name,
                Description = node.Description,
                Enable = node.Enable,
                RegeneratorId = node.RegeneratorId,
                RegeneratorName = node.RegeneratorName,
                UpdatingTime = node.UpdatingTime,
                GroupTypeName = node.GroupType == null ? DictionaryCache.Get()[node.GroupTypeId].ChineseName : node.GroupType.ChineseName,
                Regenerator = (suffix & CategoryDictionary.User) == CategoryDictionary.User ? (node.Regenerator == null ? userBLL.Find(node.RegeneratorId).ToViewData() : node.Regenerator.ToViewData()) : null,


            };
        }

        public static IList<GroupData> ToViewList(this IQueryable<Group> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            UserBLL userBLL = new UserBLL();

            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new GroupData()
            {
                Id = node.Id,
                GroupTypeId = node.GroupTypeId,
                Name = node.Name,
                Description = node.Description,
                Enable = node.Enable,
                RegeneratorId = node.RegeneratorId,
                RegeneratorName = node.RegeneratorName,
                UpdatingTime = node.UpdatingTime,
                GroupTypeName = node.GroupType == null ? DictionaryCache.Get()[node.GroupTypeId].ChineseName : node.GroupType.ChineseName,
                Regenerator = (suffix & CategoryDictionary.User) == CategoryDictionary.User ? (node.Regenerator == null ? userBLL.Find(node.RegeneratorId).ToViewData() : node.Regenerator.ToViewData()) : null,

            }).ToList();
            return results;
        }

        public static Group ToModel(this GroupData node)
        {
            return new Group()
            {
                Id = node.Id,
                GroupTypeId = node.GroupTypeId,
                Name = node.Name,
                Description = node.Description,
                Enable = node.Enable,
                RegeneratorId = node.RegeneratorId,
                RegeneratorName = node.RegeneratorName,
                UpdatingTime = node.UpdatingTime,

            };
        }
        #endregion

    }
}
