using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO.Data;

namespace WxPay2017.API.BLL
{
    public static class PumpRoomExtension
    {
        #region PumpRoom
        public static PumpRoomData ToViewData(this PumpRoom node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new PumpRoomData()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                PumpName = node.PumpName,
                PumpAddress = node.PumpAddress,
                Coordinate3dId = node.Coordinate3dId,

            };
        }

        public static IList<PumpRoomData> ToViewList(this IQueryable<PumpRoom> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new PumpRoomData()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                PumpName = node.PumpName,
                PumpAddress = node.PumpAddress,
                Coordinate3dId = node.Coordinate3dId,

            }).ToList();
            return results;
        }

        public static PumpRoom ToModel(this PumpRoomData node)
        {
            return new PumpRoom()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                PumpName = node.PumpName,
                PumpAddress = node.PumpAddress,
                Coordinate3dId = node.Coordinate3dId,

            };
        }
        #endregion

    }
}
