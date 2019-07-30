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
    public static class GatewayExtension
    {
        #region Gateway
        public static GatewayData ToViewData(this Gateway node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new GatewayData()
            {
                Id = node.Id,
                GatewayNo = node.GatewayNo,
                IPAddress = node.IPAddress,
                EnablePort = node.EnablePort,
                PortRule = node.PortRule,
                SpeedRate = node.SpeedRate,
                Address = node.Address,
                NetworkingArea = node.NetworkingArea,
                //BrandId = node.BrandId,
                Date = node.Date,
                Description = node.Description,

            };
        }

        public static IList<GatewayData> ToViewList(this IQueryable<Gateway> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new GatewayData()
            {
                Id = node.Id,
                GatewayNo = node.GatewayNo,
                IPAddress = node.IPAddress,
                EnablePort = node.EnablePort,
                PortRule = node.PortRule,
                SpeedRate = node.SpeedRate,
                Address = node.Address,
                NetworkingArea = node.NetworkingArea,
                //BrandId = node.BrandId,
                Date = node.Date,
                Description = node.Description,

            }).ToList();
            return results;
        }

        public static Gateway ToModel(this GatewayData node)
        {
            return new Gateway()
            {
                Id = node.Id,
                GatewayNo = node.GatewayNo,
                IPAddress = node.IPAddress,
                EnablePort = node.EnablePort,
                PortRule = node.PortRule,
                SpeedRate = node.SpeedRate,
                Address = node.Address,
                NetworkingArea = node.NetworkingArea,
                //BrandId = node.BrandId,
                Date = node.Date,
                Description = node.Description,

            };
        }
        #endregion

    }
}
