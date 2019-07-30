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
    public static class BrandExtension
    {
        public static BrandData ToViewData(this Brand node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new BrandData()
            {
                Id = node.Id,
                Name = node.Name,
                MeterType = node.MeterType,
                Producer = node.Producer,
                MeterTypeName = node.TypeDict.ChineseName,
                Model = node.Model,
                Description = node.Description,
                IsControllable = node.IsControllable,
                IsFJNewcapSystem=node.IsFJNewcapSystem
            };
        }

        public static IList<BrandData> ToViewList(this IQueryable<Brand> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return nodes.ToList().Select(node => new BrandData()
            {
                Id = node.Id,
                Name = node.Name,
                MeterTypeName = node.TypeDict == null ? DictionaryCache.Get()[node.MeterType].ChineseName : node.TypeDict.ChineseName,
                MeterType = node.MeterType,
                Producer = node.Producer,
                Model = node.Model,
                Description = node.Description,
                IsControllable = node.IsControllable,
                IsFJNewcapSystem = node.IsFJNewcapSystem
            }).ToList();
        }

        public static Brand ToModel(this BrandData node)
        {
            return new Brand()
            {
                Id = node.Id,
                Name = node.Name,
                MeterType = node.MeterType,
                Producer = node.Producer,
                Model = node.Model,
                Description = node.Description,
                IsControllable = node.IsControllable,
                IsFJNewcapSystem = node.IsFJNewcapSystem
                //Parameters = node.Parameters.Select(x => x.ToViewData())
            };
        }
    }
}
