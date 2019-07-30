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
    public static class MeterGroupExtension
    {
        #region MeterGroup
        public static MeterGroupData ToViewData(this MeterGroup node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            //GroupBLL groupBLL = new GroupBLL();
            MeterBLL meterBLL = new MeterBLL();
            BuildingBLL buildingBLL = new BuildingBLL();
            if (node == null)
                return null;
            return new MeterGroupData()
            {
                Id = node.Id,
                GroupId = node.GroupId,
                MeterId = node.MeterId,
                BuildingId = node.BuildingId,
                Enable = node.Enable,
                //Group = (suffix & CategoryDictionary.Group) == CategoryDictionary.Group ? (node.Group == null ? groupBLL.Find(node.GroupId).ToViewData() : node.Group.ToViewData()) : null,
                Meter = (suffix & CategoryDictionary.Meter) == CategoryDictionary.Meter ? (node.Meter == null ? meterBLL.Find(node.MeterId).ToViewData() : node.Meter.ToViewData()) : null,
                Building = (suffix & CategoryDictionary.Building) == CategoryDictionary.Building ? (node.Building == null ? buildingBLL.Find(node.BuildingId).ToViewData() : node.Building.ToViewData()) : null,

            };
        }

        public static IList<MeterGroupData> ToViewList(this IQueryable<MeterGroup> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            //GroupBLL groupBLL = new GroupBLL();
            MeterBLL meterBLL = new MeterBLL();
            BuildingBLL buildingBLL = new BuildingBLL();
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new MeterGroupData()
            {
                Id = node.Id,
                GroupId = node.GroupId,
                MeterId = node.MeterId,
                BuildingId = node.BuildingId,
                Enable = node.Enable,
                //Group = (suffix & CategoryDictionary.Group) == CategoryDictionary.Group ? (node.Group == null ? groupBLL.Find(node.GroupId).ToViewData() : node.Group.ToViewData()) : null,
                Meter = (suffix & CategoryDictionary.Meter) == CategoryDictionary.Meter ? (node.Group == null ? meterBLL.Find(node.MeterId).ToViewData() : node.Meter.ToViewData()) : null,
                Building = (suffix & CategoryDictionary.Building) == CategoryDictionary.Building ? (node.Building == null ? buildingBLL.Find(node.BuildingId).ToViewData() : node.Building.ToViewData()) : null,

            }).ToList();
            return results;
        }

        public static MeterGroup ToModel(this MeterGroupData node)
        {
            return new MeterGroup()
            {
                Id = node.Id,
                GroupId = node.GroupId,
                MeterId = node.MeterId,
                BuildingId = node.BuildingId,
                Enable = node.Enable,

            };
        }
        #endregion

    }
}
