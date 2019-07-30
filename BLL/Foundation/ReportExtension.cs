using WxPay2017.API.DAL.EmpModels;
using WxPay2017.API.VO.Common;
using WxPay2017.API.VO.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WxPay2017.API.BLL
{
    public static class ReportExtension
    {
        #region Report
        public static ReportData ToViewData(this Report node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            return new ReportData()
            {
                Id = node.Id,
                DataExtractTime = node.DataExtractTime,
                DataPackTime = node.DataPackTime,
                DataPackXml = node.DataPackXml,
                DataReportTime = node.DataReportTime,
                DataReciveTime = node.DataReciveTime,
                DataReciveXml = node.DataReciveXml,

            };
        }

        public static IList<ReportData> ToViewList(this IQueryable<Report> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new ReportData()
            {
                Id = node.Id,
                DataExtractTime = node.DataExtractTime,
                DataPackTime = node.DataPackTime,
                DataPackXml = node.DataPackXml,
                DataReportTime = node.DataReportTime,
                DataReciveTime = node.DataReciveTime,
                DataReciveXml = node.DataReciveXml,

            }).ToList();
            return results;
        }

        public static Report ToModel(this ReportData node)
        {
            return new Report()
            {
                Id = node.Id,
                DataExtractTime = node.DataExtractTime,
                DataPackTime = node.DataPackTime,
                DataPackXml = node.DataPackXml,
                DataReportTime = node.DataReportTime,
                DataReciveTime = node.DataReciveTime,
                DataReciveXml = node.DataReciveXml,

            };
        }
        #endregion


    }
}
