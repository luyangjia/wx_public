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
    public static class ConfigDetailExtension
    {
        #region ConfigDetail
        public static ConfigDetailData ToViewData(this ConfigDetail node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var result = new ConfigDetailData()
            {
                Id = node.Id,
                TemplateId = node.TemplateId,
                MeterId = node.MeterId,
                BuildingId = node.BuildingId,
                OrganizationId = node.OrganizationId,
                Enabled = node.Enabled,
                OperatorId = node.OperatorId,
                OperatorName = node.OperatorName,
                CreateTime = node.CreateTime,
                BuildingCategoryId = node.BuildingCategoryId,
                EnergyCategoryId = node.EnergyCategoryId,
                IsOpenOverLoadAlert = node.IsOpenOverLoadAlert,
                IsOpenMalignantLoadAlert = node.IsOpenMalignantLoadAlert,
                IsControlPower = node.IsControlPower,
                IsControlWater = node.IsControlWater,
                IsControlWaterByPower = node.IsControlWaterByPower,
                VacationTimeControlTemplateId = node.VacationTimeControlTemplateId,
                HolidayTimeControlTemplateId = node.HolidayTimeControlTemplateId,
                WeekEndTimeControlTemplateId = node.WeekEndTimeControlTemplateId,
                PeacetimeTimeControlTemplateId = node.PeacetimeTimeControlTemplateId,
                IsControlByAccount = node.IsControlByAccount,
                IsControlByTime = node.IsControlByTime,
                MinThresholdForMalignantLoad=node.MinThresholdForMalignantLoad,
                MinThresholdForOverLoad = node.MinThresholdForOverLoad

            };
            using (MonitoringConfigBLL configBLL = new MonitoringConfigBLL())
            {
                if (node.MonitoringConfigTemplate != null)
                    result.Template = node.MonitoringConfigTemplate.ToViewData();
                else
                {

                    result.Template = configBLL.Find(node.TemplateId).ToViewData();

                }
                if (node.MonitoringConfigTemplate == null)
                    result.VacationTimeControlTemplate = configBLL.Find(node.VacationTimeControlTemplateId).ToViewData();
                else
                    result.VacationTimeControlTemplate = node.VacationTimeControlTemplate.ToViewData();

                if (node.HolidayTimeControlTemplate == null)
                    result.HolidayTimeControlTemplate = configBLL.Find(node.HolidayTimeControlTemplateId).ToViewData();
                else
                    result.HolidayTimeControlTemplate = node.HolidayTimeControlTemplate.ToViewData();

                if (node.WeekEndTimeControlTemplate == null)
                    result.WeekEndTimeControlTemplate = configBLL.Find(node.WeekEndTimeControlTemplateId).ToViewData();
                else
                    result.WeekEndTimeControlTemplate = node.WeekEndTimeControlTemplate.ToViewData();

                if (node.PeacetimeTimeControlTemplate == null)
                    result.PeacetimeTimeControlTemplate = configBLL.Find(node.PeacetimeTimeControlTemplateId).ToViewData();
                else
                    result.PeacetimeTimeControlTemplate = node.PeacetimeTimeControlTemplate.ToViewData();
            }
            return result;
        }
        public static ConfigDetailData ToAllViewData(this ConfigDetail node, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (node == null)
                return null;
            var result = new ConfigDetailData()
            {
                Id = node.Id,
                TemplateId = node.TemplateId,
                MeterId = node.MeterId,
                BuildingId = node.BuildingId,
                OrganizationId = node.OrganizationId,
                Enabled = node.Enabled,
                OperatorId = node.OperatorId,
                OperatorName = node.OperatorName,
                CreateTime = node.CreateTime,
                BuildingCategoryId = node.BuildingCategoryId,
                EnergyCategoryId = node.EnergyCategoryId,
                IsOpenOverLoadAlert = node.IsOpenOverLoadAlert,
                IsOpenMalignantLoadAlert = node.IsOpenMalignantLoadAlert,
                IsControlPower = node.IsControlPower,
                IsControlWater = node.IsControlWater,
                IsControlWaterByPower = node.IsControlWaterByPower,
                VacationTimeControlTemplateId = node.VacationTimeControlTemplateId,
                HolidayTimeControlTemplateId = node.HolidayTimeControlTemplateId,
                WeekEndTimeControlTemplateId = node.WeekEndTimeControlTemplateId,
                PeacetimeTimeControlTemplateId = node.PeacetimeTimeControlTemplateId,
                IsControlByAccount = node.IsControlByAccount,
                IsControlByTime = node.IsControlByTime,
                MinThresholdForMalignantLoad = node.MinThresholdForMalignantLoad,
                MinThresholdForOverLoad = node.MinThresholdForOverLoad

            };
            using (MonitoringConfigBLL configBLL = new MonitoringConfigBLL())
            {
                if (node.Template.ConfigTypeId != (DictionaryCache.MonitoringConfigTypePrice.Id + 5))
                    if (node.MonitoringConfigTemplate != null)
                        result.Template = node.MonitoringConfigTemplate.ToViewData();
                    else
                    {

                        result.Template = configBLL.Find(node.TemplateId).ToViewData();
                    }
                if (node.MonitoringConfigTemplate == null)
                    result.VacationTimeControlTemplate = configBLL.Find(node.VacationTimeControlTemplateId).ToViewData();
                else
                    result.VacationTimeControlTemplate = node.VacationTimeControlTemplate.ToViewData();

                if (node.HolidayTimeControlTemplate == null)
                    result.HolidayTimeControlTemplate = configBLL.Find(node.HolidayTimeControlTemplateId).ToViewData();
                else
                    result.HolidayTimeControlTemplate = node.HolidayTimeControlTemplate.ToViewData();

                if (node.WeekEndTimeControlTemplate == null)
                    result.WeekEndTimeControlTemplate = configBLL.Find(node.WeekEndTimeControlTemplateId).ToViewData();
                else
                    result.WeekEndTimeControlTemplate = node.WeekEndTimeControlTemplate.ToViewData();

                if (node.PeacetimeTimeControlTemplate == null)
                    result.PeacetimeTimeControlTemplate = configBLL.Find(node.PeacetimeTimeControlTemplateId).ToViewData();
                else
                    result.PeacetimeTimeControlTemplate = node.PeacetimeTimeControlTemplate.ToViewData();

            }
            return result;
        }

        public static IList<ConfigDetailData> ToViewList(this IQueryable<ConfigDetail> nodes, CategoryDictionary suffix = CategoryDictionary.None)
        {
            if (nodes == null)
                return null;
            var nodeList = nodes.ToList();
            var results = nodeList.Select(node => new ConfigDetailData()
            {
                Id = node.Id,
                TemplateId = node.TemplateId,
                MeterId = node.MeterId,
                BuildingId = node.BuildingId,
                OrganizationId = node.OrganizationId,
                Enabled = node.Enabled,
                OperatorId = node.OperatorId,
                OperatorName = node.OperatorName,
                CreateTime = node.CreateTime,
                BuildingCategoryId = node.BuildingCategoryId,
                EnergyCategoryId = node.EnergyCategoryId,
                IsOpenOverLoadAlert = node.IsOpenOverLoadAlert,
                IsOpenMalignantLoadAlert = node.IsOpenMalignantLoadAlert,
                IsControlPower = node.IsControlPower,
                IsControlWater = node.IsControlWater,
                IsControlWaterByPower = node.IsControlWaterByPower,
                VacationTimeControlTemplateId = node.VacationTimeControlTemplateId,
                HolidayTimeControlTemplateId = node.HolidayTimeControlTemplateId,
                WeekEndTimeControlTemplateId = node.WeekEndTimeControlTemplateId,
                PeacetimeTimeControlTemplateId = node.PeacetimeTimeControlTemplateId,
                IsControlByAccount = node.IsControlByAccount,
                IsControlByTime = node.IsControlByTime,
                MinThresholdForMalignantLoad = node.MinThresholdForMalignantLoad,
                MinThresholdForOverLoad = node.MinThresholdForOverLoad

            }).ToList();
            using (MonitoringConfigBLL configBLL = new MonitoringConfigBLL())
            {
                for (int i = 0; i < results.Count; i++)
                {
                    if (nodeList[i].MonitoringConfigTemplate != null)
                        results[i].Template = nodeList[i].MonitoringConfigTemplate.ToViewData();
                    else
                    {

                        MonitoringConfigData config = null;
                        for (int j = 0; j < i; j++)
                        {
                            if (results[j].TemplateId == results[i].TemplateId)
                                config = results[j].Template;
                        }
                        if (config == null)
                            results[i].Template = configBLL.Find(nodeList[i].TemplateId).ToViewData();
                        else
                            results[i].Template = config;
                    }
                    if (nodeList[i].MonitoringConfigTemplate == null)
                        results[i].VacationTimeControlTemplate = configBLL.Find(nodeList[i].VacationTimeControlTemplateId).ToViewData();
                    else
                        results[i].VacationTimeControlTemplate = nodeList[i].VacationTimeControlTemplate.ToViewData();

                    if (nodeList[i].HolidayTimeControlTemplate == null)
                        results[i].HolidayTimeControlTemplate = configBLL.Find(nodeList[i].HolidayTimeControlTemplateId).ToViewData();
                    else
                        results[i].HolidayTimeControlTemplate = nodeList[i].HolidayTimeControlTemplate.ToViewData();

                    if (nodeList[i].WeekEndTimeControlTemplate == null)
                        results[i].WeekEndTimeControlTemplate = configBLL.Find(nodeList[i].WeekEndTimeControlTemplateId).ToViewData();
                    else
                        results[i].WeekEndTimeControlTemplate = nodeList[i].WeekEndTimeControlTemplate.ToViewData();

                    if (nodeList[i].PeacetimeTimeControlTemplate == null)
                        results[i].PeacetimeTimeControlTemplate = configBLL.Find(nodeList[i].PeacetimeTimeControlTemplateId).ToViewData();
                    else
                        results[i].PeacetimeTimeControlTemplate = nodeList[i].PeacetimeTimeControlTemplate.ToViewData();

                }
            }
            return results;
        }

        public static ConfigDetail ToModel(this ConfigDetailData node)
        {
            return new ConfigDetail()
            {
                Id = node.Id,
                TemplateId = node.TemplateId,
                MeterId = node.MeterId,
                BuildingId = node.BuildingId,
                OrganizationId = node.OrganizationId,
                Enabled = node.Enabled,
                OperatorId = node.OperatorId,
                OperatorName = node.OperatorName,
                CreateTime = node.CreateTime,
                BuildingCategoryId = node.BuildingCategoryId,
                EnergyCategoryId = node.EnergyCategoryId,
                IsOpenOverLoadAlert = node.IsOpenOverLoadAlert,
                IsOpenMalignantLoadAlert = node.IsOpenMalignantLoadAlert,
                IsControlPower = node.IsControlPower,
                IsControlWater = node.IsControlWater,
                IsControlWaterByPower = node.IsControlWaterByPower,
                VacationTimeControlTemplateId = node.VacationTimeControlTemplateId,
                HolidayTimeControlTemplateId = node.HolidayTimeControlTemplateId,
                WeekEndTimeControlTemplateId = node.WeekEndTimeControlTemplateId,
                PeacetimeTimeControlTemplateId = node.PeacetimeTimeControlTemplateId,
                IsControlByAccount = node.IsControlByAccount,
                IsControlByTime = node.IsControlByTime,
                MinThresholdForMalignantLoad = node.MinThresholdForMalignantLoad,
                MinThresholdForOverLoad = node.MinThresholdForOverLoad

            };
        }
        #endregion


    }
}
