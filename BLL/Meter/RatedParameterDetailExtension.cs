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
    public static class RatedParameterDetailExtension
    {
        #region RatedParameterDetail
        public static RatedParameterDetailData ToViewData(this RatedParameterDetail node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            BrandBLL brandBLL = new BrandBLL();
            MeterBLL meterBLL = new MeterBLL();
            UserBLL userBLL = new UserBLL();
            if (node == null)
                return null;
            return new RatedParameterDetailData()
            {
                Id = node.Id,
                RatedParameterId = node.RatedParameterId,
                BrandId = node.BrandId,
                MeterId = node.MeterId,
                SettingTime = node.SettingTime,
                IsSuccess = node.IsSuccess,
                OperatorId = node.OperatorId,
                OperatorName = node.OperatorName,
                Brand = (suffix & CategoryDictionary.Brand) == CategoryDictionary.Brand ? (node.Brand == null ? brandBLL.Find(node.BrandId).ToViewData() : node.Brand.ToViewData()) : null,
                Meter = (suffix & CategoryDictionary.Meter) == CategoryDictionary.Meter ? (node.Meter == null ? meterBLL.Find(node.MeterId).ToViewData() : node.Meter.ToViewData()) : null,
                Operator = (suffix & CategoryDictionary.User) == CategoryDictionary.User ? node.OperatorId == null ? null : (node.Operator == null ? userBLL.Find(node.OperatorId).ToViewData() : node.Operator.ToViewData()) : null,


            };
        }

        public static IList<RatedParameterDetailData> ToViewList(this IQueryable<RatedParameterDetail> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            BrandBLL brandBLL = new BrandBLL();
            MeterBLL meterBLL = new MeterBLL();
            UserBLL userBLL = new UserBLL();
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new RatedParameterDetailData()
            {
                Id = node.Id,
                RatedParameterId = node.RatedParameterId,
                BrandId = node.BrandId,
                MeterId = node.MeterId,
                SettingTime = node.SettingTime,
                IsSuccess = node.IsSuccess,
                OperatorId = node.OperatorId,
                OperatorName = node.OperatorName,
                Brand = (suffix & CategoryDictionary.Brand) == CategoryDictionary.Brand ? (node.Brand == null ? brandBLL.Find(node.BrandId).ToViewData() : node.Brand.ToViewData()) : null,
                Meter = (suffix & CategoryDictionary.Meter) == CategoryDictionary.Meter ? (node.Meter == null ? meterBLL.Find(node.MeterId).ToViewData() : node.Meter.ToViewData()) : null,
                Operator = (suffix & CategoryDictionary.User) == CategoryDictionary.User ? node.OperatorId==null?null:(node.Operator == null ? userBLL.Find(node.OperatorId).ToViewData() : node.Operator.ToViewData()) : null,

            }).ToList();
            return results;
        }

        public static RatedParameterDetail ToModel(this RatedParameterDetailData node)
        {
            return new RatedParameterDetail()
            {
                Id = node.Id,
                RatedParameterId = node.RatedParameterId,
                BrandId = node.BrandId,
                MeterId = node.MeterId,
                SettingTime = node.SettingTime,
                IsSuccess = node.IsSuccess,
                OperatorId = node.OperatorId,
                OperatorName = node.OperatorName,

            };
        }
        #endregion

    }
}
