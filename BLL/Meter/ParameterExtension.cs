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
    public static class ParameterExtension
    {
        public static ParameterData ToViewData(this Parameter node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new ParameterData()
            {
                Id = node.Id,
                BrandId = node.BrandId,
                DictionaryId = node.TypeId,
                Unit = node.Unit
            };
        }

        public static IEnumerable<ParameterData> ToViewList(this IQueryable<Parameter> node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            return node.ToList().Select(x => x.ToViewData(suffix));
        }

        public static Parameter ToModel(this ParameterData node)
        {
            return new Parameter()
            {
                Id = node.Id,
                BrandId = node.BrandId,
                TypeId = node.DictionaryId,
                Unit = node.Unit
            };
        }
    }
}
