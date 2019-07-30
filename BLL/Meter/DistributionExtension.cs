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
    public static class DistributionExtension
    {
        #region Distribution
        public static DistributionData ToViewData(this Distribution node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new DistributionData()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                SubstationName = node.SubstationName,
                TransformerName = node.TransformerName,
                TransformerType = node.TransformerType,
                TransformerCapacity = node.TransformerCapacity,
                DistributionAddress = node.DistributionAddress,
                Coordinate3dId = node.Coordinate3dId,
                IsSwitchingStation=node.IsSwitchingStation,

            };
        }

        public static IList<DistributionData> ToViewList(this IQueryable<Distribution> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new DistributionData()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                SubstationName = node.SubstationName,
                TransformerName = node.TransformerName,
                TransformerType = node.TransformerType,
                TransformerCapacity = node.TransformerCapacity,
                DistributionAddress = node.DistributionAddress,
                Coordinate3dId = node.Coordinate3dId,
                IsSwitchingStation = node.IsSwitchingStation,

            }).ToList();
            return results;
        }

        public static Distribution ToModel(this DistributionData node)
        {
            return new Distribution()
            {
                Id = node.Id,
                ParentId = node.ParentId,
                SubstationName = node.SubstationName,
                TransformerName = node.TransformerName,
                TransformerType = node.TransformerType,
                TransformerCapacity = node.TransformerCapacity,
                DistributionAddress = node.DistributionAddress,
                Coordinate3dId = node.Coordinate3dId,
                IsSwitchingStation = node.IsSwitchingStation,

            };
        }
        #endregion

    }
}
