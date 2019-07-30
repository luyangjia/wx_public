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
    public static class DistrictExtension
    {
        #region District
        public static DistrictData ToViewData(this District node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var model = new DistrictData()
            {
                Id = node.Id,
                Name = node.Name,
                Sort = node.Sort,
                CityId = node.CityId,
                City = (suffix & CategoryDictionary.City) == CategoryDictionary.City ? node.City.ToViewData() : null,
                Buildings = (suffix & CategoryDictionary.Building) == CategoryDictionary.Building ? node.Buildings.Select(x => x.ToViewData()).ToList() : null,
                Organizations = (suffix & CategoryDictionary.Organization) == CategoryDictionary.Organization ? node.Organizations.Select(x => x.ToViewData()).ToList() : null,
            };
            return model;
        }

        public static IList<DistrictData> ToViewList(this IQueryable<District> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new DistrictData()
            {
                Id = node.Id,
                Name = node.Name,
                Sort = node.Sort,
                CityId = node.CityId,
                City = (suffix & CategoryDictionary.City) == CategoryDictionary.City ? node.City.ToViewData() : null,
                Buildings = (suffix & CategoryDictionary.Building) == CategoryDictionary.Building ? node.Buildings.Select(x => x.ToViewData()).ToList() : null,
                Organizations = (suffix & CategoryDictionary.Organization) == CategoryDictionary.Organization ? node.Organizations.Select(x => x.ToViewData()).ToList() : null,
            }).ToList();

            return results;
        }

        public static District ToModel(this DistrictData node)
        {
            return new District()
            {
                Id = node.Id,
                Name = node.Name,
                Sort = node.Sort,
                CityId = node.CityId
            };
        }
        #endregion

    }
}
