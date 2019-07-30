using WxPay2017.API.VO.Common;
using WxPay2017.API.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL 
{
    public static class UserExtensionExtension
    {
        #region UserExtension
        public static UserExtensionData ToViewData(this WxPay2017.API.DAL.EmpModels.UserExtension node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new UserExtensionData()
            {
                Id = node.Id,
                UserId = node.UserId,
                ColumnTypeId = node.ColumnTypeId,
                ValueStr = node.ValueStr,
                Value = node.Value,

            };
        }

        public static IList<UserExtensionData> ToViewList(this IQueryable<WxPay2017.API.DAL.EmpModels.UserExtension> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new UserExtensionData()
            {
                Id = node.Id,
                UserId = node.UserId,
                ColumnTypeId = node.ColumnTypeId,
                ValueStr = node.ValueStr,
                Value = node.Value,

            }).ToList();
            return results;
        }

        public static WxPay2017.API.DAL.EmpModels.UserExtension ToModel(this UserExtensionData node)
        {
            return new WxPay2017.API.DAL.EmpModels.UserExtension()
            {
                Id = node.Id,
                UserId = node.UserId,
                ColumnTypeId = node.ColumnTypeId,
                ValueStr = node.ValueStr,
                Value = node.Value,

            };
        }
        #endregion

    }
}
