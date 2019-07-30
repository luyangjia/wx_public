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
    public static class RatedParameterExtension
    {
        #region RatedParameter
        public static RatedParameterData ToViewData(this RatedParameter node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new RatedParameterData()
            {
                Id = node.Id,
                RatedParameterTypeId = node.RatedParameterTypeId,
                BrandId = node.BrandId,
                ParameterId = node.ParameterId,
                Description = node.Description,
                MinValue = node.MinValue,
                MaxValue = node.MaxValue,
                Code = node.Code,
                SettingTime = node.SettingTime,
                PPF = node.PPF,
                RPF = node.RPF,
                PPFMax=node.PPFMax,
                PPFMin=node.PPFMin,
                RPFMax=node.RPFMax,
                RPFMin=node.RPFMin,
                Parameter = (suffix & CategoryDictionary.Parameter) == CategoryDictionary.Parameter ? node.Parameter.ToViewData() : null
            };
        }

        public static IList<RatedParameterData> ToViewList(this IQueryable<RatedParameter> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new RatedParameterData()
            {
                Id = node.Id,
                RatedParameterTypeId = node.RatedParameterTypeId,
                BrandId = node.BrandId,
                ParameterId = node.ParameterId,
                Description = node.Description,
                MinValue = node.MinValue,
                MaxValue = node.MaxValue,
                Code = node.Code,
                SettingTime = node.SettingTime,
                PPF = node.PPF,
                RPF = node.RPF,
                PPFMax = node.PPFMax,
                PPFMin = node.PPFMin,
                RPFMax = node.RPFMax,
                RPFMin = node.RPFMin,
                Parameter = (suffix & CategoryDictionary.Parameter) == CategoryDictionary.Parameter ? node.Parameter.ToViewData() : null
            }).ToList();
            return results;
        }

        public static RatedParameter ToModel(this RatedParameterData node)
        {
            return new RatedParameter()
            {
                Id = node.Id,
                RatedParameterTypeId = node.RatedParameterTypeId,
                BrandId = node.BrandId,
                ParameterId = node.ParameterId,
                Description = node.Description,
                MinValue = node.MinValue,
                MaxValue = node.MaxValue,
                Code = node.Code,
                SettingTime = node.SettingTime,
                PPF = node.PPF,
                RPF = node.RPF,
                PPFMax = node.PPFMax,
                PPFMin = node.PPFMin,
                RPFMax = node.RPFMax,
                RPFMin = node.RPFMin

            };
        }
        #endregion

    }
}
