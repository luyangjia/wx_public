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
    public static class ProvinceExtension
    {
        #region Province
        public static ProvinceData ToViewData(this Province node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var model = new ProvinceData()
            {
                Id = node.Id,
                Name = node.Name,
                Sort = node.Sort,
                Remark = node.Remark,
                Buildings = (suffix & CategoryDictionary.Building) == CategoryDictionary.Building ? node.Buildings.Select(x => x.ToViewData()).ToList() : null,
                Organizations = (suffix & CategoryDictionary.Organization) == CategoryDictionary.Organization ? node.Organizations.Select(x => x.ToViewData()).ToList() : null,
                Cities = (suffix & CategoryDictionary.City) == CategoryDictionary.City ? node.Cities.Select(x => x.ToViewData()).ToList() : null,
            };
            return model;
        }

        public static IList<ProvinceData> ToViewList(this IQueryable<Province> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new ProvinceData()
            {
                Id = node.Id,
                Name = node.Name,
                Sort = node.Sort,
                Remark = node.Remark,
                Buildings = (suffix & CategoryDictionary.Building) == CategoryDictionary.Building ? node.Buildings.Select(x => x.ToViewData()).ToList() : null,
                Organizations = (suffix & CategoryDictionary.Organization) == CategoryDictionary.Organization ? node.Organizations.Select(x => x.ToViewData()).ToList() : null,
                Cities = (suffix & CategoryDictionary.City) == CategoryDictionary.City ? node.Cities.Select(x => x.ToViewData()).ToList() : null,

            }).ToList();
            return results;
        }

        public static Province ToModel(this ProvinceData node)
        {
            return new Province()
            {
                Id = node.Id,
                Name = node.Name,
                Sort = node.Sort,
                Remark = node.Remark
            };
        }
        #endregion

    }
}
