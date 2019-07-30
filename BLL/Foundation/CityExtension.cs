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
    public static class CityExtension
    {
        public static CityData ToViewData(this City node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var model = new CityData()
            {
                Id = node.Id,
                Name = node.Name,
                ProvinceId = node.ProvinceId,
                Sort = node.Sort,
                Province = (suffix & CategoryDictionary.Province) == CategoryDictionary.Province ? node.Province.ToViewData() : null,
                Buildings = (suffix & CategoryDictionary.Building) == CategoryDictionary.Building ? node.Buildings.Select(x => x.ToViewData()).ToList() : null,
                Organizations = (suffix & CategoryDictionary.Organization) == CategoryDictionary.Organization ? node.Organizations.Select(x => x.ToViewData()).ToList() : null,
                Districts = (suffix & CategoryDictionary.District) == CategoryDictionary.District ? node.Districts.Select(x => x.ToViewData()).ToList() : null,
            };
            return model;
        }

        public static IList<CityData> ToViewList(this IQueryable<City> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new CityData()
            {
                Id = node.Id,
                Name = node.Name,
                ProvinceId = node.ProvinceId,
                Sort = node.Sort,
                Province = (suffix & CategoryDictionary.Province) == CategoryDictionary.Province ? node.Province.ToViewData() : null,
                Buildings = (suffix & CategoryDictionary.Building) == CategoryDictionary.Building ? node.Buildings.Select(x => x.ToViewData()).ToList() : null,
                Organizations = (suffix & CategoryDictionary.Organization) == CategoryDictionary.Organization ? node.Organizations.Select(x => x.ToViewData()).ToList() : null,
                Districts = (suffix & CategoryDictionary.District) == CategoryDictionary.District ? node.Districts.Select(x => x.ToViewData()).ToList() : null,
            
            }).ToList();
            return results;
        }

        public static City ToModel(this CityData node)
        {
            return new City()
            {
                Id = node.Id,
                Name = node.Name,
                ProvinceId = node.ProvinceId,
                Sort = node.Sort
            };
        }

    }
}
